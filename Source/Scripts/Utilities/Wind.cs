using DG.Tweening;
using System.Linq;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private string interactionObjType;

    private Vector3 targetPosition;
    private float animationTime;
    private int rotateIndex;

    public void Init(Vector3 targetPosition, float animationTime, int rotateIndex)
    {
        this.targetPosition = targetPosition;
        this.animationTime = animationTime;
        this.rotateIndex = rotateIndex;

        Destroy(gameObject, animationTime + 0.05f);
    }

    private void Start()
    {
        transform.DOMove(targetPosition, animationTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActionWizardObject actionObj = collision.GetComponent<ActionWizardObject>();
        WizardActionsController actions = WizardActionsController.Instance;
        FourRotateOffsetArgs fourRotate = FourRotateOffsetArgs.fourRotates[rotateIndex];

        if (!actionObj)
            return;

        Vector3 moveTo = actions.SlimesTilemap.GetCellCenterWorld(actionObj.Coordinates.ElementAt(0) + new Vector3Int(fourRotate.TargetXOffset, fourRotate.TargetYOffset));
        Vector3Int moveToCellCoords = actionObj.Coordinates.ElementAt(0) + new Vector3Int(fourRotate.TargetXOffset, fourRotate.TargetYOffset);
        if (actionObj && actionObj.ObjectType == interactionObjType && !actions.TryGetActionObjectByCell(moveToCellCoords) && actions.SlimesTilemap.GetTile(moveToCellCoords))
        {
            GetComponent<Collider2D>().enabled = false;

            collision.transform.DOMove(moveTo, 0.5f).onComplete += () => actionObj.SetOnlyOneCoordinate(moveToCellCoords);
        }
    }
}