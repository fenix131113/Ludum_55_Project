using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class WizardsSummon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private float animationTime = 0.5f;
    [SerializeField] private float summonYOffset;
    [SerializeField] private RectTransform moveCellRect;
    [SerializeField] private Tilemap tilemapForPlacingCreatures;
    [SerializeField] private RectTransform[] openingButtons;
    [SerializeField] private WizardCell[] cells = new WizardCell[0];
    [SerializeField] private GameObject spawnAnimPrefab;
    [SerializeField] private AnimationClip spawnAnimation;

    private WizardCell takedCell;
    private bool canClick = true;
    private bool isMenuOpened;
    private bool isMovingCreatureCell;

    public WizardCell TakedCell { get { return takedCell; } set { takedCell = value; } }

    private void Awake()
    {
        foreach (var cell in cells)
            cell.Init(this);
    }
    private void Update()
    {
        CheckMovingCell();
    }

    public void OpenMenu()
    {
        canClick = false;

        for (int i = 0; i < openingButtons.Length; i++)
        {
            RectTransform buttonRect = openingButtons[i];
            buttonRect.gameObject.SetActive(true);
            buttonRect.DOLocalMoveY(-110 * (i + 1), animationTime).onComplete += () => canClick = true;
        }

        isMenuOpened = true;
    }

    public void CloseMenu()
    {
        canClick = false;

        for (int i = 0; i < openingButtons.Length; i++)
        {
            RectTransform buttonRect = openingButtons[i];
            buttonRect.DOLocalMoveY(0, animationTime).onComplete += () => { buttonRect.gameObject.SetActive(false); canClick = true; };
        }

        isMenuOpened = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canClick)
            return;

        if (isMenuOpened)
            CloseMenu();
        else
            OpenMenu();
    }

    public void CheckMovingCell()
    {
        if (!isMovingCreatureCell)
            return;

        moveCellRect.position = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ActionWizardObject action = WizardActionsController.Instance.TryGetActionObjectByCell(tilemapForPlacingCreatures.WorldToCell(mouseWorldPos));

            if (tilemapForPlacingCreatures.GetTile(tilemapForPlacingCreatures.WorldToCell(mouseWorldPos)) && takedCell)
            {
                if (!action)
                    StartCoroutine(SpawnSlime(mouseWorldPos));
                else if (action.CanPlaceSlime)
                    StartCoroutine(SpawnSlime(mouseWorldPos));
            }

            DeactivateMovingCell();
        }
    }

    private IEnumerator SpawnSlime(Vector3 mouseWorldPos)
    {
        Destroy(Instantiate(spawnAnimPrefab, tilemapForPlacingCreatures.GetCellCenterWorld(tilemapForPlacingCreatures.WorldToCell(mouseWorldPos)), Quaternion.identity), spawnAnimation.length);
        yield return new WaitForSeconds(0.3f);
        GameObject createdWizard = Instantiate(takedCell.CreaturePrefab, tilemapForPlacingCreatures.GetCellCenterWorld(tilemapForPlacingCreatures.WorldToCell(mouseWorldPos)) + new Vector3(0, summonYOffset, 0), Quaternion.identity);
        createdWizard.GetComponent<WizardBase>().Init(this, tilemapForPlacingCreatures.WorldToCell(mouseWorldPos), tilemapForPlacingCreatures);
        takedCell.CanUse = false;
        takedCell = null;
    }
    public void ActivateMovingCell()
    {
        moveCellRect.gameObject.SetActive(true);
        isMovingCreatureCell = true;
    }

    public void DeactivateMovingCell()
    {
        moveCellRect.gameObject.SetActive(false);
        isMovingCreatureCell = false;
    }
}