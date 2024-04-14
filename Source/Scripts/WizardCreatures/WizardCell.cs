using UnityEngine;
using UnityEngine.EventSystems;

public class WizardCell : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject creaturePrefab;

    private WizardsSummon creatureMenuController;
    private bool canUse = true;

    public bool CanUse { set { canUse = value; } }
    public GameObject CreaturePrefab => creaturePrefab;

    public void Init(WizardsSummon creatureMenuController)
    {
        this.creatureMenuController = creatureMenuController;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && canUse)
        {
            creatureMenuController.ActivateMovingCell();
            creatureMenuController.TakedCell = this;
        }
    }
}