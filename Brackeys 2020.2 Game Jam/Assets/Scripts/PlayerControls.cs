using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    private Transform _transform;

    private Rigidbody2D _rigidbody;

    private PlayerInputActions inputActions;

    private Vector2 movimentInput;

    [Range(0f, 10f)]
    public float velocity = 5f;
    [Range(0f, 0.2f)]
    public float velocitySmoothing = 0f;
    private Vector2 m_Velocity = Vector2.zero;
    private float tVelocity;

    [Range(0f, 10f)]
    public float dashSpeed = 5f;
    [Range(0f, 0.5f)]
    public float dashDuration = 0.1f;
    private float dashTime;
    private Vector2 dashDirection;
    private bool isDashing = false;
    private float tDashSpeed;

    [Range(0f, 10f)]
    public float coolDownTime = 1f;
    private float coolDown;
    private bool dashCoolDown = false;





    // Start is called before the first frame update
    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.PlayerControls.Move.performed += ctx => movimentInput = ctx.ReadValue<Vector2>();
    }

    void Start()
    {
        _transform = GetComponent<Transform>();
        if (_transform == null)
            Debug.LogWarning("transform não encontrado");

        _rigidbody = GetComponent<Rigidbody2D>();
        if (_rigidbody == null)
            Debug.LogWarning("rigidbody não encontrado");

        dashTime = dashDuration;
        coolDown = coolDownTime;
        tDashSpeed = dashSpeed * velocity * 100;

        tVelocity = velocity * 100;
    }

    void FixedUpdate()
    {
        Moviment();
    }

    void Moviment()
    {
        if (!isDashing)
        {
            Vector2 targetVelocity = movimentInput * tVelocity * Time.fixedDeltaTime;
            _rigidbody.velocity = Vector2.SmoothDamp(_rigidbody.velocity, targetVelocity, ref m_Velocity, velocitySmoothing);
        }
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
