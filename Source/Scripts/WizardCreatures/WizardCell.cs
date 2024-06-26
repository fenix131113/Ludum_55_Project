using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WizardCell : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject creaturePrefab;
    [SerializeField] private Image existIndicator;

    private WizardsSummon creatureMenuController;
    private bool canUse = true;

    public bool CanUse { set { canUse = value; } }
    public GameObject CreaturePrefab => creaturePrefab;
    public Image ExistIndicator => existIndicator;

    public void Init(WizardsSummon creatureMenuController)
    {
        this.creatureMenuController = creatureMenuController;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && canUse)
        {
            creatureMenuController.TakedCell = this;
            creatureMenuController.ActivateMovingCell();
        }
    }
}