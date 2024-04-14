using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private string objectInteractionType;
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
        if (collision.gameObject != ignoreGameObject && !collision.GetComponent<ActionWizardObject>())
            Destroy(gameObject);
        else if (collision.GetComponent<ActionWizardObject>() && collision.GetComponent<ActionWizardObject>().ObjectType == objectInteractionType)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
