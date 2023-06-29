using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;


public class ML_Agents_First_Script : Agent
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float jumpForce = 1;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool isGrounded = true;

    private Vector3 agentStartTransformPosition;

    [SerializeField] private SimplePlayerController simpleController;
    
    private void Awake()
    {
        agentStartTransformPosition = transform.position;
    }
    public override void OnEpisodeBegin()
    {
        transform.position = agentStartTransformPosition;
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(target.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
       // int jump = actions.DiscreteActions[0];
        //simpleController.MoveX = moveX;
        
       //if (jump == 1)
       // {
       //     rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
       //     isGrounded = false;
       // }
        //transform.Translate(new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime);
        transform.position += new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime;
        

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        //ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        //discreteActions[0] = Convert.ToInt32(Input.GetButtonDown("Jump"));

    }

  
    public void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("collision");
        if (coll.CompareTag("Obstacle"))
        {
            SetReward(-1f);
            EndEpisode();
            Debug.Log("ouch");
        }

        if (coll.CompareTag("Goal"))
        {
            if (Time.time is > 15 and < 30)
            {
             SetReward(1f);
            }
            else
            { 
                SetReward(-0.5f);
            }
            EndEpisode();
            Debug.Log("win");
            
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        Debug.Log("grounded");
        isGrounded = true;
    }
  


}
