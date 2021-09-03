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
    }

    [Serializable]
    public class Value
    {
        public float knockBackPower;
        public Vector3 knockBackVelocity;
        public float constDecrease = 12f;
        [Space]
        public Vector3 moveVector;
        public Vector3 moveVelocity;
        public float velocityY;
        public float gravity = 1f;
        [Space]
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
        public bool isCrouching;
        public bool isHit;
        public bool isInvincible;
        public bool isCanParrying;
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
    }

    [Serializable]
    public class InputState
    {
        public bool upPush;
        public bool downPush;
        public bool rightPush;
        public bool leftPush;
        public int currentInput;
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

    public GameObject bullets;
    public List<GameObject> bullet = new List<GameObject>();
    private int bulletCount;
    private Vector3 recentBulletPos;

    private void Awake()
    {
        Stat.Initialize();
        transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
    }

    private void Start()
    {
        for(int i = 0; i < bullets.transform.childCount; i++)
        {
            bullet.Add(bullets.transform.GetChild(i).gameObject);
        }

        bulletCount = 0;
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
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    recentBulletPos = collision.transform.position;
    //    if (State.isCanParrying == true || State.isHitted)
    //    {
    //        return;
    //    }
    //    else
    //    {
    //        StartCoroutine(Hitted(recentBulletPos));
    //    }
    //}

    private void SetInput()
    {
        InputVal.movementInput = 0f;

        if (Input.GetKey(Key.moveLeft))
        {
            //gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            InputVal.movementInput = -1f;
            //InState.leftPush = true;
            //InState.currentInput = 1;
        }
        //else InState.leftPush = false;

        if (Input.GetKey(Key.moveRight))
        {
            //gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            InputVal.movementInput = 1f;
            //InState.rightPush = true;
            //InState.currentInput = 0;
        }
        //else InState.rightPush = false;

        //if (Input.GetKey(Key.lookUp))
        //{
        //    State.isCrouch = false;
        //    InState.upPush = true;
        //}
        //else InState.upPush = false;

        //if (Input.GetKeyDown(Key.attack)) Attack();

        if (!State.isGrounded)
            return;
        //if (Input.GetKey(Key.crouch))
        //{
        //    State.isCrouch = true;
        //    InState.downPush = true;
        //    InState.upPush = false;
        //}
        //else
        //{
        //    State.isCrouch = false;
        //    InState.downPush = false;
        //}
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

        if (State.isCrouching)
        {
            if (Physics.CapsuleCast(crouchCapsulePoint1, crouchCapsulePoint2, Com.crouchCollier.radius * 0.8f, Vector3.right * input.movementInput, out var hit, Val.forwardCheck, layerMask, QueryTriggerInteraction.Ignore))
            {
                State.isForwardBlocked = true;
                //Debug.Log(hit.collider);
            }
        }
        else
        {
            if (Physics.CapsuleCast(capsulePoint1, capsulePoint2, Com.collider.radius * 0.8f, Vector3.right * input.movementInput, out var hit, Val.forwardCheck, layerMask, QueryTriggerInteraction.Ignore))
            {
                State.isForwardBlocked = true;
                //Debug.Log(hit.collider);
            }
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
        //if (!State.isGrounded)
        //{
        //    if (State.isCanParrying)
        //    {
        //        if (Input.GetKey(Key.jump))
        //        {
        //            Val.velocityY = Stat.jumpForce;
        //            StopCoroutine(Parrying());
        //            State.isCanParrying = false;
        //        }
                
        //    }
        //    else return;
        //}
        if (!State.isGrounded)
        {
            return;
        }

        if (Input.GetKey(Key.jump))
        {
            Val.velocityY = Stat.jumpForce;
            State.isJumping = true;
            //State.isCrouching = false;
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

        //if (State.isCrouch)
        //{
        //    Com.collider.enabled = false;
        //    Com.crouchCollier.enabled = true;
        //    Com.standingModel.SetActive(false);
        //    Com.crouchModel.SetActive(true);
        //    Stat.movementSpeed = 5 * 0.7f;
        //}
        //else
        //{
        //    Com.collider.enabled = true;
        //    Com.crouchCollier.enabled = false;
        //    Com.standingModel.SetActive(true);
        //    Com.crouchModel.SetActive(false);
        //    Stat.movementSpeed = 5;
        //}
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


    public IEnumerator Parrying()
    {
        State.isCanParrying = true;
        Debug.Log("DoParrying!!");
        yield return new WaitForSeconds(Stat.parryingTime);
        StartCoroutine(Hitted(recentBulletPos));
        State.isCanParrying = false;
    }

    //private void Attack()
    //{
    //    if (InState.upPush == true && InState.downPush == false && InState.rightPush == false && InState.leftPush == false) // 위만
    //    {
    //        bullet[bulletCount].GetComponent<Bullet>().moveSpeed = Stat.bulletSpeed;
    //        bullet[bulletCount].gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0.5f, 0);
    //        bullet[bulletCount].gameObject.SetActive(true);
    //        bullet[bulletCount].GetComponent<Bullet>().moveDir = 5;
    //    }
    //    else if (InState.upPush == true && InState.downPush == false && InState.rightPush == true && InState.leftPush == false) // 위 우측
    //    {
    //        bullet[bulletCount].GetComponent<Bullet>().moveSpeed = Stat.bulletSpeed;
    //        bullet[bulletCount].gameObject.transform.position = gameObject.transform.position + new Vector3(0.25f, 0.25f, 0);
    //        bullet[bulletCount].gameObject.SetActive(true);
    //        bullet[bulletCount].GetComponent<Bullet>().moveDir = 3;
    //    }
    //    else if (InState.upPush == true && InState.downPush == false && InState.rightPush == false && InState.leftPush == true) // 위 좌측
    //    {
    //        bullet[bulletCount].GetComponent<Bullet>().moveSpeed = Stat.bulletSpeed;
    //        bullet[bulletCount].gameObject.transform.position = gameObject.transform.position + new Vector3(-0.25f, 0.25f, 0);
    //        bullet[bulletCount].gameObject.SetActive(true);
    //        bullet[bulletCount].GetComponent<Bullet>().moveDir = 4;
    //    }
    //    else if (InState.upPush == false && InState.downPush == false && InState.rightPush == true && InState.leftPush == false) // 우측
    //    {
    //        bullet[bulletCount].GetComponent<Bullet>().moveSpeed = Stat.bulletSpeed;
    //        bullet[bulletCount].gameObject.transform.position = gameObject.transform.position + new Vector3(0.5f, 0, 0);
    //        bullet[bulletCount].gameObject.SetActive(true);
    //        bullet[bulletCount].GetComponent<Bullet>().moveDir = 1;
    //    }
    //    else if (InState.upPush == false && InState.downPush == false && InState.rightPush == false && InState.leftPush == true) // 좌측
    //    {
    //        bullet[bulletCount].GetComponent<Bullet>().moveSpeed = Stat.bulletSpeed;
    //        bullet[bulletCount].gameObject.transform.position = gameObject.transform.position + new Vector3(-0.5f, 0, 0);
    //        bullet[bulletCount].gameObject.SetActive(true);
    //        bullet[bulletCount].GetComponent<Bullet>().moveDir = 2;
    //    }
    //    else if (InState.upPush == false && InState.downPush == true && InState.rightPush == false && InState.leftPush == false) // 아래
    //    {
    //        if (InState.currentInput == 0)
    //        {
    //            bullet[bulletCount].GetComponent<Bullet>().moveSpeed = Stat.bulletSpeed;
    //            bullet[bulletCount].gameObject.transform.position = gameObject.transform.position + new Vector3(0.5f, -0.25f, 0);
    //            bullet[bulletCount].gameObject.SetActive(true);
    //            bullet[bulletCount].GetComponent<Bullet>().moveDir = 1;
    //        }
    //        else
    //        {
    //            bullet[bulletCount].GetComponent<Bullet>().moveSpeed = Stat.bulletSpeed;
    //            bullet[bulletCount].gameObject.transform.position = gameObject.transform.position + new Vector3(-0.5f, -0.25f, 0);
    //            bullet[bulletCount].gameObject.SetActive(true);
    //            bullet[bulletCount].GetComponent<Bullet>().moveDir = 2;
    //        }
    //    }
    //    else if (InState.upPush == false && InState.downPush == true && InState.rightPush == true && InState.leftPush == false) // 아래 우측
    //    {
    //        bullet[bulletCount].GetComponent<Bullet>().moveSpeed = Stat.bulletSpeed;
    //        bullet[bulletCount].gameObject.transform.position = gameObject.transform.position + new Vector3(0.5f, -0.25f, 0);
    //        bullet[bulletCount].gameObject.SetActive(true);
    //        bullet[bulletCount].GetComponent<Bullet>().moveDir = 1;
    //    }
    //    else if (InState.upPush == false && InState.downPush == true && InState.rightPush == false && InState.leftPush == true) // 아래 좌측
    //    {
    //        bullet[bulletCount].GetComponent<Bullet>().moveSpeed = Stat.bulletSpeed;
    //        bullet[bulletCount].gameObject.transform.position = gameObject.transform.position + new Vector3(-0.5f, -0.25f, 0);
    //        bullet[bulletCount].gameObject.SetActive(true);
    //        bullet[bulletCount].GetComponent<Bullet>().moveDir = 2;
    //    }
    //    else
    //    {
    //        if (InState.currentInput == 0)
    //        {
    //            bullet[bulletCount].GetComponent<Bullet>().moveSpeed = Stat.bulletSpeed;
    //            bullet[bulletCount].gameObject.transform.position = gameObject.transform.position;
    //            bullet[bulletCount].gameObject.SetActive(true);
    //            bullet[bulletCount].GetComponent<Bullet>().moveDir = 1;
    //        }
    //        else
    //        {
    //            bullet[bulletCount].GetComponent<Bullet>().moveSpeed = Stat.bulletSpeed;
    //            bullet[bulletCount].gameObject.transform.position = gameObject.transform.position;
    //            bullet[bulletCount].gameObject.SetActive(true);
    //            bullet[bulletCount].GetComponent<Bullet>().moveDir = 2;
    //        }
    //    }

    //    if (bulletCount < 99) bulletCount++;
    //    else bulletCount = 0;
    //}

    private IEnumerator Hitted(Vector3 bullet)
    {
        //피격 애니메이션


        // 넉백
        Vector3 direction = (transform.position - bullet).normalized;
        
        //Com.rigidbody.AddForce(new Vector3(direction.x * Val.knockBackPower.x, Val.knockBackPower.y, Val.knockBackPower.z), ForceMode.Impulse);

        if(Stat.hp == 1)
        {
            //게임오버
        }
        else Stat.hp--;

        State.isHit = true;
        yield return new WaitForSeconds(Stat.invincibleTime);
        State.isHit = false;
    }
}
