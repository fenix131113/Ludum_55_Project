using UnityEngine;

public class FireWizard : WizardBase
{
    [SerializeField] private float shootInterval;
    [SerializeField] private GameObject fireballPrefab;

    private void Start()
    {
        InvokeRepeating(nameof(WizardAction), shootInterval, shootInterval);
    }
    public override void WizardAction()
    {
        WizardActionsController actions = WizardActionsController.Instance;
        Fireball fireball = Instantiate(fireballPrefab, actions.SlimesTilemap.GetCellCenterWorld(cellIntPos), Quaternion.Euler(0, 0, FourRotateOffsetArgs.GetRotateByIndexRotate(currentTargetHighlightRotIndex))).GetComponent<Fireball>();
        fireball.Init(currentTargetHighlightRotIndex, gameObject, ActionLineLenght + 0.2f, transform);
    }
}