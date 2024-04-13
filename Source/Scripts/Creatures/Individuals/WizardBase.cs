using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class WizardBase : MonoBehaviour, IPointerClickHandler
{
    private CreaturesSummon creatureSummonController;
    private Vector3Int cellIntPos;
    private Tilemap selfPlacingTilemap;

    public void Init(CreaturesSummon creatureSummonController, Vector3Int cellIntPos, Tilemap selfPlacingTilemap)
    {
        this.creatureSummonController = creatureSummonController;
        this.cellIntPos = cellIntPos;
        this.selfPlacingTilemap = selfPlacingTilemap;
    }

    protected void RotateWizard()
    {

        HighlightTarget();
    }
    protected void HighlightTarget()
    {
        creatureSummonController.TargetHighlighter.transform.position = selfPlacingTilemap.GetCellCenterWorld(cellIntPos - new Vector3Int(2, 0, 0));
        creatureSummonController.TargetHighlighter.SetActive(true);
    }

    protected void DeactivateTargetHighlight()
    {
        creatureSummonController.TargetHighlighter.SetActive(false);
    }

    public void OnMouseEnter()
    {
        HighlightTarget();
    }

    public void OnMouseExit()
    {
        DeactivateTargetHighlight();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            RotateWizard();
    }
}