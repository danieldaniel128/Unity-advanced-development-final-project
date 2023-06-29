using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    public float MoveSpeed = 1f;
    [SerializeField] private bool isAI = false;
    public float JumpForce = 1f;
    [SerializeField] Rigidbody2D rb;
   public float MoveX;
   public float MoveY;

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
        if (!isAI)
        {
           MoveX = Input.GetAxis("Horizontal");
           MoveY = Input.GetAxis("Vertical");
        }
        transform.Translate(new Vector3(MoveX, MoveY, 0) * MoveSpeed * Time.deltaTime);
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


