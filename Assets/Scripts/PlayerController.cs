using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] public Vector2 movementSpeed = new Vector2(4.0f, 4.0f);

    [Header("Attack")] public Transform attackPoint;
    public float attackRadius = 5.0f;
    public LayerMask enemyLayers;
    public float attackTime = 2.0f;
    public float attackDelay = 4.0f;
    private float _attackTimeCounter = 1000;

    private new Rigidbody2D _rigidbody2D;
    private Vector2 _inputVector = new Vector2(0.0f, 0.0f);
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    [Header("States")] [SerializeField] private string _currentState = "Idle";
    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _canAttack = false;
    [SerializeField] private bool _isAttacking = false;


    void Awake()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _animator = gameObject.GetComponent<Animator>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        UpdateTimers();
        GetInputs();
        HandleStates();
    }

    private void UpdateTimers()
    {
        _attackTimeCounter += Time.deltaTime;
        if (_isAttacking && _attackTimeCounter > attackTime)
            _isAttacking = false;
        if (!_canAttack && _attackTimeCounter > attackDelay)
            _canAttack = true;
    }

    private void GetInputs()
    {
        _inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    private void HandleStates()
    {
        if (_isAttacking)
        {
            _inputVector = Vector2.zero;
        }
        else
        {
            FlipPlayer();
        }

        _isMoving = _inputVector != Vector2.zero;

        string newAnimationState = GetPlayerState();
        if (_currentState != newAnimationState)
        {
            _animator.CrossFade(newAnimationState, 0, 0);
            _currentState = newAnimationState;
        }
    }

    private string GetPlayerState()
    {
        if (_isAttacking) return "Attack";
        if (_isMoving) return "Walk";
        return "Idle";
    }

    private void FlipPlayer()
    {
        var currentScale = transform.localScale;
        float flippedScale = currentScale.x;
        if (_inputVector.x < 0)
            flippedScale = -1;
        if (_inputVector.x > 0)
            flippedScale = 1;
        transform.localScale = new Vector3(flippedScale, currentScale.y, currentScale.z);
    }

    private void Attack()
    {
        //Check timers
        if (!_canAttack) return;

        _isAttacking = true;
        _canAttack = false;
        _attackTimeCounter = 0;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayers);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == this.gameObject)
            {
                Debug.Log("Hit Player:", hitCollider);
                continue;
            }
            Debug.Log("Hit:", hitCollider);
            Destroy(hitCollider.gameObject);
            
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    void FixedUpdate()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + (_inputVector * movementSpeed * Time.fixedDeltaTime));
    }
}