using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControl : MonoBehaviour
{

    [Header("Settings")]

    private PlayerInputActions inputActions;
    private float movimentInput;
    private Rigidbody2D _rigidbody;

    [SerializeField] [Range(0f, 10f)] private float velocity = 5f;
    private float tVelocity;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    private Vector2 _velocity = Vector2.zero;
    
    [SerializeField] private float jumpForce = 400f;
    private bool isJumping = false;
    
    const float groundedRadius = .2f;
    private bool grounded;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;



    [Header("Events")]

    public UnityEvent OnLandEvent;





    // Start is called before the first frame update
    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.PlayerControls.Move.performed += ctx => movimentInput = ctx.ReadValue<Vector2>().x;
        inputActions.PlayerControls.Jump.performed += ctx => JumpTrigger();
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        tVelocity = velocity * 10;
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
        Vector2 targetVelocity = new Vector2(movimentInput * tVelocity, _rigidbody.velocity.y);
        _rigidbody.velocity = Vector2.SmoothDamp(_rigidbody.velocity, targetVelocity, ref _velocity, movementSmoothing);

        if (grounded && isJumping)
        {
            grounded = false;
            _rigidbody.AddForce(new Vector2(0, jumpForce));
        }
    }


    void JumpTrigger()
    {
        isJumping = true;
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
