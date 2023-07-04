using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SimplePlayerController : MonoBehaviour
{
    public float MoveSpeed = 1f;
    [SerializeField] private bool isAI = false;
    public float JumpForce = 1f;
    [SerializeField] Rigidbody2D rb;
    public float MoveX;
    public float MoveY;



    // Update is called once per frame
    void Update()
    {
       
       
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
        if (isAI == false)
        {
            MoveX = Input.GetAxis("Horizontal");
        }
       // transform.Translate(new Vector3(MoveX, 0, 0) * MoveSpeed * Time.deltaTime);
    }

    public void Jump()
    {
        rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

}


