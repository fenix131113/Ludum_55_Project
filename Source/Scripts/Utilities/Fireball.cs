using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;

    private GameObject ignoreGameObject;
    private Transform fireSlime;
    private float destroyDistance;

    private int rotateIndex;
    public void Init(int rotateIndex, GameObject ignoreObject, float destroyDistance, Transform fireSlime)
    {
        ignoreGameObject = ignoreObject;
        this.rotateIndex = rotateIndex;
        this.destroyDistance = destroyDistance;
        this.fireSlime = fireSlime;
    }
    private void Update()
    {
        FourRotateOffsetArgs rotate = FourRotateOffsetArgs.fourRotates[rotateIndex];
        rb.velocity = new Vector2(rotate.TargetXOffset, rotate.TargetYOffset) * speed;
        if (Vector2.Distance(transform.position, fireSlime.position) >= destroyDistance)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActionWizardObject actionObj = collision.GetComponent<ActionWizardObject>();
        if (collision.gameObject != ignoreGameObject && !actionObj)
            Destroy(gameObject);
        else if(actionObj && actionObj.StopsMissles)
            Destroy(gameObject);

        else if (actionObj)
            switch (actionObj.ObjectType)
            {
                case "Water":
                case "Ice":
                    Destroy(collision.gameObject);
                    Destroy(gameObject);
                    break;
            }
    }
}
