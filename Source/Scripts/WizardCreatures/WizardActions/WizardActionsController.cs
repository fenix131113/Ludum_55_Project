using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WizardActionsController : MonoBehaviour
{
    [SerializeField] private Tilemap slimesTilemap;
    [SerializeField] private List<DirtBlock> dirtsBlocks;

    private List<ActionWizardObject> actionObjects = new();

    public static WizardActionsController Instance;
    public Tilemap SlimesTilemap => slimesTilemap;
    public IReadOnlyList<DirtBlock> DirtBlocks => dirtsBlocks;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    public void RegisterActionObject(ActionWizardObject actionObject)
    {
        if (!actionObjects.Contains(actionObject))
            actionObjects.Add(actionObject);
    }

    public List<ActionWizardObject> GetAllContactedActionObjects(WizardBase wizard)
    {
        List<ActionWizardObject> actionObjects = new();

        foreach (Vector3Int coordinate in wizard.GetLineTilemapCellsCoordinates())
        {
            ActionWizardObject actionObject = TryGetActionObjectByCell(coordinate);
            if(!actionObject)
                continue;

            WizardBase gettedWizard = GetComponent<WizardBase>();

            if (gettedWizard && gettedWizard.gameObject.GetHashCode() == wizard.gameObject.GetHashCode())
                continue;

            if (actionObject != null && !actionObjects.Contains(actionObject))
                actionObjects.Add(actionObject);
        }

        if (actionObjects.Count > 0)
            return actionObjects;
        else
            return null;
    }

    public ActionWizardObject TryGetActionObjectByCell(Vector3Int coordinates)
    {
        foreach (ActionWizardObject actionObject in actionObjects)
            foreach (Vector3Int actionCoords in actionObject.Coordinates)
                if (actionCoords == coordinates)
                    return actionObject;
        return null;
    }
}