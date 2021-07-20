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
    public class PlayerState
    {
        public bool isMoving;
        public bool isGrounded;
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
    [SerializeField] private PlayerState playerState = new PlayerState();
    [SerializeField] private KeyOption keyOption = new KeyOption();

    public Components Com => components;
    public PlayerStatus Stat => playerStatus;
    public PlayerState State => playerState;
    public KeyOption Key => keyOption;

    private Vector2 moveDir;


    #endregion

    private void Awake()
    {
        Stat.Initialize();
    }

    private void Update()
    {
        SetInput();
        GroundCheck();

        Move();
        Jump();
    }

    private void FixedUpdate()
    {
    }

    private void SetInput()
    {
        float movementInput =0;
        if (Input.GetKey(Key.moveLeft)) movementInput = -1f;
        if (Input.GetKey(Key.moveRight)) movementInput = 1f;

        moveDir.x = movementInput;

        State.isMoving = moveDir.x != 0;
    }

    private void Move()
    {
        if(State.isMoving == false)
        {
            Com.rigidbody.velocity = new Vector3(0, Com.rigidbody.velocity.y, 0);
            return;
        }

        float moveVector = moveDir.x * Stat.movementSpeed;
        Com.rigidbody.velocity = new Vector3(moveVector, Com.rigidbody.velocity.y, 0);
    }

    private void GroundCheck()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Ground");

        const float rayDistance = 10f;
        const float threshold = 0.01f;

        bool cast = Physics.SphereCast(transform.position, Com.collider.radius, Vector3.down, out var hit, rayDistance, layerMask);
        float distance = cast ? hit.distance + Com.collider.radius - Com.collider.height / 2 : rayDistance;
        State.isGrounded = distance <= threshold;
    }

    private void Jump()
    {
        if (!State.isGrounded)
            return;

        if(Input.GetKeyDown(Key.jump))
        {
            Com.rigidbody.AddForce(Vector3.up * Stat.jumpForce, ForceMode.VelocityChange);
        }
    }
}
