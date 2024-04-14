using UnityEngine;

public class DirtBlock : ActionWizardObject
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D myCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool activeState = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void ChangeState()
    {
        activeState = !activeState;

        if (activeState)
        {
            myCollider.enabled = true;
            animator.enabled = true;
            spriteRenderer.enabled = true;
            StopsMissles = true;
            animator.SetTrigger("GrowDirt");
        }
        else
            animator.SetTrigger("DestroyDirt");
    }
}