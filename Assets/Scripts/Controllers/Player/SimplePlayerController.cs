using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    public float MoveSpeed = 1f;

    public float JumpForce = 1f;
    [SerializeField] Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
    }

    public void Inputs()
    {
        Move();
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }
    public void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(moveX, moveY, 0) * MoveSpeed * Time.deltaTime);
    }

    public void Jump()
    {
        rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision");
    }
}


