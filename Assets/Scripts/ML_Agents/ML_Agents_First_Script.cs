using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;
using System.Timers;
using UnityEngine.PlayerLoop;


public class ML_Agents_First_Script : Agent
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float jumpForce = 1;
    [SerializeField] private float maxReward = 1;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int jumpingInput;
    [SerializeField] private float moveInput;

    [SerializeField] private float minTimeToReachGoal = 15;
    [SerializeField] private float maxTimeToReachGoal = 30;
    [SerializeField] private bool isGrounded = true;

    
    private float distance;
    private float startDistace;
    private Vector3 agentStartTransformPosition;

    [SerializeField] private SimplePlayerController simpleController;

    private void Awake()
    {
        agentStartTransformPosition = transform.position;
    }


    public override void Initialize()
    {
        base.Initialize();
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("start");
        transform.position = agentStartTransformPosition;
        startDistace = Vector3.Distance(transform.localPosition, target.localPosition);
        StartCoroutine(EpisodeTimer());


    }


    private IEnumerator EpisodeTimer()
    {
        yield return new WaitForSeconds(maxTimeToReachGoal);
        GiveReward();
    }

    private void GiveReward()
    {
        float distance = Vector3.Distance(transform.localPosition, target.localPosition);
        float reward = (distance / minTimeToReachGoal) * maxReward;
        // Debug.Log("distance is: " + distance);


        //if (distance > startDistace / 2)
        //{
        //    reward *= -3;
        //}
        if ( distance < startDistace / 2)
        {

            reward *= 2;
        }
        if (distance < startDistace / 4 )
        {

            reward *= 4;
        }

        SetReward(reward);

        if (distance > startDistace)
        {
            reward *= -7;
            SetReward(reward);
        }

 
        if (distance <= 0.3f)
        {
            reward *= 20f;
            SetReward(reward);
        }

        Debug.Log("reward is: " + reward);
        EndEpisode();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((Vector3)transform.localPosition);
        sensor.AddObservation((Vector3)target.localPosition);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {

        float moveX = actions.ContinuousActions[0];
        int jumpInput = actions.DiscreteActions[0];
        jumpingInput = jumpInput;
        moveInput = moveX;
        transform.position += new Vector3(moveX, 0) * moveSpeed * Time.deltaTime;


        if (transform.position.x < agentStartTransformPosition.x + 1 && transform.position.x
            > agentStartTransformPosition.x - 1)
        {
            SetReward(-1);
        }


        if (jumpInput == 1 && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        discreteActions[0] = (Input.GetButtonDown("Jump")) ? 1 : 0;

    }


    public void OnTriggerEnter2D(Collider2D coll)
    {
        float distance = Vector3.Distance(transform.localPosition, target.localPosition);
        float reward = (distance / minTimeToReachGoal) * maxReward;
        if (coll.CompareTag("Obstacle"))
        {
            SetReward(reward *= -2f);
            GiveReward();
            Debug.Log("ouch" + coll.name);
        }

        if (coll.CompareTag("Goal"))
        {
           
            GiveReward();
            Debug.Log("win ");
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }



}
