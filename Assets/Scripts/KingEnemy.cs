using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class KingEnemy : MonoBehaviour
{
    protected Rigidbody2D _rigidbody2D;
    protected Animator _animator;
    public GameObject laughBarFill;

    public int baseHp = 3;
    private int _currentHp = 3;


    public KinguAttackZone attackZone;
    [Header("Movement")] public Vector2 movementSpeed = new Vector2(2.0f, 2.0f);

    // public List<Vector3> pointsOfInterest;
    [SerializeField] protected Vector3 _targetPointOfInterest;
    protected Vector3 _inputVector = Vector2.zero;

    public float laughTime = 3;
    [SerializeField] protected float _laughTimerCounter = 1000;

    public float idleTime = 3;
    [SerializeField] protected float _idleTimerCounter = 1000;

    [Header("States")] [SerializeField] protected string _currentState = "Idle";

    [SerializeField] protected bool _isWaiting = false;

    [FormerlySerializedAs("_isAttacking")] [SerializeField]
    protected bool _isLaughing = false;


    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _currentHp = baseHp;
    }

    private void SelectRandomPointOfInterest()
    {
        _targetPointOfInterest = GameController.Instance.playerController.transform.position;
        attackZone.gameObject.SetActive(true);
        attackZone.canDealDamage = true;
    }

    private void Start()
    {
        SelectRandomPointOfInterest();
    }

    private void Update()
    {
        if (_currentHp <= 0) SceneManager.LoadScene("CG3");
        if (!_isWaiting && Vector2.Distance(this.transform.position, _targetPointOfInterest) < 0.1f)
        {
            _idleTimerCounter = 0;
            _isWaiting = true;
        }

        _inputVector = (_targetPointOfInterest - this.transform.position).normalized;

        float currentHpBarValue = (float)_currentHp / baseHp;
        laughBarFill.transform.parent.gameObject.transform.localScale = transform.localScale;
        laughBarFill.transform.localScale = new Vector3(currentHpBarValue, laughBarFill.transform.localScale.y, 1);
        UpdateTimers();
        HandleStates();
    }

    public virtual void UpdateTimers()
    {
        _idleTimerCounter += Time.deltaTime;
        if (_idleTimerCounter > idleTime && _isWaiting)
        {
            _isWaiting = false;
            SelectRandomPointOfInterest();
        }

        _laughTimerCounter += Time.deltaTime;
        if (_laughTimerCounter > laughTime && _isLaughing)
        {
            _isLaughing = false;

            SelectRandomPointOfInterest();
            _isWaiting = false;
            _idleTimerCounter = 1000;
        }
    }


    protected virtual void HandleStates()
    {
        FlipEnemy();


        if (_isWaiting || _isLaughing)
        {
            _inputVector = Vector2.zero;
        }

        SoundController.Instance.SetSoundActive(SoundController.Instance.kingLaugh1Sfx, _isLaughing);
        string newAnimationState = GetAnimState();

        if (_currentState != newAnimationState)
        {
            _animator.CrossFade(newAnimationState, 0, 0);
            _currentState = newAnimationState;
        }
    }

    protected virtual string GetAnimState()
    {
        if (_isLaughing) return "Laugh";
        if (_isWaiting) return "Idle";
        return "Walk";
    }

    public void MakeMeLaugh()
    {
        _isLaughing = true;
        _laughTimerCounter = 0;

        this._currentHp--;
        if (_currentHp <= 0)
        {
            Destroy(this.gameObject, .5f);
            _animator.CrossFade("Die", 0, 0);
        }
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
            _animator.CrossFade("Die", 0, 0);
        }
    }
}