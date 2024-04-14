using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlantsWizard : WizardBase
{
    private List<GameObject> grassHighliters = new();
    protected override void AfterInit() =>
        highlightersLine = new GameObject[WizardActionsController.Instance.GrassBlocks.Count];

    protected override void RotateWizard()
    {
        return;
    }
    protected override void HighlightTarget()
    {
        WizardActionsController actions = WizardActionsController.Instance;

        if (actions.GrassBlocks.Count > 0)
            foreach (var block in actions.GrassBlocks)
            {
                grassHighliters.Add(Instantiate(highlighterPrefab, actions.SlimesTilemap.GetCellCenterWorld(block.Coordinates.ElementAt(0)), Quaternion.identity));
                block.ChangeState();
            }
    }

    protected override void ClearHighlightArray()
    {
        foreach (GameObject obj in grassHighliters)
            Destroy(obj);
    }
}