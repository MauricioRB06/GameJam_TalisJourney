
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerStatePowerUp
{
    FirePowerUp,
    WindPowerUp
}

namespace Player
{
    // 
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    
    public class PlayerController : MonoBehaviour
    {
        
        public static PlayerController Instance;
        
        [Header("Player Stats Settings")] [Space(5)]
        [ SerializeField ] private Transform groundPoint;
        [ SerializeField ] [Range(3,10)] private float movementSpeed = 6f;
        [ SerializeField ] [Range(5,15)] private float jumpForce = 12f;
        [ SerializeField ] private LayerMask groundLayer;
        [Space(15)]
        
        [Header("PowerUps Settings")] [Space(5)]
        [ SerializeField ] private GameObject firePowerUp;
        [ SerializeField ] private GameObject windPowerUp;
        
        // 
        private bool _attackControllerEnabled = true;
        private bool _moveControllerEnabled = true;
        
        // We use it to detect the direction in which the object is facing
        public int facingDirection;

        // We use it to avoid having to create a new vector every time the character moves
        private Vector2 _newVelocity;
        
        // 
        private bool _isKnockbackActive;
        private float _knockbackStartTime;
        
        // 
        private PlayerStatePowerUp _playerPowerUp;
        private Rigidbody2D _playerRigidbody;
        private Animator _playerAnimator;
        private bool _isGrounded;
        
        //
        public PlayerHealth PlayerHealth { get; private set; }
        
        // 
        private float _inputX;
        private bool _canAttack;

        // 
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Damage = Animator.StringToHash("Damage");
        private static readonly int Death = Animator.StringToHash("Death");

        // 
        private void Awake()
        {
            // 
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            _playerPowerUp = PlayerStatePowerUp.FirePowerUp;
            _playerRigidbody = GetComponent<Rigidbody2D>();
            _playerAnimator = GetComponent<Animator>();
            PlayerHealth = GetComponent<PlayerHealth>();
        }
        
        // 
        private void Update()
        {
            // 
            _playerRigidbody.velocity = new Vector2(_inputX * movementSpeed, _playerRigidbody.velocity.y);
            
            // 
            _isGrounded = Physics2D.OverlapCircle(groundPoint.position, 0.2f, groundLayer);
            
            _playerAnimator.SetFloat(Speed, Mathf.Abs(_playerRigidbody.velocity.x));
            _playerAnimator.SetBool(IsGrounded, _isGrounded);
            
            //
            if (_playerRigidbody.velocity.x > 0)
            {
                transform.localScale = Vector3.one;
                facingDirection = 1;
            }
            else if (_playerRigidbody.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                facingDirection = -1;
            }
        }
        
        // 
        public void OnMove(InputAction.CallbackContext context)
        {
            if (_moveControllerEnabled)
            {
                _inputX = context.ReadValue<Vector2>().x;
            }
        }
        
        // 
        public void OnJumpFire(InputAction.CallbackContext context)
        {
            if (!_moveControllerEnabled) return;
            if (!context.started || !_isGrounded) return;
            
            _playerAnimator.SetTrigger(Jump);
            _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, jumpForce);
            _playerPowerUp  = PlayerStatePowerUp.FirePowerUp;
            _canAttack = true;
            Debug.Log("Fire PowerUp Selected");
        }
        
        // 
        public void OnJumpWind(InputAction.CallbackContext context)
        {
            if (!_moveControllerEnabled) return;
            if (!context.started || !_isGrounded) return;
            
            _playerAnimator.SetTrigger(Jump);
            _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, jumpForce);
            _playerPowerUp  = PlayerStatePowerUp.WindPowerUp;
            _canAttack = true;
            Debug.Log("Wind PowerUp Selected");
        }
        
        // 
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!_attackControllerEnabled) return;
            if (!_canAttack) return;
            
            _playerAnimator.SetTrigger(Attack);
            
            switch (context.started)
            {
                case true when _playerPowerUp == PlayerStatePowerUp.FirePowerUp && _canAttack:
                    _canAttack = false;
                    var transform1 = transform;
                    Instantiate(firePowerUp, transform1.position, transform1.rotation);
                    Debug.Log(facingDirection);
                    break;
                case true when _playerPowerUp == PlayerStatePowerUp.WindPowerUp && _canAttack:
                    _canAttack = false;
                    var transform2 = transform;
                    Instantiate(windPowerUp, transform2.position, transform2.rotation);
                    Debug.Log(facingDirection);
                    break;
            }
        }
        
        public void Dead()
        {
            _playerAnimator.SetTrigger(Death);
            transform.tag = "Dead";
        }
        
        //
        public void KnockBackAnimation()
        {
            _playerAnimator.SetTrigger(Damage);
        }
        
        //
        public void KnockBack(float knockbackDuration, float knockbackPower, Transform obj)
        {
            StartCoroutine(KnockbackImpulse(knockbackDuration, knockbackPower, obj));
        }
        
        private IEnumerator KnockbackImpulse(float knockbackDuration, float knockbackPower, Transform obj)
        {
            float timer = 0;
            
            while (knockbackDuration > timer)
            {
                
                timer += Time.deltaTime;
                Vector2 direction = (transform.position - obj.position).normalized;
                if (Math.Abs(direction.x - -1.0f) < 0.1 || Math.Abs(direction.x - 1.0f) < 0.1)
                {
                    _playerRigidbody.AddForce(new Vector2(direction.x * knockbackPower, direction.y * knockbackPower));
                }
                else
                {
                    _playerRigidbody.AddForce(new Vector2(direction.x * knockbackPower/4, direction.y * knockbackPower/4));
                }
                
            }
            
            yield return 0;
        }
        
        // 
        public void OnPause(InputAction.CallbackContext context)
        {
            Debug.Log("Pause Game");
        }
        
        // 
        public void CanAttack()
        {
            _canAttack  = true;
        }
        
        //
        public void SetVelocityZero()
        {
            _playerRigidbody.velocity = Vector2.zero;
        }
        
        //
        public void ChangeAttackState()
        {
            _attackControllerEnabled = !_attackControllerEnabled;
            Debug.Log(_attackControllerEnabled);
        }
        
        // 
        public void ChangeControllerState()
        {
            _moveControllerEnabled = !_moveControllerEnabled;
            Debug.Log(_moveControllerEnabled);
        }
        
    }
}
