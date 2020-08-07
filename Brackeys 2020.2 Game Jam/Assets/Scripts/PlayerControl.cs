using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControl : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private float movimentInput;
    private bool canMove;
    private Rigidbody2D _rigidbody;
    private Transform _transform;
    private GameManager gm;

    private float tVelocity;
    
    private Vector2 _velocity = Vector2.zero;
    private bool isJumping = true;
    
    const float groundedRadius = .2f;
    private bool grounded;
    private bool walking = false;
    private bool right = true;

    [Header("Settings")]

    [SerializeField] [Range(0f, 10f)] private float velocity = 5f;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    [SerializeField] private float jumpForce = 400f;
#pragma warning disable 649
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Animator playerAnim;
#pragma warning restore 649



    [Header("Events")]

    public UnityEvent OnLandEvent;



    // Start is called before the first frame update
    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.PlayerControls.Move.performed += ctx => movimentInput = ctx.ReadValue<Vector2>().x;
        inputActions.PlayerControls.Jump.performed += ctx => JumpTrigger();
        inputActions.PlayerControls.RevertTime.performed += ctx => ChangeTime();
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        playerAnim = GetComponent<Animator>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        gm = GameManager.gm;
        tVelocity = velocity * 10;
        canMove = true;
    }

    void Update()
    {
        if (canMove)
            playerAnim.SetFloat("Speed", Mathf.Abs(movimentInput));

    }

    void FixedUpdate()
    {

        bool wasGrounded = grounded;
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }

        Move();
        isJumping = false;
    }

    public void Move()
    {

        if (canMove)
        {
            Vector2 targetVelocity = new Vector2(movimentInput * tVelocity, _rigidbody.velocity.y);

            _rigidbody.velocity = Vector2.SmoothDamp(_rigidbody.velocity, targetVelocity, ref _velocity, movementSmoothing);

            if (movimentInput > 0 && !right)
            {
                Flip();
            }
            else if (movimentInput < 0 && right)
            {
                Flip();
            }

            if (grounded && isJumping)
            {
                grounded = false;
                _rigidbody.AddForce(new Vector2(0, jumpForce));
                playerAnim.SetBool("Jump", true);
            }

        }
        else
        {
            walking = false;
        }

    }


    void JumpTrigger()
    {
        isJumping = true;
    }

    public void OnLanding()
    {
        playerAnim.SetBool("Jump", false);
    }

    void ChangeTime()
    {
        if (grounded)
            gm.MoveTime();
    }

    public void Freeze()
    {
        _rigidbody.isKinematic = true;
        canMove = false;
        _rigidbody.velocity = new Vector2(0, 0);
    }

    public void Unfreeze()
    {
        canMove = true;
        _rigidbody.isKinematic = false;
    }

    private void Flip()
    {
        right = !right;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
