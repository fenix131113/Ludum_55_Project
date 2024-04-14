using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WizardBase : MonoBehaviour
{
    [SerializeField] protected GameObject highlighterPrefab;
    [SerializeField] protected int actionLineLenght = 2;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    protected WizardsSummon creatureSummonController;
    protected Vector3Int cellIntPos;
    protected Tilemap selfPlacingTilemap;
    protected readonly FourRotateOffsetArgs[] targetRotateVariants = new FourRotateOffsetArgs[4] { new(-1, 0), new(0, 1), new(1, 0), new(0, -1) };
    protected int currentTargetHighlightRotIndex;
    protected GameObject[] highlightersLine = new GameObject[0];

    public Tilemap SelfPlacingTilemap => selfPlacingTilemap;
    public int CurrentTargetHighlightRotIndex => currentTargetHighlightRotIndex;
    public IEnumerable TargetRotateVariants => highlightersLine;
    public int ActionLineLenght => actionLineLenght;

    public void Init(WizardsSummon creatureSummonController, Vector3Int cellIntPos, Tilemap selfPlacingTilemap)
    {
        BeforeInit();
        this.selfPlacingTilemap = selfPlacingTilemap;
        highlightersLine = new GameObject[actionLineLenght];
        this.creatureSummonController = creatureSummonController;
        this.cellIntPos = cellIntPos;
        GetComponent<ActionWizardObject>().SetOnlyOneCoordinate(cellIntPos);
        AfterInit();
    }
    public virtual void WizardAction()
    {
        // Wizard action
    }

    protected virtual void AfterInit()
    {
        // Invoke after Init method
    }

    protected virtual void BeforeInit()
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
    protected virtual void RotateWizard()
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
    protected virtual void HighlightTarget()
    {
        ClearHighlightArray();

        for (int i = 0; i < highlightersLine.Length; i++)
        {
            Vector3Int cellPos = cellIntPos + new Vector3Int(targetRotateVariants[currentTargetHighlightRotIndex].TargetXOffset * (i + 1),
            targetRotateVariants[currentTargetHighlightRotIndex].TargetYOffset * (i + 1));
            if (selfPlacingTilemap.GetTile(cellPos))
            {
                ActionWizardObject actionObjectOnCell = WizardActionsController.Instance.TryGetActionObjectByCell(currentTargetHighlightRotIndex == 3 ? cellPos + new Vector3Int(0, 1, 0) : cellPos);
                if (actionObjectOnCell && actionObjectOnCell.IsSlime && actionObjectOnCell.gameObject.GetHashCode() != gameObject.GetHashCode())
                    return;


                highlightersLine[i] = Instantiate(highlighterPrefab, selfPlacingTilemap.GetCellCenterWorld(cellPos), Quaternion.identity);
            }
            else
                return;

        }
    }
    protected virtual void DeactivateTargetHighlight()
    {
        ClearHighlightArray();
    }

    protected virtual void ClearHighlightArray()
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
        RotateWizard();
    }
}

public class FourRotateOffsetArgs
{
    public static FourRotateOffsetArgs[] fourRotates => new FourRotateOffsetArgs[4] { new(-1, 0), new(0, 1), new(1, 0), new(0, -1) };
    private int targetXOffset;
    private int targetYOffset;

    public int TargetXOffset => targetXOffset;
    public int TargetYOffset => targetYOffset;

    public FourRotateOffsetArgs(int targetXOffset, int targetYOffset)
    {
        this.targetXOffset = targetXOffset;
        this.targetYOffset = targetYOffset;
    }

    public static float GetRotateByIndexRotate(int index)
    {
        switch (index)
        {
            case 0:
                return 180;
            case 1:
                return 90;
            case 2:
                return 0;
            case 3:
                return -90;
        }
        return -1;
    }
}