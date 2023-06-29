using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;


public class ML_Agents_First_Script : Agent
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 1;
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
        float moveY = actions.ContinuousActions[1];

        simpleController.MoveX = moveX;
        simpleController.MoveY = moveY;
        //transform.position += new Vector3(moveX, moveY, 0) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
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
            SetReward(1f);
            EndEpisode();
            Debug.Log("win");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided");
    }


}
