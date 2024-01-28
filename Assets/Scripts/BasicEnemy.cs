using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class BasicEnemy : MonoBehaviour
{
    protected Rigidbody2D _rigidbody2D;
    protected Animator _animator;

    public int baseHp = 3;
    private int _currentHp = 3;
    [Header("Movement")] public Vector2 movementSpeed = new Vector2(2.0f, 2.0f);
    public List<Vector3> pointsOfInterest;
    [SerializeField] protected Vector3 _targetPointOfInterest;
    protected Vector3 _inputVector = Vector2.zero;

    public float idleTime = 3;
    [SerializeField] protected float _idleTimerCounter = 1000;

    [Header("Attack")] public float attackRadius = 5.0f;
    public LayerMask enemyLayers;
    public float attackTime = 2.0f;
    public float attackDelay = 4.0f;
    protected float _attackTimeCounter = 1000;
    public float attackAnimTime = 0.43f;
    protected float _attackAnimTimeCounter = 1000;
    [SerializeField] protected bool _isPlayerInAttackZone = false;


    [Header("States")] [SerializeField] protected string _currentState = "Idle";
    [SerializeField] protected bool _isWaiting = false;
    [SerializeField] protected bool _canAttack = true;
    [SerializeField] protected bool _alreadyAttacked = true;
    [SerializeField] protected bool _isAttacking = false;


    private void Awake()
    {
        SelectRandomPointOfInterest();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _currentHp = baseHp;
    }

    private void SelectRandomPointOfInterest()
    {
        Vector3 nextPoint = _targetPointOfInterest;
        Vector3 randomOffset = new Vector3(Random.Range(0, 0.8f), Random.Range(0, 0.8f), 1);
        int pointIndex = Random.Range(0, pointsOfInterest.Count);
        nextPoint = pointsOfInterest[pointIndex] + randomOffset;

        _targetPointOfInterest = nextPoint;
    }

    public virtual void onPlayerEnterAttackZone()
    {
        _isPlayerInAttackZone = true;
    }

    public virtual void onPlayerExitAttackZone()
    {
        _isPlayerInAttackZone = false;
    }

    private void Update()
    {
        if (_currentHp <= 0) return;
        if (!_isWaiting && Vector2.Distance(this.transform.position, _targetPointOfInterest) < 0.5f)
        {
            SelectRandomPointOfInterest();
            _idleTimerCounter = 0;
            _isWaiting = true;
        }

        _inputVector = (_targetPointOfInterest - this.transform.position).normalized;

        UpdateTimers();
        HandleStates();
    }

    public virtual void UpdateTimers()
    {
        _idleTimerCounter += Time.deltaTime;
        if (_idleTimerCounter > idleTime && _isWaiting)
        {
            _isWaiting = false;
        }

        _attackTimeCounter += Time.deltaTime;
        if (_isAttacking && !_alreadyAttacked && _attackTimeCounter > attackTime)
        {
            _alreadyAttacked = true;
            if (_isPlayerInAttackZone)
            {
                GameController.Instance.DealDamage();
            }
        }

        _attackAnimTimeCounter += Time.deltaTime;
        if (_isAttacking && _attackAnimTimeCounter > attackAnimTime)
        {
            _isAttacking = false;
        }

        if (!_canAttack && _attackTimeCounter > attackDelay)
            _canAttack = true;
    }


    protected virtual void HandleStates()
    {
        FlipEnemy();
        if (_canAttack && _isPlayerInAttackZone)
        {
            _attackTimeCounter = 0;
            _attackAnimTimeCounter = 0;
            _canAttack = false;
            _isAttacking = true;
            _alreadyAttacked = false;
        }


        if (_isWaiting || _isAttacking)
        {
            _inputVector = Vector2.zero;
        }

        string newAnimationState = GetAnimState();

        if (_currentState != newAnimationState)
        {
            _animator.CrossFade(newAnimationState, 0, 0);
            _currentState = newAnimationState;
        }
    }

    protected virtual string GetAnimState()
    {
        if (_isAttacking) return "Attack";
        if (_isWaiting) return "Idle";
        return "Walk";
    }

    private void FixedUpdate()
    {
        if (_currentHp <= 0) return;
        _rigidbody2D.MovePosition(_rigidbody2D.position + (_inputVector * movementSpeed * Time.fixedDeltaTime));
    }

    protected void FlipEnemy()
    {
        var currentScale = transform.localScale;
        float flippedScale = currentScale.x;
        if (_inputVector.x < 0)
            flippedScale = 1;
        if (_inputVector.x > 0)
            flippedScale = -1;
        transform.localScale = new Vector3(flippedScale, currentScale.y, currentScale.z);
    }

    public void TakeDamage()
    {
        this._currentHp--;
        if (_currentHp <= 0)
        {
            Destroy(this.gameObject, .5f);
            _animator.CrossFade("Die",0,0);
        }
    }
}