using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

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

        public PlayerHitBox hitBox;
        public Weapon weapon;
        public Pixy pixy;

        public ParticleSystem parry;
        public ParticleSystem hit;

        public Animator animator;

        public Material mat1;
        public Material mat2;
        public Material mat3;

        public Color originalColor;
        public Color hitColor;
    }

    [Serializable]
    public class PlayerStatus : Status
    {
        public float movementSpeed = 10f;
        public float crouchMoveSpeed = 5f;
        public float jumpForce = 1f;

        public float parryingForce = 10f;
        public float parryingTime = 1f;
        public float parryInvincibleTime = 0.5f;

        public float hitTime = 1f;
        public float invincibleTime = 1f;

        public float hitColorTime = 1f;
        public float hitColorDelay = 1f;

        public float pixyEnerge = 0f;
        public float parryingEnerge = 1f;
        public float attackEnerge = 0.1f;

        [Header("unused")]
        public float jumpingSpeed = 1f;
    }

    [Serializable]
    public class Value
    {
        [Header("movement value")]
        public Vector3 moveVector;
        public Vector3 moveVelocity;
        [Header("physics value")]
        public float velocityY;
        public float gravity = 1f;
        public Vector3 groundNormal;
        public float groundedDistance = 0f;
        public float upDistance = 0f;
        public float groundedCheck = 2f;
        public float forwardCheck = 0.1f;
        public bool upTrigger = true;
        public bool prevJump;
        [Header("KnockBack")]
        public float knockBackPower;
        public Vector3 knockBackVelocity;
        public float constDecrease = 12f;
        [Header("FirePos")]
        public Vector3 FirePos;
        public Vector3 upFirePos;
        public Vector3 crouchFirePos;
        public Vector3 crouchUpFirePos;
    }

    [Serializable]
    public class PlayerState
    {
        public bool isMoving;
        public bool isGrounded;
        public bool prevGrounded;
        public bool isJumping;
        public bool isForwardBlocked;
        public bool isUpBlocked;
        public bool isCrouching;
        public bool isHit;
        public bool isInvincible;
        public bool isLookUp;
        public bool canParry;
        public bool isParrying;
        public bool isPoison;
        public bool canCounter;
        public bool isAttack;
    }

    [Serializable]
    public class InputValue
    {
        public float movementInput = 0;
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

    private Vector3 capsulePoint1 => new Vector3(transform.position.x, transform.position.y - Com.collider.height / 2 + Com.collider.radius, 0);
    private Vector3 capsulePoint2 => new Vector3(transform.position.x, transform.position.y + Com.collider.height / 2 - Com.collider.radius, 0);

    private Vector3 crouchCapsulePoint1 => new Vector3(transform.position.x, Com.crouchCollier.radius, 0);
    private Vector3 crouchCapsulePoint2 => new Vector3(transform.position.x, transform.position.y + Com.crouchCollier.height / 2 - Com.crouchCollier.radius, 0);

    private IEnumerator parry;
    #endregion

    private void Awake()
    {
        Stat.Initialize();
        transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        parry = Parry();
        Com.mat1.color = Com.originalColor;
        Com.mat2.color = Com.originalColor;
        Com.mat3.color = Com.originalColor;

        Stat.pixyEnerge = 0f;
    }

    private void Start()
    {

    }

    private void Update()
    {
        SetInput();

        FirePos();
        Attack();
        LookUp();
        Crouch();
        Rotate();
        ReadyToParry();

        Counter();

        HandleAnimation();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        ForwardCheck();
        UpdatePhysics();
        UpCheck();


        Jump();
        Move();
    }

    private void SetInput()
    {
        InputVal.movementInput = 0f;

        if (State.isHit)
            return;

        if (Input.GetKey(Key.moveLeft))
        {
            InputVal.movementInput = -1f;
        }

        if (Input.GetKey(Key.moveRight))
        {
            InputVal.movementInput = 1f;
        }
    }

    private void GroundCheck()
    {
        State.prevGrounded = State.isGrounded;

        int layerMask = 1 << LayerMask.NameToLayer("Ground");

        const float rayDistance = 10f;
        const float threshold = 0.13f;

        bool cast = Physics.SphereCast(transform.position, Com.collider.radius * 0.9f, Vector3.down, out var hit, rayDistance, layerMask);
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

    private void UpCheck()
    {
        if (State.isGrounded)
        {
            Val.upTrigger = true;
        }

        State.isUpBlocked = false;

        int layerMask = 1 << LayerMask.NameToLayer("Ground");

        const float rayDistance = 10f;
        const float threshold = 0.13f;

        if(!State.isCrouching)
        {
            bool cast = Physics.SphereCast(transform.position, Com.collider.radius * 0.9f, Vector3.up, out var hit, rayDistance, layerMask);
            Val.upDistance = cast ? hit.distance + Com.collider.radius - Com.collider.height / 2 : rayDistance;
            State.isUpBlocked = Val.upDistance <= threshold;
        }
        else
        {
            bool cast = Physics.SphereCast(transform.position +Vector3.down * 0.25f, Com.collider.radius * 0.9f, Vector3.up, out var hit, rayDistance, layerMask);
            Val.upDistance = cast ? hit.distance + Com.collider.radius - Com.collider.height / 2 : rayDistance;
            State.isUpBlocked = Val.upDistance <= threshold;
        }

        if (State.isUpBlocked && Val.upTrigger)
        {
            Val.velocityY = 0f;
            Val.upTrigger = false;
        }

    }

    private void UpdatePhysics()
    {
        //gravity
        if (State.isGrounded && State.prevGrounded)
        {
            if(!State.isParrying)
            {
                State.isJumping = false;
                Val.velocityY = 0f;
            }
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
        if (State.isGrounded || Val.groundedDistance < Val.groundedCheck && !State.isJumping)
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
            if (State.isCrouching)
            {
                Val.moveVelocity = Val.moveVector * Stat.crouchMoveSpeed;
            }
            else
                Val.moveVelocity = Val.moveVector * Stat.movementSpeed;
        }

        //최종 이동속도 결정
        //Com.rigidbody.velocity = Vector3.ClampMagnitude(new Vector3(Val.moveVelocity.x, Val.moveVelocity.y, 0) + Val.knockBackVelocity, 5) + (Vector3.up * Val.velocityY);
      
        Com.rigidbody.velocity = new Vector3(Val.moveVelocity.x, Val.moveVelocity.y, 0)+ Val.knockBackVelocity + (Vector3.up * Val.velocityY);
    }

    private void Rotate()
    {
        if (State.isHit)
            return;
        //if (State.isJumping)
        //    return;

        if (InputVal.movementInput == -1f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (InputVal.movementInput == 1f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Jump()
    {
        if (!State.isGrounded || State.isHit)
        {
            return;
        }

        if (!Val.prevJump && Input.GetKey(Key.jump))
        {
            Val.velocityY = Stat.jumpForce;
            State.isJumping = true;
        }

        Val.prevJump = Input.GetKey(Key.jump);
    }

    private void Crouch()
    {
        if (State.isHit)
            return;

        if (State.isJumping || !Input.GetKey(Key.crouch) && State.isCrouching )
        {
            if (State.isUpBlocked)
                return;

            State.isCrouching = false;

            Com.collider.enabled = true;
            Com.crouchCollier.enabled = false;

            //hit box
            Com.hitBox.hitBox.enabled = true;
            Com.hitBox.crouchHitBox.enabled = false;

            Com.pixy.transform.localPosition = Com.pixy.firePos;

            return;
        }

        if (Input.GetKey(Key.crouch))
        {
            State.isCrouching = true;

            Com.collider.enabled = false;
            Com.crouchCollier.enabled = true;

            //hit box
            Com.hitBox.hitBox.enabled = false;
            Com.hitBox.crouchHitBox.enabled = true;

            Com.pixy.transform.localPosition = Com.pixy.crouchFirePos;
        }
    }

    public void Hit()
    {
        State.isHit = true;
        State.isAttack = false;
        State.isCrouching = false;
        State.isLookUp = false;


        Stat.hp -= 1;

        Com.animator.SetTrigger("Hit");

        Com.hit.transform.position = transform.position;
        Com.hit.Play();


        var knockBack = KnockBack();
        StartCoroutine(knockBack);
        var invincible = Invincible();
        StartCoroutine(invincible);
    }

    private IEnumerator KnockBack()
    {
        if (transform.localScale.x == -1)
        {
            Val.knockBackVelocity = new Vector3(Val.knockBackPower, 0, 0);
        }
        else if (transform.localScale.x == 1)
        {
            Val.knockBackVelocity = new Vector3(-Val.knockBackPower, 0, 0);
        }

        yield return new WaitForSeconds(Stat.hitTime);

        State.isHit = false;

        Val.knockBackVelocity = Vector3.zero;

        var hitColor = HitColor();
        StartCoroutine(hitColor);
    }

    private IEnumerator Invincible()
    {
        State.isInvincible = true;

        yield return new WaitForSeconds(Stat.invincibleTime);

        State.isInvincible = false;
    }

    private IEnumerator HitColor()
    {
        while(State.isInvincible)
        {
            Com.mat1.color = Com.hitColor;
            Com.mat2.color = Com.hitColor;
            Com.mat3.color = Com.hitColor;
            yield return new WaitForSeconds(Stat.hitColorTime);
            Com.mat1.color = Com.originalColor;
            Com.mat2.color = Com.originalColor;
            Com.mat3.color = Com.originalColor;
            yield return new WaitForSeconds(Stat.hitColorDelay);
        }
    }

    private void LookUp()
    {
        if (Input.GetKey(Key.lookUp))
        {
            State.isLookUp = true;
        }
        else
        {
            State.isLookUp = false;
        }
    }

    public void ReadyToParry()
    {
        //패링 불가능한 상태
        if (State.isJumping && State.isGrounded || !State.isJumping || State.isGrounded || State.isHit)
        {
            StopCoroutine(parry);
            State.canParry = false;
            Com.parry.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            return;
        }

        if(State.canParry)
        {
            return;
        }

        if (Input.GetKeyDown(Key.jump))
        {
            parry = Parry();
            StartCoroutine(parry);
        }

    }

    public IEnumerator Parry()
    {
        State.canParry = true;

        //effect
        Com.parry.Play();

        yield return new WaitForSeconds(Stat.parryingTime);

        Com.parry.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        State.canParry = false;
    }

    public IEnumerator Parrying()
    {
        State.canParry = false;
        Val.velocityY = Stat.parryingForce;
        Com.animator.SetTrigger("Parrying");

        Stat.pixyEnerge = Mathf.Clamp(Stat.pixyEnerge += Stat.parryingEnerge, 0, 30);

        State.isParrying = true;
        Val.upTrigger = true;

        var delay = DelayGrounded();
        StartCoroutine(delay);

        var parryVFX = CustomPoolManager.Instance.parryPool.SpawnThis(GameManager.instance.playerController.transform.position, Vector3.zero, null);
        parryVFX.Play();
        //parryVFX.transform.DOLocalMove(Vector3.zero, Com.pixy.pixyMoveTime).SetEase(Ease.Unset);

        //if (!State.canCounter)
        //{
        //    State.canCounter = true;
        //    Com.pixy.ReadyToCounter();
        //}


        Com.parry.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        StopCoroutine(parry);

        //시간 단위 무적
        State.isInvincible = true;
        yield return new WaitForSeconds(Stat.parryInvincibleTime);
        State.isInvincible = false;
    }

    IEnumerator DelayGrounded()
    {
        yield return new WaitForSeconds(0.1f);
        State.isParrying = false;
    }

    private void Attack()
    {
        if (State.isHit)
            return;

        if (Input.GetKey(Key.attack))
        {
            State.isAttack = true;
            Com.weapon.Fire();
        }
        else
            State.isAttack = false;
    }

    private void FirePos()
    {
        //스탠딩, 업, 크라우치, 크라우치 업, 
        if (!State.isLookUp && !State.isCrouching)
        {
            Com.weapon.transform.localPosition = Val.FirePos;
            Com.weapon.transform.localEulerAngles = Vector3.zero;
        }
        else if (State.isLookUp && !State.isCrouching)
        {
            Com.weapon.transform.localPosition = Val.upFirePos;
            Com.weapon.transform.localEulerAngles = new Vector3(-90, 0, 0);
        }
        else if (!State.isLookUp && State.isCrouching)
        {
            Com.weapon.transform.localPosition = Val.crouchFirePos;
            Com.weapon.transform.localEulerAngles = Vector3.zero;
        }
        else if (State.isLookUp && State.isCrouching)
        {
            Com.weapon.transform.localPosition = Val.crouchUpFirePos;
            Com.weapon.transform.localEulerAngles = new Vector3(-90, 0, 0);
        }
        else
            Debug.Log("state error");
    }

    public void Counter()
    {
        if(Stat.pixyEnerge < 10f)
        {
            return;
        }

        if (Input.GetKeyDown(Key.counter))
        {
            Stat.pixyEnerge -= 10f;

            Com.pixy.ReadyToCounter();

            //State.canCounter = false;
            //Com.pixy.isReady = false;
            //var counter = Com.pixy.Counter();
            //StartCoroutine(counter);
            //Com.pixy.EndCounter();
        }
    }

    public void HandleAnimation()
    {
        Com.animator.SetFloat("Speed", Mathf.Abs(Com.rigidbody.velocity.x));
        Com.animator.SetBool("isMoving", State.isMoving);
        Com.animator.SetBool("isAttack", State.isAttack);
        Com.animator.SetBool("isCrouching", State.isCrouching);
        Com.animator.SetBool("isJumping", State.isJumping);
        Com.animator.SetBool("isLookUp", State.isLookUp);
        Com.animator.SetBool("isHit", State.isHit);
    }

    public bool CanParry()
    {
        return State.canParry;
    }

    public bool IsInvincible()
    {
        return State.isInvincible;
    }
}
