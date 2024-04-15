using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlantsWizard : WizardBase
{
    [SerializeField] private float grassChangePeriod = 5f;
    [SerializeField] private AudioClip dirtSound;
    [SerializeField] private Canvas childCanvas;
    [SerializeField] private Image timerFillerImg;

    private List<GameObject> grassHighliters = new();
    private float timer;

    protected override void BeforeInit()
    {
        childCanvas.worldCamera = Camera.main;
    }
    protected override void AfterInit()
    {
        highlightersLine = new GameObject[WizardActionsController.Instance.GrassBlocks.Count];
    }
    private void Update()
    {
        TimerLogic();
    }

    private void TimerLogic()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            WizardAction();
            timer = grassChangePeriod;
        }

        timerFillerImg.fillAmount = (grassChangePeriod - timer) / grassChangePeriod;
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