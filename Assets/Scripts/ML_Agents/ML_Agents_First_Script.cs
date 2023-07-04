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
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float minTimeToReachGoal = 15;
    [SerializeField] private float maxTimeToReachGoal = 30;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private PlayerController playerController;

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
        transform.position = agentStartTransformPosition;
        startDistace = Vector3.Distance(transform.position, target.position);
        StartCoroutine(EpisodeTimer());
        
    }
    
    private IEnumerator EpisodeTimer()
    {
        yield return new WaitForSeconds(maxTimeToReachGoal);
        GiveReward();
    }

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
        //var direction = target.position - transform.position;
        //var normalizedDistance = Vector3.Distance(transform.position, target.position);
        //sensor.AddObservation(direction.normalized);
        //sensor.AddObservation(normalizedDistance);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        int jumpInput = actions.DiscreteActions[0];
      //  playerController.AgentJumpInput = jumpInput;
     //   playerController.AgentMoveInput = moveX;
     
       
       // Debug.Log(moveX);
        //transform.position += new Vector3(moveX, 0) * moveSpeed * Time.deltaTime;  USE
        if (jumpInput == 1 && isGrounded)
        {
            
            //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); USE
            //isGrounded = false;                                       USE
        }
     //   transform.position += new Vector3(moveXint, 0, 0) * moveSpeed * Time.deltaTime; 
        // int jump = actions.DiscreteActions[0];
        //simpleController.MoveX = moveX;
        
       //if (jump == 1)
       // {
       //     rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
       //     isGrounded = false;
       // }
        //transform.Translate(new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime);
  
    }
    
   
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
   
        discreteActions[0] = (Input.GetButtonDown("Jump")) ? 1 : 0;
        //Debug.Log("discrete" + discreteActions[0]);
       // Debug.Log("contentious" + continuousActions[0]);

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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        //Debug.Log("grounded");
        isGrounded = true;
    }
    //public void CalculateAgentWalk()
    //{
    //    if (AgentMoveInput != 0)
    //    {
    //        // Set horizontal move speed
    //        _currentHorizontalSpeed += AgentMoveInput * _acceleration * Time.deltaTime;

    //        // clamped by max frame movement
    //        _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);

    //        // Apply bonus at the apex of a jump
    //        var apexBonus = Mathf.Sign(AgentMoveInput) * _apexBonus * _apexPoint;
    //        _currentHorizontalSpeed += apexBonus * Time.deltaTime;
    //    }
    //    else
    //    {
    //        // No input. Let's slow the character down
    //        _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
    //    }
    //    if (_currentHorizontalSpeed > 0 && _colRight || _currentHorizontalSpeed < 0 && _colLeft)
    //    {
    //        // Don't walk through walls
    //        _currentHorizontalSpeed = 0;
    //    }
    //public float AgentMoveInput;
    //public int AgentJumpInput;
    //[SerializeField] bool useAgentInput = false;
}



