using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private float fireballDistanceOffset;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip iceSound;

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
        SoundController.Instance.PlayOneShot(shootSound, 0.5f);
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
                    SoundController.Instance.PlayOneShot(iceSound);
                    DestroyFireball();
                    break;
                case "IceWall":
                    collision.GetComponent<Animator>().SetTrigger("Break");
                    IEnumerator DestroyAfterAnim()
                    {
                        yield return new WaitForSeconds(0.68f);
                        Destroy(actionObj.gameObject);
                        Destroy(gameObject);
                    }
                    StartCoroutine(DestroyAfterAnim());
                    DestroyFireball(false);
                    break;
                case "Water":
                    collision.GetComponent<Animator>().SetTrigger("Evaporate");
                    IEnumerator DestroyAfterAnimWaterEvaporate()
                    {
                        yield return new WaitForSeconds(0.5f);
                        Destroy(actionObj.gameObject);
                        Destroy(gameObject);
                    }
                    StartCoroutine(DestroyAfterAnimWaterEvaporate());
                    DestroyFireball(false);
                    break;
            }
        if (collision.gameObject != ignoreGameObject && !actionObj)
            DestroyFireball();
        else if (actionObj && actionObj.StopsMissles && !actionObj.GetComponent<FireWizard>() && !isStopped)
            DestroyFireball();
    }
    private void DestroyFireball(bool deleteSelf = true)
    {
        StopFireball();
        GetComponent<Collider2D>().enabled = false;
        anim.SetTrigger("Stop");

        IEnumerator WaitStopAnimAndDestroy()
        {
            yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length - 0.7f);
            if (deleteSelf)
            {
                Destroy(gameObject);
            }
            else
            {
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Light2D>().enabled = false;
            }
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
