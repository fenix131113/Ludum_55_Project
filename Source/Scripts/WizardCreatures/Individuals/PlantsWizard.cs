using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlantsWizard : WizardBase
{
    [SerializeField] private float grassChangePeriod = 5f;

    private List<GameObject> grassHighliters = new();

    protected override void AfterInit()
    {
        highlightersLine = new GameObject[WizardActionsController.Instance.GrassBlocks.Count];
        InvokeRepeating(nameof(WizardAction), grassChangePeriod, grassChangePeriod);
    }

    protected override void RotateWizard()
    {
        return;
    }

    public override void OnClick()
    {
        return;
    }

    public override void WizardAction()
    {
        WizardActionsController actions = WizardActionsController.Instance;
        foreach (var block in actions.GrassBlocks)
            block.ChangeState();
    }

    protected override void HighlightTarget()
    {
        WizardActionsController actions = WizardActionsController.Instance;

        if (actions.GrassBlocks.Count > 0)
            foreach (var block in actions.GrassBlocks)
                grassHighliters.Add(Instantiate(highlighterPrefab, actions.SlimesTilemap.GetCellCenterWorld(block.Coordinates.ElementAt(0)), Quaternion.identity));
    }

    protected override void ClearHighlightArray()
    {
        foreach (GameObject obj in grassHighliters)
            Destroy(obj);
    }
}