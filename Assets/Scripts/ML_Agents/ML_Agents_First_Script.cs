using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;
using System.Timers;
using TarodevController;
using UnityEngine.PlayerLoop;


public class ML_Agents_First_Script : Agent
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float jumpForce = 1;
    [SerializeField] private float maxReward = 1;
    [SerializeField] private float minTimeToReachGoal = 15;
    [SerializeField] private float maxTimeToReachGoal = 30;
    [SerializeField] private PlayerControllerNew playerControllerNew;
    private float startDistace;
    private Vector3 agentStartTransformPosition;
    private Predicate<bool> PressedJumped;

    private void Awake()
    {
        agentStartTransformPosition = transform.position;
        //PressedJumped = OnJumpedMLAgentPressed;
    }

    public override void Initialize()
    {
        base.Initialize();
    }
    
    public override void OnEpisodeBegin()
    {
        transform.position = agentStartTransformPosition;
        startDistace = Vector3.Distance(transform.position, target.position);
        StartCoroutine(EpisodeTimer());
        
    }
    
    private IEnumerator EpisodeTimer()
    {
        yield return new WaitForSeconds(maxTimeToReachGoal);
        GiveReward();
    }
    //private bool OnJumpedMLAgentPressed(bool jumpInput)
    //{
    ///    return jumpInput;
    //}
    private void GiveReward()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        float reward = 0;
        
        if (distance > 0)
        {
            reward = (distance / minTimeToReachGoal) * maxReward;
            Debug.Log("positive reward is:" + reward);
            SetReward(reward);
            EndEpisode();
        }
        if (transform.localPosition.x < transform.localPosition.x + 2)
        {
            reward = -0.2f;
            SetReward(reward);
        }
        Debug.Log("reward is: " + reward);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((Vector2)transform.localPosition);
        sensor.AddObservation((Vector2)target.localPosition);
    }
 
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        int jumpInput = actions.DiscreteActions[0];

        if (jumpInput == 1)
        {
            //playerControllerNew.OnInputEvent?.Invoke();

            playerControllerNew.AgentJumpDownInput = true;
            //PressedJumped?.Invoke(playerControllerNew.AgentJumpDownInput);
        }
        //  playerControllerNew.AgentJumpDownInput = Convert.ToBoolean(jumpInput);
        playerControllerNew.AgentMoveInput = moveX;
        //playerControllerNew.OnInputEvent?.Invoke();

    }
    
   
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        discreteActions[0] = (Input.GetButtonDown("Jump")) ? 1 : 0;
        playerControllerNew.AgentMoveInput = continuousActions[0];
        playerControllerNew.AgentJumpDownInput = Convert.ToBoolean(discreteActions[0]);
    }


    public void OnTriggerEnter2D(Collider2D coll)
    {
      
        if (coll.CompareTag("Obstacle"))
        {
            SetReward(-0.25f);
            GiveReward();
            Debug.Log("ouch");
        }

        if (coll.CompareTag("Goal"))
        {
            GiveReward();
             Debug.Log("win");
        }
        
    }

}



