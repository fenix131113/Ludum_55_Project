using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTet : MonoBehaviour
{
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * 5f;
    }
}