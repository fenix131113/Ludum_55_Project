using UnityEngine;

public class WindWizard : WizardBase
{
    [SerializeField] private GameObject windPrefab;
    [SerializeField] private AnimationClip windAnimClip;
    [SerializeField] private float shootInterval;
    private void Start()
    {
        InvokeRepeating(nameof(WizardAction), shootInterval, shootInterval);
    }
    public override void WizardAction()
    {
        WizardActionsController actions = WizardActionsController.Instance;
        FourRotateOffsetArgs fourRotates = FourRotateOffsetArgs.fourRotates[currentTargetHighlightRotIndex];
        Wind wind = Instantiate(windPrefab, actions.SlimesTilemap.GetCellCenterWorld(cellIntPos) + new Vector3(fourRotates.TargetXOffset, fourRotates.TargetYOffset, 0) * 0.3f, Quaternion.Euler(0, 0, FourRotateOffsetArgs.GetRotateByIndexRotate(currentTargetHighlightRotIndex))).GetComponent<Wind>();
        wind.Init(
            actions.SlimesTilemap.GetCellCenterWorld(cellIntPos + new Vector3Int(fourRotates.TargetXOffset, fourRotates.TargetYOffset, 0) * 2),
            windAnimClip.length, currentTargetHighlightRotIndex);
    }
}