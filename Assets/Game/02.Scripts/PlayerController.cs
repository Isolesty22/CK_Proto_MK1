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
        public CapsuleCollider crouchCollier;
        public GameObject standingModel;
        public GameObject crouchModel;
        public Weapon weapon;
    }

    [Serializable]
    public class PlayerStatus : Status
    {
        public float parryingTime = 1f;
        public float bulletSpeed = 10f;
        public float movementSpeed = 10f;
        public float crouchMoveSpeed = 1f;
        public float jumpingSpeed = 1f;
        public float jumpForce = 1f;
        public float invincibleTime = 1f;
        public float parryingForce = 10f;
    }

    [Serializable]
    public class Value
    {
        [Header("movement value")]
        public Vector3 moveVector;
        public Vector3 moveVelocity;
        public float velocityY;
        public float gravity = 1f;
        [Header("physics value")]
        public Vector3 groundNormal;
        public float groundedDistance = 0f;
        public float groundedCheck = 2f;
        public float forwardCheck = 0.1f;
        [Header("KnockBack")]
        public float knockBackPower;
        public Vector3 knockBackVelocity;
        public float constDecrease = 12f;
    }

    [Serializable]
    public class PlayerState
    {
        public bool isMoving;
        public bool isGrounded;
        public bool isJumping;
        public bool isForwardBlocked;
        public bool isCrouching;
        public bool isHit;
        public bool isInvincible;
        public bool isLookUp;
        public bool canParry;
        public bool isParrying;
        public bool isPoison;
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
        public KeyCode lookUp = KeyCode.UpArrow;
        public KeyCode attack = KeyCode.Z;
        public KeyCode jump = KeyCode.X;
        public KeyCode parry = KeyCode.C;
    }

    [Serializable]
    public class InputState
    {
        public bool attack;
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
    [SerializeField] private InputState inputState = new InputState();

    public Components Com => components;
    public PlayerStatus Stat => playerStatus;
    public Value Val => value;
    public InputValue InputVal => input;
    public PlayerState State => playerState;
    public KeyOption Key => keyOption;
    public InputState InState => inputState;

    private Vector3 capsulePoint1 => new Vector3(transform.position.x, transform.position.y - Com.collider.height / 2 + Com.collider.radius, 0);
    private Vector3 capsulePoint2 => new Vector3(transform.position.x, transform.position.y + Com.collider.height/2 - Com.collider.radius, 0);

    private Vector3 crouchCapsulePoint1 => new Vector3(transform.position.x, Com.crouchCollier.radius, 0);
    private Vector3 crouchCapsulePoint2 => new Vector3(transform.position.x, transform.position.y + Com.crouchCollier.height / 2 - Com.crouchCollier.radius, 0);

    #endregion

    private void Awake()
    {
        Stat.Initialize();
        transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
    }

    private void Start()
    {

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
        Crouch();
        Move();
        Rotate();
        LookUp();
        ReadyToParry();
    }

    private void SetInput()
    {
        InputVal.movementInput = 0f;

        if (Input.GetKey(Key.moveLeft))
        {
            InputVal.movementInput = -1f;
        }
        //else InState.leftPush = false;

        if (Input.GetKey(Key.moveRight))
        {
            InputVal.movementInput = 1f;
        }

        if (Input.GetKeyDown(Key.attack)) Attack();

        if (!State.isGrounded)
            return;
    }

    private void GroundCheck()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Ground");

        const float rayDistance = 10f;
        const float threshold = 0.13f;

        bool cast = Physics.SphereCast(transform.position, Com.collider.radius* 0.9f, Vector3.down, out var hit, rayDistance, layerMask);
        Val.groundedDistance = cast ? hit.distance + Com.collider.radius - Com.collider.height / 2 : rayDistance;
        State.isGrounded = Val.groundedDistance <= threshold;

        Val.groundNormal = hit.normal;
    }

    private void ForwardCheck()
    {
        State.isForwardBlocked = false;

        int layerMask = (-1) - (1 << LayerMask.NameToLayer("Monster"));

        if (Physics.CapsuleCast(capsulePoint1, capsulePoint2, Com.collider.radius * 0.8f, Vector3.right * input.movementInput, Val.forwardCheck, layerMask, QueryTriggerInteraction.Ignore))
        {
            State.isForwardBlocked = true;
        }

        //if (State.isCrouching)
        //{
        //    if (Physics.CapsuleCast(crouchCapsulePoint1, crouchCapsulePoint2, Com.crouchCollier.radius * 0.8f, Vector3.right * input.movementInput, out var hit, Val.forwardCheck, layerMask, QueryTriggerInteraction.Ignore))
        //    {
        //        State.isForwardBlocked = true;
        //    }
        //}
        //else
        //{
        //    if (Physics.CapsuleCast(capsulePoint1, capsulePoint2, Com.collider.radius * 0.8f, Vector3.right * input.movementInput, out var hit, Val.forwardCheck, layerMask, QueryTriggerInteraction.Ignore))
        //    {
        //        State.isForwardBlocked = true;
        //    }
        //}
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
            if(State.isHit)
            {
                Val.moveVelocity = Val.moveVector * Stat.movementSpeed * 0.3f;
            }
            else
                Val.moveVelocity = Val.moveVector * Stat.movementSpeed;
        }

        //점프중에 이동속도, 조건 더 추가해줘야 벽에서 버그 안생김.
        //if (State.isJumping && !State.isGrounded)
        //{
        //    Val.moveVelocity = Val.moveVector * Stat.movementSpeed * Stat.jumpingSpeed;
        //}

        //crouch movement speed;

        //if (State.isCrouching)
        //{
        //    Val.moveVelocity = Val.moveVector * Stat.movementSpeed * Stat.crouchMoveSpeed;
        //}


        //최종 이동속도 결정
        Com.rigidbody.velocity = new Vector3(Val.moveVelocity.x, Val.moveVelocity.y, 0) + Val.knockBackVelocity + (Vector3.up * Val.velocityY);
    }

    private void Rotate()
    {
        if (InputVal.movementInput == -1f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
        }
        else if (InputVal.movementInput == 1f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        }
    }

    private void Jump()
    {

        if (!State.isGrounded)
        {
            return;
        }

        if (Input.GetKey(Key.jump))
        {
            Val.velocityY = Stat.jumpForce;
            State.isJumping = true;
        }
    }

    private void Crouch()
    {
        if (State.isJumping || !Input.GetKey(Key.crouch) && State.isCrouching)
        {
            State.isCrouching = false;

            Com.collider.enabled = true;
            Com.crouchCollier.enabled = false;

            //instance model
            Com.standingModel.SetActive(true);
            Com.crouchModel.SetActive(false);

            return;
        }

        if (Input.GetKey(Key.crouch))
        {
            State.isCrouching = true;

            Com.collider.enabled = false;
            Com.crouchCollier.enabled = true;

            //instance model
            Com.standingModel.SetActive(false);
            Com.crouchModel.SetActive(true);
        }
    }

    public void Hit(Transform monsterTransform)
    {
        State.isHit = true;

        Stat.hp -= 1;

        float knockBackDir = transform.position.x - monsterTransform.position.x;

        if(knockBackDir >= 0)
        {
            knockBackDir = 1f;
        }
        else
        {
            knockBackDir = -1f;
        }


        StartCoroutine(KnockBack(knockBackDir));
        StartCoroutine(Invincible());
    }

    private IEnumerator KnockBack(float knockBackDir)
    {
        Val.knockBackVelocity.x = knockBackDir * Val.knockBackPower;

        if(Val.knockBackVelocity.x >= 0 )
        {
            while (Val.knockBackVelocity.x >= 0 )
            {
                Val.knockBackVelocity.x -= Val.constDecrease * Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }
        }
        else if (Val.knockBackVelocity.x < 0)
        {
            while (Val.knockBackVelocity.x < 0)
            {
                Val.knockBackVelocity.x += Val.constDecrease * Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }
        }

        State.isHit = false;

        Val.knockBackVelocity.x = 0;
    }

    private IEnumerator Invincible()
    {
        State.isInvincible = true;

        yield return new WaitForSeconds(Stat.invincibleTime);

        State.isInvincible = false;
    }

    private void LookUp()
    {
        if (Input.GetKey(Key.lookUp))
        {
            State.isLookUp = true;
            Com.weapon.transform.localPosition = new Vector3(0, 1f, 0);
        }
        else
        {
            State.isLookUp = false;
            Com.weapon.transform.localPosition = new Vector3(0, 0, 0.5f);
        }
    }

    public void ReadyToParry()
    {
        //패링 불가능한 상태
        if (!State.isJumping || State.isGrounded)
        {
            State.canParry = false;
        }

        if (State.canParry)
        {
            return;
        }

        if(Input.GetKey(Key.parry))
        {
            Debug.Log("Parrying");
            StartCoroutine(Parry());
        }

    }

    public IEnumerator Parry()
    {
        State.canParry = true;

        //effect

        yield return new WaitForSeconds(Stat.parryingTime);

        State.canParry = false;
    }

    public IEnumerator Parrying()
    {
        Val.velocityY = Stat.jumpForce;

        State.canParry = false;
        StopCoroutine(Parry());

        //프레임 단위 무적
        //State.isInvincible = true;
        //yield return null;
        //State.isInvincible = false;

        //시간 단위 무적
        State.isInvincible = true;
        yield return new WaitForSeconds(0.1f);
        State.isInvincible = false;
    }

    private void Attack()
    {
        var fireDir = Vector3.forward;

        if(State.isLookUp)
        {
            fireDir = transform.up;
        }
        else
        {
            fireDir = transform.forward;
        }

        StartCoroutine(Com.weapon.Fire(fireDir));
    }
}
