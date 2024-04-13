using UnityEngine;
using UnityEngine.EventSystems;

public class CreatureCell : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject creaturePrefab;

    private CreaturesSummon creatureMenuController;
    private bool canUse = true;

    public bool CanUse { set { canUse = value; } }
    public GameObject CreaturePrefab => creaturePrefab;

    public void Init(CreaturesSummon creatureMenuController)
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