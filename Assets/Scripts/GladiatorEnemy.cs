using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GladiatorEnemy : BasicEnemy
{
    public float minTargetX = -5f;
    public float maxTargetX = 5f;
    public GladiatorAttackZone gladiatorAttackZone;

    public override void UpdateTimers()
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
        }

        _attackAnimTimeCounter += Time.deltaTime;
        if (_isAttacking && _attackAnimTimeCounter > attackAnimTime)
        {
            _isAttacking = false;
        }

        if (!_canAttack && _attackTimeCounter > attackDelay)
            _canAttack = true;
    }

    public override void onPlayerEnterAttackZone()
    {
        bool isFacingRight = transform.localScale.x == -1;
        gladiatorAttackZone.canDealDamage = true;
        _canAttack = true;
        gladiatorAttackZone.gameObject.SetActive(true);
        _targetPointOfInterest = new Vector3(isFacingRight ? maxTargetX : minTargetX, transform.position.y, 1);
        _isPlayerInAttackZone = true;
    }

    public override void onPlayerExitAttackZone()
    {
        _isPlayerInAttackZone = false;
    }

    protected override void HandleStates()
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


        if (_isWaiting)
        {
            _inputVector = Vector2.zero;
            _isAttacking = false;
        }

        string newAnimationState = GetAnimState();

        if (_currentState != newAnimationState)
        {
            _animator.CrossFade(newAnimationState, 0, 0);
            _currentState = newAnimationState;
        }
    }
}