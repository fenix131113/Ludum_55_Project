using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WizardBase : MonoBehaviour
{
	[SerializeField] protected GameObject highlighterPrefab;
	[SerializeField] protected int actionLineLenght = 2;
	[SerializeField] protected SpriteRenderer spriteRenderer;

	protected CreaturesSummon creatureSummonController;
	protected Vector3Int cellIntPos;
	protected Tilemap selfPlacingTilemap;
	protected readonly TargetHighlighterOffsetArgs[] targetRotateVariants = new TargetHighlighterOffsetArgs[4] { new(-1, 0), new(0, 1), new(1, 0), new(0, -1) };
	protected int currentTargetHighlightRotIndex;
	protected GameObject[] highlightersLine = new GameObject[0];

	public Tilemap SelfPlacingTilemap => selfPlacingTilemap;
	public int CurrentTargetHighlightRotIndex => currentTargetHighlightRotIndex;
	public IEnumerable TargetRotateVariants => highlightersLine;
	public int ActionLineLenght => actionLineLenght;

	public void Init(CreaturesSummon creatureSummonController, Vector3Int cellIntPos, Tilemap selfPlacingTilemap)
	{
		BeforeInit();
        this.selfPlacingTilemap = selfPlacingTilemap;
        highlightersLine = new GameObject[actionLineLenght];
		this.creatureSummonController = creatureSummonController;
		this.cellIntPos = cellIntPos;
        AfterInit();
	}

	public virtual void AfterInit()
	{
		// Invoke after Init method
	}

	public virtual void BeforeInit()
	{
		// Invoke before Init method
	}
	public List<Vector3Int> GetLineTilemapCellsCoordinates()
	{
		List<Vector3Int> lineCoordiates = new();

		for (int i = 0; i < actionLineLenght; i++)
		{
			lineCoordiates.Add(cellIntPos + new Vector3Int(targetRotateVariants[currentTargetHighlightRotIndex].TargetXOffset * (i + 1),
                targetRotateVariants[currentTargetHighlightRotIndex].TargetYOffset * (i + 1)));
		}
		return lineCoordiates;
	}
	protected void RotateWizard()
	{
		if (currentTargetHighlightRotIndex + 1 >= targetRotateVariants.Length)
			currentTargetHighlightRotIndex = 0;
		else
			currentTargetHighlightRotIndex++;


		if (currentTargetHighlightRotIndex == 0)
			spriteRenderer.flipX = true;
		else if (currentTargetHighlightRotIndex == 2)
			spriteRenderer.flipX = false;


            HighlightTarget();
	}
	protected void HighlightTarget()
	{
		ClearHighlightArray();

		for (int i = 0; i < highlightersLine.Length; i++)
			if (selfPlacingTilemap.GetTile(cellIntPos + new Vector3Int(targetRotateVariants[currentTargetHighlightRotIndex].TargetXOffset * (i + 1),
			targetRotateVariants[currentTargetHighlightRotIndex].TargetYOffset * (i + 1))))
				highlightersLine[i] = Instantiate(highlighterPrefab, selfPlacingTilemap.GetCellCenterWorld
				(cellIntPos + new Vector3Int(targetRotateVariants[currentTargetHighlightRotIndex].TargetXOffset * (i + 1),
				targetRotateVariants[currentTargetHighlightRotIndex].TargetYOffset * (i + 1), 0)), Quaternion.identity);
			else
				return;
	}

	protected void DeactivateTargetHighlight()
	{
		ClearHighlightArray();
	}

	protected void ClearHighlightArray()
	{
		if (highlightersLine.Length > 0)
			foreach (var obj in highlightersLine)
				Destroy(obj);
	}

	public void OnMouseEnter()
	{
		HighlightTarget();
	}

	public void OnMouseExit()
	{
		DeactivateTargetHighlight();
	}
	public void OnMouseDown()
	{
		//if (Input.GetMouseButton(0))
		RotateWizard();
	}

	protected class TargetHighlighterOffsetArgs
	{
		private int targetXOffset;
		private int targetYOffset;

		public int TargetXOffset => targetXOffset;
		public int TargetYOffset => targetYOffset;

		public TargetHighlighterOffsetArgs(int targetXOffset, int targetYOffset)
		{
			this.targetXOffset = targetXOffset;
			this.targetYOffset = targetYOffset;
		}
	}
}