using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

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
	[SerializeField] private Sprite[] buttonSprites;
	[SerializeField] private RectTransform slimeIconRect;
	[SerializeField] private float slimeIconOffset;
	[SerializeField] private UnityEvent onClick;

    private WizardCell takedCell;
	private bool canClick = true;
	private bool isMenuOpened;
	private bool isMovingCreatureCell;
	private Vector3 slimeIconStartPos;
	

	public WizardCell TakedCell { get { return takedCell; } set { takedCell = value; } }

	private void Awake()
	{
		slimeIconStartPos = slimeIconRect.localPosition;

		foreach (var cell in cells)
			cell.Init(this);
	}
	private void Update()
	{
		CheckMovingCell();
	}

	private void ButtonAnimation()
	{
        GetComponent<Image>().sprite = buttonSprites[1];
        slimeIconRect.DOLocalMoveY(slimeIconStartPos.y - slimeIconOffset, 0.05f);
        IEnumerator SetButtonSpriteBack() { yield return new WaitForSeconds(0.1f); GetComponent<Image>().sprite = buttonSprites[0]; slimeIconRect.DOLocalMoveY(slimeIconStartPos.y, 0.05f); }
        StartCoroutine(SetButtonSpriteBack());
    }
	public void OpenMenu()
	{
		canClick = false;

		ButtonAnimation();

        for (int i = 0; i < openingButtons.Length; i++)
		{
			RectTransform buttonRect = openingButtons[i];
			buttonRect.gameObject.SetActive(true);
			buttonRect.DOLocalMoveY(-120 * (i + 1), animationTime).onComplete += () => canClick = true;
		}

		isMenuOpened = true;
	}

	public void CloseMenu()
	{
		canClick = false;

        ButtonAnimation();

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
		onClick?.Invoke();
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
		takedCell.ExistIndicator.enabled = false;
		yield return new WaitForSeconds(0.3f);
		GameObject createdWizard = Instantiate(takedCell.CreaturePrefab, tilemapForPlacingCreatures.GetCellCenterWorld(tilemapForPlacingCreatures.WorldToCell(mouseWorldPos)) + new Vector3(0, summonYOffset, 0), Quaternion.identity);
		createdWizard.GetComponent<WizardBase>().Init(this, tilemapForPlacingCreatures.WorldToCell(mouseWorldPos), tilemapForPlacingCreatures);
		takedCell.CanUse = false;
		takedCell = null;
	}
	public void ActivateMovingCell()
	{
		Image moveImage = moveCellRect.GetComponent<Image>();
		moveImage.sprite = takedCell.ExistIndicator.sprite;
		moveImage.SetNativeSize();
        moveCellRect.gameObject.SetActive(true);
		isMovingCreatureCell = true;
	}

	public void DeactivateMovingCell()
	{

        moveCellRect.gameObject.SetActive(false);
		isMovingCreatureCell = false;
	}
}