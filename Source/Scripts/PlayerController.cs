using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    void Update()
    {
        Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveVector.x < 0)
            spriteRenderer.flipX = true;
        else if (moveVector.x > 0)
            spriteRenderer.flipX = false;

        GetComponent<Rigidbody2D>().velocity = moveVector * moveSpeed;
    }
}