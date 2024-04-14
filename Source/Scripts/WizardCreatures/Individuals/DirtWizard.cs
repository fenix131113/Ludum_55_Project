using System.Collections.Generic;
using UnityEngine;

public class DirtWizard : WizardBase
{
    private List<GameObject> dirtHighliters = new();
    protected override void AfterInit()
    {
        highlightersLine = new GameObject[WizardActionsController.Instance.DirtBlocks.Count];
        WizardAction();
    }

    protected override void RotateWizard()
    {
        return;
    }

    public override void WizardAction()
    {
        foreach (DirtBlock block in WizardActionsController.Instance.DirtBlocks)
            block.ChangeState();
    }

    protected override void HighlightTarget()
    {
        WizardActionsController actions = WizardActionsController.Instance;

        if (actions.DirtBlocks.Count > 0)
            foreach (DirtBlock block in actions.DirtBlocks)
                dirtHighliters.Add(Instantiate(highlighterPrefab, actions.SlimesTilemap.GetCellCenterWorld(block.GetFirstCoordinate), Quaternion.identity));
    }

    protected override void ClearHighlightArray()
    {
        foreach (GameObject obj in dirtHighliters)
            Destroy(obj);
    }
}