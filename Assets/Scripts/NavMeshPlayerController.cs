using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class NavMeshPlayerController : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private LayerMask moveableLayers;

    [Header("Player settings")]
    [Tooltip("Overwrites NavMeshAgent.speed")] [SerializeField] private float movementSpeed = 3;
    [Tooltip("Overwrites NavMeshAgent.acceleration")] [SerializeField] private float acceleration = 8;
    [Tooltip("Overwrites NavMeshAgent.angularSpeed")] [SerializeField] private float angularSpeed = 120;
    [Tooltip("Overwrites NavMeshAgent.stoppingDistance")] [SerializeField] private float stoppingDistance = 0;

    [Header("Animation settings")]
    [Tooltip("Smooth time between animations")] [SerializeField] private float animationSmoothTime = 0.1f;

    // Required components
    private NavMeshAgent playerNavMeshAgent;
    private Animator playerAnimator;

    private Vector2 mousePositionToMove;
    
    
    private bool moveIsPressed;

    public float MovementSpeed
    {
        set { movementSpeed = value; playerNavMeshAgent.speed = value; }
        get { return movementSpeed; }
    }

    public float Acceleration
    {
        set { acceleration = value; playerNavMeshAgent.acceleration = value; }
        get { return acceleration; }
    }

    public float StoppingDistance
    {
        set { stoppingDistance = value; playerNavMeshAgent.stoppingDistance = value; }
        get { return stoppingDistance; }
    }

    public float AngularSpeed
    {
        set { angularSpeed = value; playerNavMeshAgent.angularSpeed = value; }
        get { return angularSpeed; }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerNavMeshAgent = GetComponent<NavMeshAgent>();
        playerNavMeshAgent.updateRotation = false;
        playerAnimator = GetComponent<Animator>();
        MovementSpeed = movementSpeed;
        Acceleration = acceleration;
        AngularSpeed = angularSpeed;
        StoppingDistance = stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move(InputAction.CallbackContext value)
    {
        if (value.started)
            moveIsPressed = true;
        if (value.canceled)
            moveIsPressed = false;
    }

    private void Move()
    {
        if (moveIsPressed)
        {
            mousePositionToMove = Mouse.current.position.ReadValue();
            float distanceBetweenPlayerAndDestination = 0;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePositionToMove), out RaycastHit hit, Mathf.Infinity, moveableLayers))
            {
                distanceBetweenPlayerAndDestination = Vector3.Distance(transform.position, hit.point);

                if (NavMesh.SamplePosition(hit.point, out NavMeshHit nmh, playerNavMeshAgent.radius, NavMesh.AllAreas))
                {
                    playerNavMeshAgent.SetDestination(nmh.position);
                }

                if (playerNavMeshAgent.velocity.magnitude < Mathf.Epsilon)
                {
                    transform.LookAt(hit.point);
                }
            }
        }

        if (playerNavMeshAgent.velocity.magnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(playerNavMeshAgent.velocity.normalized);
        }

        float speedPercent = playerNavMeshAgent.velocity.magnitude / playerNavMeshAgent.speed;
        playerAnimator.SetFloat("speedPercent", speedPercent, animationSmoothTime, Time.deltaTime);

    }
}
