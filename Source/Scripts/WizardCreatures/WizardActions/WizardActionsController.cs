using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WizardActionsController : MonoBehaviour
{
    [SerializeField] private Tilemap slimesTilemap;

    private List<ActionWizardObject> actionObjects = new();

    public static WizardActionsController Instance;
    public Tilemap SlimesTilemap => slimesTilemap;

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