using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    private bool canControll = true;

    public void SetControllStatus(bool status) => canControll = status;
    void Update()
    {
        if (!canControll)
            return;

        Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


        if (moveVector.x != 0 || moveVector.y != 0)
            animator.SetTrigger("Walk");
        else if(moveVector.x == 0 && moveVector.y == 0)
            animator.SetTrigger("Idle");


        if (moveVector.x < 0)
            spriteRenderer.flipX = true;
        else if (moveVector.x > 0)
            spriteRenderer.flipX = false;


        GetComponent<Rigidbody2D>().velocity = moveVector * moveSpeed;
    }
}