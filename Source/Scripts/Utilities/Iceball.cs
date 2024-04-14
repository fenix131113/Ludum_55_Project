using UnityEngine;

public class Iceball : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private string objectInteractionType;
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
        if (collision.gameObject != ignoreGameObject && !collision.GetComponent<ActionWizardObject>())
            Destroy(gameObject);
        else if (collision.GetComponent<ActionWizardObject>() && collision.GetComponent<ActionWizardObject>().ObjectType == objectInteractionType)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}