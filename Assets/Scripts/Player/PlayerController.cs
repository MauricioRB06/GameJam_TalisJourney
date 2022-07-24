
using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerStatePowerUp
{
    FirePowerUp,
    WindPowerUp,
    WaterPowerUp,
    ElectricPowerUp
}

namespace Player
{
    // 
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    
    public class PlayerController : MonoBehaviour
    {
        
        public static PlayerController Instance;
        
        [Header("Player Stats Settings")] [Space(5)]
        [ SerializeField ] [Range(3,10)] private float movementSpeed = 6f;
        [ SerializeField ] [Range(5,15)] private float jumpForce = 12f;
        [ SerializeField ] private LayerMask groundLayer;
        [ SerializeField ] private Transform groundPoint;
        [ SerializeField ] private Transform wallPoint;
        [Space(15)]
        
        [Header("PowerUps Settings")] [Space(5)]
        [ SerializeField ] private GameObject firePowerUp;
        [ SerializeField ] private GameObject windPowerUp;
        [ SerializeField ] private GameObject waterPowerUp;
        [ SerializeField ] private GameObject electricPowerUp;
        [Space(15)]
        
        [Header("PowerUps Settings")] [Space(5)]
        [SerializeField] private AudioClip playerJump;
        [SerializeField] private AudioClip playerWalk;
        [SerializeField] private AudioClip playerDead;
        
        // 
        public static event Action<int> PlayerPowerUp;
        public static event Action<bool> PlayerPause;
        
        private bool _isPaused;
        // 
        private bool _attackControllerEnabled = true;
        private bool _moveControllerEnabled = true;
        
        // We use it to detect the direction in which the object is facing
        private int _facingDirection;
        public int FacingDirection => _facingDirection;

        // We use it to avoid having to create a new vector every time the character moves
        private Vector2 _newVelocity;
        
        // 
        private bool _isKnockbackActive;
        private float _knockbackStartTime;
        
        // 
        private PlayerStatePowerUp _playerPowerUp;
        private Rigidbody2D _playerRigidbody;
        private Animator _playerAnimator;
        private AudioSource _playerAudioSource;
        
        private bool _isGrounded;
        private bool _wallCheck;
        
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
            _playerAudioSource = GetComponent<AudioSource>();
            _isPaused = false;
            _attackControllerEnabled = true;
            _moveControllerEnabled = true;
        }
        
        // 
        private void Update()
        {
            // 
            _isGrounded = Physics2D.OverlapCircle(groundPoint.position, 0.2f, groundLayer);
            _wallCheck = Physics2D.Raycast(wallPoint.position,
                Vector2.right * _facingDirection, 0.4f, groundLayer);
            
            //
            if (!_wallCheck)
            {
                _playerRigidbody.velocity = new Vector2(_inputX * movementSpeed, _playerRigidbody.velocity.y);
            }
            else
            {
                if (!_isGrounded && ((_facingDirection == 1 && _inputX < 0) || (_facingDirection == -1 && _inputX > 0)))
                {
                    _playerRigidbody.velocity = new Vector2(_inputX * movementSpeed, _playerRigidbody.velocity.y);
                }
                else if (!_isGrounded)
                {
                    _playerRigidbody.velocity = new Vector2(0f, -5.0f);
                }
                else if ((_facingDirection == 1 && _inputX < 0) || (_facingDirection == -1 && _inputX > 0))
                {
                    _playerRigidbody.velocity = new Vector2(_inputX * movementSpeed, _playerRigidbody.velocity.y);
                }
            }
            
            _playerAnimator.SetFloat(Speed, Mathf.Abs(_playerRigidbody.velocity.x));
            _playerAnimator.SetBool(IsGrounded, _isGrounded);
            
            //
            if (_playerRigidbody.velocity.x > 0)
            {
                transform.localScale = Vector3.one;
                _facingDirection = 1;
            }
            else if (_playerRigidbody.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                _facingDirection = -1;
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
            _playerAudioSource.PlayOneShot(playerJump);
            _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, jumpForce);
            _playerPowerUp  = PlayerStatePowerUp.FirePowerUp;
            PlayerPowerUp?.Invoke(1);
            _canAttack = true;
        }
        
        // 
        public void OnJumpWind(InputAction.CallbackContext context)
        {
            if (!_moveControllerEnabled) return;
            if (!context.started || !_isGrounded) return;
            
            _playerAnimator.SetTrigger(Jump);
            _playerAudioSource.PlayOneShot(playerJump);
            _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, jumpForce);
            _playerPowerUp  = PlayerStatePowerUp.WindPowerUp;
            PlayerPowerUp?.Invoke(2);
            _canAttack = true;
        }
        
        // 
        public void OnJumpWater(InputAction.CallbackContext context)
        {
            if (!_moveControllerEnabled) return;
            if (!context.started || !_isGrounded) return;
            
            _playerAnimator.SetTrigger(Jump);
            _playerAudioSource.PlayOneShot(playerJump);
            _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, jumpForce);
            _playerPowerUp  = PlayerStatePowerUp.WaterPowerUp;
            PlayerPowerUp?.Invoke(3);
            _canAttack = true;
        }
        
        // 
        public void OnJumpElectric(InputAction.CallbackContext context)
        {
            if (!_moveControllerEnabled) return;
            if (!context.started || !_isGrounded) return;
            
            _playerAnimator.SetTrigger(Jump);
            _playerAudioSource.PlayOneShot(playerJump);
            _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, jumpForce);
            _playerPowerUp  = PlayerStatePowerUp.ElectricPowerUp;
            PlayerPowerUp?.Invoke(4);
            _canAttack = true;
        }
        
        // 
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!_attackControllerEnabled) return;
            if (!_canAttack) return;
            
            _playerAnimator.SetTrigger(Attack);
            
            var playerTransform = transform;
            
            switch (context.started)
            {
                case true when _playerPowerUp == PlayerStatePowerUp.FirePowerUp && _canAttack:
                    _canAttack = false;
                    Instantiate(firePowerUp, playerTransform.position, playerTransform.rotation);
                    break;
                
                case true when _playerPowerUp == PlayerStatePowerUp.WindPowerUp && _canAttack:
                    _canAttack = false;
                    Instantiate(windPowerUp, playerTransform.position, playerTransform.rotation);
                    break;
                
                case true when _playerPowerUp == PlayerStatePowerUp.WaterPowerUp && _canAttack:
                    _canAttack = false;
                    Instantiate(waterPowerUp, playerTransform.position, playerTransform.rotation);
                    break;
                
                case true when _playerPowerUp == PlayerStatePowerUp.ElectricPowerUp && _canAttack:
                    _canAttack = false;
                    Instantiate(electricPowerUp, playerTransform.position, playerTransform.rotation);
                    break;
            }
        }
        
        // 
        public void Dead()
        {
            _playerAnimator.SetTrigger(Death);
            transform.tag = "Dead";
            _playerAudioSource.PlayOneShot(playerDead);
        }
        
        //
        public void ActivateDeadHUD()
        {
            HUDGameplay.Instance.Dead();
        }
        
        //
        public void KillPlayer()
        {
            Destroy(gameObject);
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
            if (context.started)
            {
                PlayerPause?.Invoke(!_isPaused);
                _isPaused = !_isPaused;
            }
        }
        
        // 
        public void CanAttack()
        {
            _canAttack  = true;
        }

        //
        public void ChangeAttackState()
        {
            _attackControllerEnabled = !_attackControllerEnabled;
        }
        
        // 
        public void ChangeControllerState()
        {
            _moveControllerEnabled = !_moveControllerEnabled;
        }

        public void PlayerWalk()
        {
            if (!_isGrounded) return;
            _playerAudioSource.PlayOneShot(playerWalk);
        }
        
    }
}
