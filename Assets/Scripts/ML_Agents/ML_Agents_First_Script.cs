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
    #region Movement Variables
    [Header("Movement Elements")]
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float jumpForce = 1;
    [SerializeField] private float maxReward = 1;
    [SerializeField] private int maxPlatformSteps = 3;
    [SerializeField] private Rigidbody2D rb;
    #endregion

    #region Debug Elements
    [Header("Debug Elements")]
    [SerializeField] private int jumpingInput; //for debug
    [SerializeField] private float moveInput; //for debug
    #endregion

    #region Penalties & Rewards Variables
    [Header("Penalties & Rewards")]
    [SerializeField] private float samePlatformStepPenalty = -10;
    [SerializeField] private float wrongDirectionPenaltyMultiplier = -10;
    [SerializeField] private float obstaclePenalty = -10;
    [SerializeField] private float goalReward = 60;
    private float reward;
    #endregion

    #region Objectives
    [Header("Objectives")]
    [SerializeField] private Transform target;
    private Vector3 agentStartTransformPosition;
    #endregion

    #region Timer Variables
    [Header("TimerElements")]
    [SerializeField] private float minTimeToReachGoal = 15;
    [SerializeField] private float maxTimeToReachGoal = 30;
    [SerializeField] private bool isGrounded = true;
    #endregion

    #region Penalty Checking Variables
    [Header("Penalty Checking Variables")]
    private float distance;
    private float lastDistance;
    private float startDistace;
    private GameObject lastColildedPlatform;
    private int samePlatformSteps;
    #endregion

    private void Awake()
    {
        agentStartTransformPosition = transform.position;
    }

 



    private IEnumerator EpisodeTimer()
    {
        yield return new WaitForSeconds(maxTimeToReachGoal);
        GiveReward();
    }

    #region RewardSystem
    private void GiveReward()
    {
        float actualReward = RewardBasedOnDistance();
        Debug.Log("reward is: " + actualReward);
        EndEpisode();
    }

    private float RewardBasedOnDistance()
    {
        float distance = Vector3.Distance(transform.localPosition, target.localPosition);
        reward = (distance / minTimeToReachGoal) * maxReward;
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
        return reward;
    }

    #endregion

    #region MainML_AgentEvents
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

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((Vector3)transform.localPosition);
        sensor.AddObservation((Vector3)target.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions) // like update but an event that happen when AI use input
    {
        float moveX = actions.ContinuousActions[0];
        int jumpInput = actions.DiscreteActions[0];
        jumpingInput = jumpInput;
        moveInput = moveX;
        Move(moveX, jumpInput);
        ActionFrameEnabler();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        discreteActions[0] = (Input.GetButtonDown("Jump")) ? 1 : 0;
    }
    private void Move(float moveX, int jumpInput)
    {
        transform.position += new Vector3(moveX, 0) * moveSpeed * Time.deltaTime;

        if (jumpInput != 1 || !isGrounded) return;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }
    #endregion

    #region FrameHandlers
    private void ActionFrameEnabler()
    {
        FrameRewardsAndPenalties();
    }

    private void FrameRewardsAndPenalties()
    {
        FramePenalties();
        FrameRewards();
    }
    private void FrameRewards()
    {
        if (transform.position.x < agentStartTransformPosition.x + 1 && transform.position.x
            > agentStartTransformPosition.x - 1)
        {
            SetReward(10);
        }
    }
    private void FramePenalties()
    {
        if (distance > lastDistance) SetReward(wrongDirectionPenaltyMultiplier);
        LiveDistanceChecker();
    }
    private void LiveDistanceChecker()
    {
        lastDistance = distance;
    }

    #endregion



    #region TriggerAndCollision

    
    public void OnTriggerEnter2D(Collider2D coll)
    {
        TriggerRewards(coll);
    }
    private void TriggerRewards(Collider2D coll)
    {
        float distance = Vector3.Distance(transform.localPosition, target.localPosition);
        float reward = (distance / minTimeToReachGoal) * maxReward;
        if (coll.CompareTag("Obstacle"))
        {
            SetReward(reward *= obstaclePenalty);
            GiveReward();
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
        CollidedPlatformStepChecker(collision);
        isGrounded = true;
    }
    private void CollidedPlatformStepChecker(Collision2D collision)
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
    }

    #endregion
}
