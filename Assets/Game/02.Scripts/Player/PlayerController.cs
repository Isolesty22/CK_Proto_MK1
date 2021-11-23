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

        [Header("pixy")]
        public Pixy pixy;
        public Transform pixyTargetPos;

        public Vector3 pixyPos;
        public Vector3 pixyCrouchPos;
        public Transform pixyFirePos;
        public Transform pixyUltPos;


        [Header("VFX")]
        public ParticleSystem parry;
        public ParticleSystem hit;
        public ParticleSystem walk;
        public ParticleSystem landing;

        [Space]
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

        public float spawnTime = 0.5f;

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
        public bool isLeft;
        public bool moveSystem;
        public bool isAlive;
        public bool counterCheck;
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

        Com.pixyTargetPos.localPosition = Com.pixyPos;

        State.isAlive = true;

        //Com.walk.gameObject.SetActive(false);
    }

    private void Start()
    {
        //Com.walk.Play();
        //MoveSystem(new Vector3(6,0,0));
        //MoveSystem(Vector3.zero);
    }

    private void Update()
    {
        if (!State.moveSystem)
        {
            SetInput();

            FirePos();
            Attack();
            LookUp();
            Crouch();
            Rotate();
            ReadyToParry();

            Counter();

        }
           

        HandleAnimation();
        VFXControl();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        ForwardCheck();
        UpdatePhysics();
        UpCheck();

        if (!State.moveSystem)
        {
            Jump();
        }

        Move();
    }

    private void SetInput()
    {
        if (State.moveSystem)
            return;

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
                //Val.velocityY = -5f;
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

        if (Val.moveVelocity != Vector3.zero)
        {
            if (State.isGrounded)
            {
                if (!State.isCrouching)
                {
                    if (!AudioManager.Instance.Audios.audioSource_PRun.isPlaying)
                    {
                        AudioManager.Instance.Audios.audioSource_PWalk.Stop();
                        AudioManager.Instance.Audios.audioSource_PRun.Play();
                    }
                }
                else
                {
                    if (!AudioManager.Instance.Audios.audioSource_PWalk.isPlaying)
                    {
                        AudioManager.Instance.Audios.audioSource_PRun.Stop();
                        AudioManager.Instance.Audios.audioSource_PWalk.Play();
                    }
                }
            }
        }
        else
        {
            AudioManager.Instance.Audios.audioSource_PWalk.Stop();
            AudioManager.Instance.Audios.audioSource_PRun.Stop();
        }
        //else
        //{
        //    playerSFX.Stop();
        //}

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
            State.isLeft = true;
        }
        else if (InputVal.movementInput == 1f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            transform.localScale = new Vector3(1, 1, 1);
            State.isLeft = false;
        }
    }

    private void Jump()
    {
        if (!State.isGrounded || State.isHit || State.isUpBlocked)
        {
            return;
        }

        if (!Val.prevJump && Input.GetKey(Key.jump))
        {
            AudioManager.Instance.Audios.audioSource_PJump.PlayOneShot(AudioManager.Instance.Audios.audioSource_PJump.clip);
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

            if(!Com.pixy.isAttack)
                //Com.pixy.transform.localPosition = Com.pixy.pixyPos;

            Com.pixyTargetPos.localPosition = Com.pixyPos;

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

            if (!Com.pixy.isAttack)
                //Com.pixy.transform.localPosition = Com.pixy.courchPixyPos;

            Com.pixyTargetPos.localPosition = Com.pixyCrouchPos;
        }
    }

    public void Hit()
    {
        State.isHit = true;
        State.isAttack = false;
        State.isCrouching = false;
        State.isLookUp = false;


        Stat.hp -= 1;
        AudioManager.Instance.Audios.audioSource_PHit.PlayOneShot(AudioManager.Instance.Audios.audioSource_PHit.clip);

        Com.hit.transform.position = transform.position;
        Com.hit.Play();

        //Camera Shake
        GameManager.instance.cameraManager.ShakeCamera();

        if(Stat.hp <1)
        {
            State.isAlive = false;
            Com.animator.SetTrigger("Death");
        }
        else
        {
            Com.animator.SetTrigger("Hit");
            var knockBack = KnockBack();
            StartCoroutine(knockBack);
        }



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

        if (State.isCrouching)
            State.isLookUp = false;
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
        AudioManager.Instance.Audios.audioSource_PParrying.PlayOneShot(AudioManager.Instance.Audios.audioSource_PParrying.clip);

        State.canParry = false;
        Val.velocityY = Stat.parryingForce;
        Com.animator.SetTrigger("Parrying");

        if (!Com.pixy.isUlt)
            Stat.pixyEnerge = Mathf.Clamp(Stat.pixyEnerge += Stat.parryingEnerge, 0, 30);

        State.isParrying = true;
        Val.upTrigger = true;

        var delay = DelayGrounded();
        StartCoroutine(delay);

        var parryVFX = CustomPoolManager.Instance.parryPool.SpawnThis(GameManager.instance.playerController.transform.position, Vector3.zero, null);
        parryVFX.Play();
        //var getParryVFX = CustomPoolManager.Instance.getParryPool.SpawnThis(GameManager.instance.playerController.transform.position, Vector3.zero, null);
        //getParryVFX.Play();

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
        if(Com.pixy.isAttack)
        {
            return;
        }

        if (Input.GetKeyDown(Key.counter))
        {
            if (Stat.pixyEnerge > 9.95f)
            {
                Stat.pixyEnerge -= 10f;
                Com.pixy.ReadyToCounter();
            }
        }

        if (Input.GetKeyDown(Key.ult))
        {
            if (Stat.pixyEnerge > 29.95f)
            {
                Stat.pixyEnerge -= 30f;
                Com.pixy.Ult();
            }
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
        //Com.animator.SetBool("isAlive", State.isAlive);
    }

    public bool CanParry()
    {
        return State.canParry;
    }

    public bool IsInvincible()
    {
        return State.isInvincible;
    }

    public void MoveSystem(Vector3 targetPos)
    {
        //input 안되게 해야 함
        InputVal.movementInput = 0f;
        State.moveSystem = true;
        State.isLookUp = false;
        State.isCrouching = false;
        State.isAttack = false;

        //Debug.Log("work");

        if(transform.position.x < targetPos.x)
        {
            InputVal.movementInput = 1f;
            State.isMoving = true;

            var arrive = ArriveCheck(targetPos);
            StartCoroutine(arrive);
        }
        else
        {
            InputVal.movementInput = -1f;
            State.isMoving = true;
        }
    }

    IEnumerator ArriveCheck(Vector3 targetPos)
    {
        while (true)
        {
            if (transform.position.x > targetPos.x)
            {
                InputVal.movementInput = 0f;
                State.isMoving = false;

                //input 되게 해야 함
                State.moveSystem = false;

                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void FillFullEnerge()
    {
        Stat.pixyEnerge = 10000f;
    }

    public void CounterCheck()
    {
        if(Input.GetKeyDown(Key.counter))
        {
            State.counterCheck = true;
        }
    }

    void VFXControl()
    {
        if (State.isMoving && State.isGrounded && !Com.walk.isPlaying)
        {
            Com.walk.Play();
            //Debug.Log("walk");
            //Com.walk.gameObject.SetActive(true);
        }
        else if(!State.isMoving || !State.isGrounded)
        {
            Com.walk.Stop();
            //Debug.Log("stop walk");
            //Com.walk.gameObject.SetActive(false);
        }

        //if(!State.prevGrounded && State.isGrounded && State.isJumping)
        //{
        //    Com.landing.Play();
        //}

    }
}
