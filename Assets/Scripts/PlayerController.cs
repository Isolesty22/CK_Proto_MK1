using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    //definitions
    #region
    [Serializable]
    public class Components
    {
        public Rigidbody rigidbody;
        public CapsuleCollider collider;
    }

    [Serializable]
    public class PlayerStatus : Status
    {
        public float movementSpeed = 10f;
        public float jumpForce = 1f;
    }

    [Serializable]
    public class Value
    {
        public Vector3 moveVector;
        public Vector3 moveVelocity;
        public float velocityY;
        public float gravity = 1f;
        public Vector3 groundNormal;
        public float groundedDistance = 0f;
        public float groundedCheck = 2f;
        public float forwardCheck = 0.1f;
    }

    [Serializable]
    public class PlayerState
    {
        public bool isMoving;
        public bool isGrounded;
        public bool isJumping;
        public bool isForwardBlocked;
    }

    [Serializable]
    public class InputValue
    {
        public float movementInput = 0;
    }

    [Serializable]
    public class KeyOption
    {
        //default
        public KeyCode moveRight = KeyCode.RightArrow;
        public KeyCode moveLeft = KeyCode.LeftArrow;
        public KeyCode crouch = KeyCode.DownArrow;
        public KeyCode attack = KeyCode.X;
        public KeyCode jump = KeyCode.C;
    }
    #endregion

    //field
    #region
    [SerializeField] private Components components = new Components();
    [SerializeField] private PlayerStatus playerStatus = new PlayerStatus();
    [SerializeField] private Value value = new Value();
    [SerializeField] private InputValue input = new InputValue();
    [SerializeField] private PlayerState playerState = new PlayerState();
    [SerializeField] private KeyOption keyOption = new KeyOption();

    public Components Com => components;
    public PlayerStatus Stat => playerStatus;
    public Value Val => value;
    public InputValue InputVal => input;
    public PlayerState State => playerState;
    public KeyOption Key => keyOption;

    private Vector3 capsulePoint1 => new Vector3(transform.position.x, Com.collider.radius, 0);
    private Vector3 capsulePoint2 => new Vector3(transform.position.x, transform.position.y + Com.collider.height/2 - Com.collider.radius, 0);


    #endregion

    private void Awake()
    {
        Stat.Initialize();
    }

    private void Update()
    {
        SetInput();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        ForwardCheck();
        UpdatePhysics();

        Jump();
        Move();
    }

    private void SetInput()
    {
        InputVal.movementInput = 0f;
        if (Input.GetKey(Key.moveLeft)) InputVal.movementInput = -1f;
        if (Input.GetKey(Key.moveRight)) InputVal.movementInput = 1f;
    }

    private void Move()
    {
        Val.moveVector = new Vector3(InputVal.movementInput, 0, 0);
        State.isMoving = Val.moveVector.x != 0;

        //slope vector
        if(State.isGrounded || Val.groundedDistance < Val.groundedCheck && !State.isJumping)
        {
            Vector3 projection = Vector3.ProjectOnPlane(Val.moveVector, Val.groundNormal);
            Val.moveVector = new Vector3(projection.x, projection.y, 0).normalized;
        }

        //forward block
        if (State.isForwardBlocked && !State.isGrounded || State.isJumping && State.isGrounded)
        {
            Val.moveVelocity = Vector3.zero;
        }
        else
        {
            Val.moveVelocity = Val.moveVector * Stat.movementSpeed;
        }

        Com.rigidbody.velocity = new Vector3(Val.moveVelocity.x, Val.moveVelocity.y, 0) + (Vector3.up * Val.velocityY);
    }

    private void GroundCheck()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Ground");

        const float rayDistance = 10f;
        const float threshold = 0.01f;

        bool cast = Physics.SphereCast(transform.position, Com.collider.radius, Vector3.down, out var hit, rayDistance, layerMask);
        Val.groundedDistance = cast ? hit.distance + Com.collider.radius - Com.collider.height / 2 : rayDistance;
        State.isGrounded = Val.groundedDistance <= threshold;

        Val.groundNormal = hit.normal;
    }

    private void Jump()
    {
        if (!State.isGrounded)
            return;

        if(Input.GetKey(Key.jump))
        {
            Val.velocityY = Stat.jumpForce;
            State.isJumping = true;
        }
    }

    private void UpdatePhysics()
    {
        //gravity
        if(State.isGrounded)
        {
            Val.velocityY = 0f;
            State.isJumping = false;
        }
        else
        {
            Val.velocityY -= Val.gravity * Time.fixedDeltaTime;
        }
    }

    private void ForwardCheck()
    {
        State.isForwardBlocked = false;

        if (Physics.CapsuleCast(capsulePoint1, capsulePoint2, Com.collider.radius * 0.8f, Vector3.right * input.movementInput, Val.forwardCheck, -1, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("blocked");
            State.isForwardBlocked = true;
        }
    }
}
