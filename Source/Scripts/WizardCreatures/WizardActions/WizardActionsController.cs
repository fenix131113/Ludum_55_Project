using System.Collections.Generic;
using UnityEngine;

public class WizardActionsController : MonoBehaviour
{
    public static WizardActionsController Instance;

    private List<ActionWizardObject> actionObjects = new();


    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    private void RegisterActionObject(ActionWizardObject actionObject)
    {
        if (!actionObjects.Contains(actionObject))
            actionObjects.Add(actionObject);
    }

    private ActionWizardObject[] GetAllContactedActionObjects(WizardBase wizard)
    {
        throw new System.Exception();
    }
}
