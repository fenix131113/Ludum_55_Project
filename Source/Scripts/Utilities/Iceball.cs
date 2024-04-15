using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Iceball : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private float iceballDistanceOffset;

    private GameObject ignoreGameObject;
    private Transform iceSlime;
    private float destroyDistance;
    private bool dontDestroyByDistance;
    private bool isStopped;
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

        if (!isStopped)
            rb.velocity = new Vector2(rotate.TargetXOffset, rotate.TargetYOffset) * speed;

        if (Vector2.Distance(transform.position, iceSlime.position) + iceballDistanceOffset >= destroyDistance && !dontDestroyByDistance)
            DestroyIceball();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActionWizardObject actionObj = collision.GetComponent<ActionWizardObject>();

        if (collision.gameObject != ignoreGameObject && !actionObj)
            DestroyIceball();
        else if (actionObj && actionObj.StopsMissles && !actionObj.GetComponent<WaterWizard>())
            DestroyIceball();
        else if (actionObj)
            switch (actionObj.ObjectType)
            {
                case "Water":
                    SetWaterToIce(actionObj);
                    DestroyIceball();
                    break;
                case "FireWall":
                    IEnumerator DestroyFire(ActionWizardObject actionObj)
                    {
                        Animator fireAnim = actionObj.GetComponent<Animator>();
                        fireAnim.SetTrigger("Dissolve");
                        yield return new WaitForSeconds(fireAnim.GetCurrentAnimatorClipInfo(0).Length - 0.3f);
                        Destroy(actionObj.gameObject);
                        DestroyIceball();
                    }
                    StartCoroutine(DestroyFire(actionObj));

                    dontDestroyByDistance = true;

                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<Collider2D>().enabled = false;
                    GetComponent<Light2D>().enabled = false;
                    break;
            }
    }
    private void DestroyIceball()
    {
        StopIceball();
        GetComponent<Collider2D>().enabled = false;
        anim.SetTrigger("Stop");

        IEnumerator WaitStopAnimAndDestroy()
        {
            yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length - 0.7f);
            Destroy(gameObject);
        }
        StartCoroutine(WaitStopAnimAndDestroy());
    }

    private void StopIceball()
    {
        isStopped = true;
        rb.velocity = Vector3.zero;
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