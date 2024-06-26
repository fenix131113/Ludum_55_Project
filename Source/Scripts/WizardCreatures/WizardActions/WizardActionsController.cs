using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WizardActionsController : MonoBehaviour
{
    [SerializeField] private Tilemap slimesTilemap;
    [SerializeField] private List<DirtBlock> dirtBlocks;
    [SerializeField] private List<GrassBlock> grassBlocks;

    private List<ActionWizardObject> actionObjects = new();

    public static WizardActionsController Instance;
    public Tilemap SlimesTilemap => slimesTilemap;
    public IReadOnlyList<GrassBlock> GrassBlocks => grassBlocks;
    public IReadOnlyList<DirtBlock> DirtBlocks => dirtBlocks;

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
            if (!actionObject)
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

    public void ShakeCamera(float duration)
    {
        Vector3 startCameraPos = Camera.main.transform.position;
        Camera.main.DOShakePosition(duration, 0.08f, 15, fadeOut: false).onComplete += () => Camera.main.transform.position = startCameraPos;
    }
}