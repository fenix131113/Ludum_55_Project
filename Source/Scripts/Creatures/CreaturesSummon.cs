using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class CreaturesSummon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private float animationTime = 0.5f;
    [SerializeField] private RectTransform moveCellRect;
    [SerializeField] private GameObject targetHighlighter;
    [SerializeField] private Tilemap tilemapForPlacingCreatures;
    [SerializeField] private RectTransform[] openingButtons;
    [SerializeField] private CreatureCell[] cells = new CreatureCell[0];
    
    public GameObject TargetHighlighter => targetHighlighter;
    private CreatureCell takedCell;
    private bool canClick = true;
    private bool isMenuOpened;
    private bool isMovingCreatureCell;

    public CreatureCell TakedCell { get { return takedCell; } set { takedCell = value; } }

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

            if (tilemapForPlacingCreatures.GetTile(tilemapForPlacingCreatures.WorldToCell(mouseWorldPos)) && takedCell)
            {
                Instantiate(takedCell.CreaturePrefab, tilemapForPlacingCreatures.GetCellCenterWorld(tilemapForPlacingCreatures.WorldToCell(mouseWorldPos)), Quaternion.identity)
                    .GetComponent<WizardBase>().Init(this, tilemapForPlacingCreatures.WorldToCell(mouseWorldPos), tilemapForPlacingCreatures);
                takedCell.CanUse = false;
                takedCell = null;
            }

            DeactivateMovingCell();
        }
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