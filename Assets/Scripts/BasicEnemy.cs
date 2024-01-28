using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class BasicEnemy : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    [Header("Movement")] public Vector2 movementSpeed = new Vector2(2.0f, 2.0f);
    public List<Vector3> pointsOfInterest;
    [SerializeField] private Vector3 _targetPointOfInterest;
    private Vector3 _inputVector = Vector2.zero;

    public float idleTime = 3;
    [SerializeField] private float _idleTimerCounter = 1000;

    [Header("Attack")] public float attackRadius = 5.0f;
    public LayerMask enemyLayers;
    public float attackTime = 2.0f;
    public float attackDelay = 4.0f;
    private float _attackTimeCounter = 1000;
    public float attackAnimTime = 0.43f;
    private float _attackAnimTimeCounter = 1000;
    [SerializeField] private bool _isPlayerInAttackZone = false;


    [Header("States")] [SerializeField] private string _currentState = "Idle";
    [SerializeField] private bool _isWaiting = false;
    [SerializeField] private bool _canAttack = true;
    [SerializeField] private bool _alreadyAttacked = true;
    [SerializeField] private bool _isAttacking = false;


    private void Awake()
    {
        SelectRandomPointOfInterest();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void SelectRandomPointOfInterest()
    {
        Vector3 nextPoint = _targetPointOfInterest;
        Vector3 randomOffset = new Vector3(Random.Range(0, 0.8f), Random.Range(0, 0.8f), 1);
        int pointIndex = Random.Range(0, pointsOfInterest.Count);
        nextPoint = pointsOfInterest[pointIndex] + randomOffset;

        _targetPointOfInterest = nextPoint;
    }

    public void onPlayerEnterAttackZone()
    {
        _isPlayerInAttackZone = true;
    }

    public void onPlayerExitAttackZone()
    {
        _isPlayerInAttackZone = false;
    }

    private void Update()
    {
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

    private void UpdateTimers()
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


    private void HandleStates()
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

    private string GetAnimState()
    {
        if (_isAttacking) return "Attack";
        if (_isWaiting) return "Idle";
        return "Walk";
    }

    private void FixedUpdate()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + (_inputVector * movementSpeed * Time.fixedDeltaTime));
    }

    private void FlipEnemy()
    {
        var currentScale = transform.localScale;
        float flippedScale = currentScale.x;
        if (_inputVector.x < 0)
            flippedScale = 1;
        if (_inputVector.x > 0)
            flippedScale = -1;
        transform.localScale = new Vector3(flippedScale, currentScale.y, currentScale.z);
    }
}