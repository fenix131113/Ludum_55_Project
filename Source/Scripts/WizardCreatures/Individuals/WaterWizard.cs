using UnityEngine;

public class WaterWizard : WizardBase
{
    [SerializeField] private float shootInterval;
    [SerializeField] private GameObject iceballPrefab;

    private void Start()
    {
        InvokeRepeating(nameof(WizardAction), shootInterval, shootInterval);
    }
    public override void WizardAction()
    {
        WizardActionsController actions = WizardActionsController.Instance;
        Iceball iceball = Instantiate(iceballPrefab, actions.SlimesTilemap.GetCellCenterWorld(cellIntPos), Quaternion.Euler(0, 0, FourRotateOffsetArgs.GetRotateByIndexRotate(currentTargetHighlightRotIndex))).GetComponent<Iceball>();
        iceball.Init(currentTargetHighlightRotIndex, gameObject, ActionLineLenght + 0.2f, transform);
    }
}