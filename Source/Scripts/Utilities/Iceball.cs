using System.Collections.Generic;
using UnityEngine;

public class Iceball : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;

    private GameObject ignoreGameObject;
    private Transform iceSlime;
    private float destroyDistance;

    private int rotateIndex;
    public void Init(int rotateIndex, GameObject ignoreObject, float destroyDistance, Transform iceSlime)
    {
        ignoreGameObject = ignoreObject;
        this.rotateIndex = rotateIndex;
        this.destroyDistance = destroyDistance;
        this.iceSlime = iceSlime;
    }
    private void Update()
    {
        FourRotateOffsetArgs rotate = FourRotateOffsetArgs.fourRotates[rotateIndex];
        rb.velocity = new Vector2(rotate.TargetXOffset, rotate.TargetYOffset) * speed;
        if (Vector2.Distance(transform.position, iceSlime.position) >= destroyDistance)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActionWizardObject actionObj = collision.GetComponent<ActionWizardObject>();

        if (collision.gameObject != ignoreGameObject && !actionObj)
            Destroy(gameObject);
        else if (actionObj && actionObj.StopsMissles)
            Destroy(gameObject);

        else if (actionObj)
            switch (actionObj.ObjectType)
            {
                case "Water":
                    SetWaterToIce(actionObj);
                    Destroy(gameObject);
                    break;
                case "FireBall":
                    Destroy(collision.gameObject);
                    Destroy(gameObject);
                    break;
            }
    }

    private void SetWaterToIce(ActionWizardObject iceObject)
    {
        iceObject.gameObject.layer = 3;
        iceObject.SetObjectType("Ice");
    }
}