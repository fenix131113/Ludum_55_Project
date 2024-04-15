using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtWizard : WizardBase
{
    [SerializeField] private float dirtChangeCooldown = 1.5f;

    private List<GameObject> dirtHighliters = new();
    private bool canChangeState = true;
    
    protected override void AfterInit()
    {
        highlightersLine = new GameObject[WizardActionsController.Instance.DirtBlocks.Count];
    }

    public override void OnClick()
    {
        if (canChangeState)
            WizardAction();
    }
    private IEnumerator StateCooldown()
    {
        yield return new WaitForSeconds(dirtChangeCooldown);

        canChangeState = true;
    }

    public override void WizardAction()
    {
        canChangeState = false;

        foreach (DirtBlock block in WizardActionsController.Instance.DirtBlocks)
            block.ChangeState();
        
        StartCoroutine(StateCooldown());
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