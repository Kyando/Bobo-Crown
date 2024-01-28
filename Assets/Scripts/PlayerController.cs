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

    public float stunTime = .5f;
    private float _stunTimeCounter = 1000;

    [Header("Taunt")] public GameObject tauntBarFill;
    public float tauntFillTime;
    [SerializeField] private float _tauntFillTimeCounter;


    [Header("States")] [SerializeField] private string _currentState = "Idle";
    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _canAttack = false;
    [SerializeField] private bool _isAttacking = false;
    [SerializeField] private bool _isStunned = false;
    [SerializeField] private bool _isTaunting = false;
    [SerializeField] private bool _canGetItem = false;


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
        _stunTimeCounter += Time.deltaTime;
        _tauntFillTimeCounter += Time.deltaTime;
        if (_isAttacking && _attackTimeCounter > attackTime)
            _isAttacking = false;
        if (!_canAttack && _attackTimeCounter > attackDelay)
            _canAttack = true;
        if (_isStunned && _stunTimeCounter > stunTime)
            _isStunned = false;
        if (!_isTaunting)
            _tauntFillTimeCounter = 0;
    }

    private void GetInputs()
    {
        _inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }

        _isTaunting = Input.GetKey(KeyCode.F);
        if (Input.GetKeyDown(KeyCode.F)) _canGetItem = true;
        if (Input.GetKeyUp(KeyCode.F)) _canGetItem = false;
    }

    private void HandleStates()
    {
        if (_isAttacking || _isStunned)
        {
            _inputVector = Vector2.zero;
            _isTaunting = false;
        }
        else
        {
            FlipPlayer();
        }

        _isMoving = _inputVector != Vector2.zero;
        if (_isMoving) _isTaunting = false;

        _spriteRenderer.flipX = _isTaunting;

        tauntBarFill.transform.parent.gameObject.SetActive(_isTaunting);
        tauntBarFill.transform.parent.gameObject.transform.localScale = transform.localScale;
        if (_isTaunting)
        {
            float xScale = _tauntFillTimeCounter / tauntFillTime;
            if (xScale >= 1)
            {
                xScale = 1;
                if (_canGetItem)
                {
                    _canGetItem = false;
                    GameController.Instance.SpawnItem();
                }
            }

            tauntBarFill.transform.localScale = new Vector3(xScale, tauntBarFill.transform.localScale.y, 1);
        }

        string newAnimationState = GetPlayerState();
        if (_currentState != newAnimationState)
        {
            _animator.CrossFade(newAnimationState, 0, 0);
            _currentState = newAnimationState;
        }
    }

    private string GetPlayerState()
    {
        if (_isStunned) return "TakeDamage";
        if (_isAttacking) return "Attack";
        if (_isMoving) return "Walk";
        if (_isTaunting) return "Taunt";
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
                continue;
            }

            Debug.Log("Hit:", hitCollider);
            // Destroy(hitCollider.gameObject);
            if (hitCollider.gameObject.GetComponent<SpriteRenderer>() != null)
            {
                GameController.Instance.ScreenShake();
                hitCollider.gameObject.GetComponent<SpriteRenderer>().color =
                    new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f), 1);
            }
        }
    }

    public void TakeDamage()
    {
        _stunTimeCounter = 0;
        _isStunned = true;
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