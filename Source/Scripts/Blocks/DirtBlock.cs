using System.Collections;
using UnityEngine;

public class DirtBlock : ActionWizardObject
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D myCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool preActivatedDirt;
    private bool activeState = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (preActivatedDirt)
            ChangeState();
    }
    public void ChangeState()
    {
        activeState = !activeState;

        if (activeState)
        {
            myCollider.enabled = true;
            spriteRenderer.enabled = true;
            StopsMissles = true;
            animator.SetTrigger("GrowDirt");
        }
        else
        {
            animator.SetTrigger("DestroyDirt");

            StartCoroutine(WaitForAnimToOff());
        }
    }

    private IEnumerator WaitForAnimToOff()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        myCollider.enabled = false;
        spriteRenderer.enabled = false;
        StopsMissles = false;
    }
}