using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private float fireballDistanceOffset;

    private GameObject ignoreGameObject;
    private Transform fireSlime;
    private float destroyDistance;
    private bool isStopped;
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

        if (!isStopped)
            rb.velocity = new Vector2(rotate.TargetXOffset, rotate.TargetYOffset) * speed;

        if (Vector2.Distance(transform.position, fireSlime.position) + fireballDistanceOffset >= destroyDistance)
            DestroyFireball();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActionWizardObject actionObj = collision.GetComponent<ActionWizardObject>();


        if (actionObj)
            switch (actionObj.ObjectType)
            {
                case "Ice":
                    UnFrozeIce(actionObj);
                    DestroyFireball();
                    break;
                case "IceWall":
                    Destroy(actionObj.gameObject);
                    DestroyFireball();
                    break;
                case "Water":
                    Destroy(actionObj.gameObject);
                    DestroyFireball();
                    break;
            }
        if (collision.gameObject != ignoreGameObject && !actionObj)
            DestroyFireball();
        else if (actionObj && actionObj.StopsMissles && !actionObj.GetComponent<FireWizard>())
            DestroyFireball();
    }
    private void DestroyFireball()
    {
        StopFireball();
        GetComponent<Collider2D>().enabled = false;
        anim.SetTrigger("Stop");

        IEnumerator WaitStopAnimAndDestroy()
        {
            yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length - 0.7f);
            Destroy(gameObject);
        }
        StartCoroutine(WaitStopAnimAndDestroy());
    }

    private void StopFireball()
    {
        isStopped = true;
        rb.velocity = Vector3.zero;
    }

    private void UnFrozeIce(ActionWizardObject iceObject)
    {
        Animator iceAnimator = iceObject.GetComponent<Animator>();
        iceObject.gameObject.layer = 1;
        iceObject.SetObjectType("Water");
        //iceObject.CanPlaceSlime = true;
        iceAnimator.SetTrigger("UnFroze");
    }
}
