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
    [SerializeField] private int maxPlatformSteps = 3;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int jumpingInput; //for debug
    [SerializeField] private float moveInput; //for debug
    [SerializeField] private float samePlatformStepPenalty = -10;
    [SerializeField] private float wrongDirectionPenaltyMultiplier = -10;
    [SerializeField] private float obstaclePenalty = -10;
    [SerializeField] private float goalReward = 60;

    [SerializeField] private float minTimeToReachGoal = 15;
    [SerializeField] private float maxTimeToReachGoal = 30;
    [SerializeField] private bool isGrounded = true;

    private float distance;
    private float lastDistance;
    private float startDistace;
    private Vector3 agentStartTransformPosition;
    private GameObject lastColildedPlatform;
    private int samePlatformSteps;
    private float reward;

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
        reward = (distance / minTimeToReachGoal) * maxReward;

        SetReward(reward);

        if (distance > startDistace)
        {
            reward *= wrongDirectionPenaltyMultiplier;
            SetReward(reward);
        }

        if (distance <= 0.1f)
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

        FrameRewards();
        if (transform.position.x < agentStartTransformPosition.x + 1 && transform.position.x
            > agentStartTransformPosition.x - 1)
        {
            SetReward(10);
        }

        if (jumpInput == 1 && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }
    private void FrameRewards()
    {
        if (distance > lastDistance) SetReward(wrongDirectionPenaltyMultiplier);
        LiveDistanceChecker();
    }

    private void LiveDistanceChecker()
    {
        lastDistance = distance;
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
            SetReward(reward *= obstaclePenalty);
            GiveReward();
            // Debug.Log("ouch" + coll.name);
        }

        if (coll.CompareTag("Goal"))
        {
            SetReward(goalReward);
            GiveReward();
            Debug.Log("win ");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (lastColildedPlatform != null && collision.gameObject == lastColildedPlatform)
        {

            samePlatformSteps++;
            Debug.Log("stepped on same platform: " + samePlatformSteps + " times");
            if (samePlatformSteps > maxPlatformSteps)
            {
                SetReward(samePlatformStepPenalty);
                Debug.Log("Stepped on Platform more then max steps penalty is: " + samePlatformStepPenalty);
            }
        }
        else
        {
            lastColildedPlatform = collision.gameObject;
            samePlatformSteps = 0;
        }
        isGrounded = true;
    }

}
