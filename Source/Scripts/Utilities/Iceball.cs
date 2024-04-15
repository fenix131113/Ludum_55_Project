using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Iceball : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;

    private GameObject ignoreGameObject;
    private Transform iceSlime;
    private float destroyDistance;
    private bool dontDestroyByDistance;

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
        if (Vector2.Distance(transform.position, iceSlime.position) >= destroyDistance && !dontDestroyByDistance)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActionWizardObject actionObj = collision.GetComponent<ActionWizardObject>();

        if (collision.gameObject != ignoreGameObject && !actionObj)
            Destroy(gameObject);
        else if (actionObj && actionObj.StopsMissles && !actionObj.GetComponent<WaterWizard>())
            Destroy(gameObject);
        else if (actionObj)
            switch (actionObj.ObjectType)
            {
                case "Water":
                    SetWaterToIce(actionObj);
                    Destroy(gameObject);
                    break;
                case "FireWall":
                    IEnumerator DestroyFire(ActionWizardObject actionObj)
                    {
                        Animator fireAnim = actionObj.GetComponent<Animator>();
                        fireAnim.SetTrigger("Dissolve");
                        yield return new WaitForSeconds(fireAnim.GetCurrentAnimatorClipInfo(0).Length - 0.3f);
                        Destroy(actionObj.gameObject);
                        Destroy(gameObject);
                    }
                    StartCoroutine(DestroyFire(actionObj));

                    dontDestroyByDistance = true;

                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<Collider2D>().enabled = false;
                    GetComponent<Light2D>().enabled = false;
                    break;
            }
    }

    private void SetWaterToIce(ActionWizardObject iceObject)
    {
        Animator iceAnimator = iceObject.GetComponent<Animator>();
        iceObject.gameObject.layer = 3;
        iceObject.SetObjectType("Ice");
        //iceObject.CanPlaceSlime = true;
        iceAnimator.SetTrigger("Froze");
    }
}