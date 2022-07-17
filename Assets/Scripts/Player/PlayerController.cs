
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerStatePowerUp
{
    FirePowerUp,
    WindPowerUp,
    ElectricPowerUp,
    SlimePowerUp
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
        [Header("Player Stats Settings")] [Space(5)]
        [ SerializeField ] private Transform groundPoint;
        [ SerializeField ] [Range(3,10)] private float movementSpeed = 6f;
        [ SerializeField ] [Range(5,15)] private float jumpForce = 12f;
        [ SerializeField ] private LayerMask groundLayer;
        [Space(15)]
        
        [Header("PowerUps Settings")] [Space(5)]
        [ SerializeField ] private GameObject firePowerUp;
        [ SerializeField ] private GameObject windPowerUp;
        [ SerializeField ] private GameObject electricPowerUp;
        [ SerializeField ] private GameObject slimePowerUp;
        
        private PlayerStatePowerUp _playerPowerUp;
        private Rigidbody2D _playerRigidbody;
        private Animator _playerAnimator;
        private bool _isGrounded;
        
        // 
        private float _inputX;
        private bool _canAttack;

        // 
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Jump = Animator.StringToHash("Jump");

        // 
        private void Awake()
        {
            _playerPowerUp = PlayerStatePowerUp.FirePowerUp;
            _playerRigidbody = GetComponent<Rigidbody2D>();
            _playerAnimator = GetComponent<Animator>();
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
            }
            else if (_playerRigidbody.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        
        // 
        public void OnMove(InputAction.CallbackContext context)
        {
            _inputX = context.ReadValue<Vector2>().x;
        }
        
        // 
        public void OnJumpFire(InputAction.CallbackContext context)
        {
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
            if (!context.started || !_isGrounded) return;
            
            _playerAnimator.SetTrigger(Jump);
            _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, jumpForce);
            _playerPowerUp  = PlayerStatePowerUp.WindPowerUp;
            _canAttack = true;
            Debug.Log("Wind PowerUp Selected");
        }
        
        // 
        public void OnJumpElectric(InputAction.CallbackContext context)
        {
            if (!context.started || !_isGrounded) return;
            
            _playerAnimator.SetTrigger(Jump);
            _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, jumpForce);
            _playerPowerUp  = PlayerStatePowerUp.ElectricPowerUp;
            _canAttack = true;
            Debug.Log("Electric PowerUp Selected");
        }
        
        // 
        public void OnJumpSlime(InputAction.CallbackContext context)
        {
            if (!context.started || !_isGrounded) return;
            
            _playerAnimator.SetTrigger(Jump);
            _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, jumpForce);
            _playerPowerUp  = PlayerStatePowerUp.SlimePowerUp;
            _canAttack = true;
            Debug.Log("Slime PowerUp Selected");
        }
        
        // 
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!_canAttack) return;
            
            _playerAnimator.SetTrigger(Attack);
            
            switch (context.started)
            {
                case true when _playerPowerUp == PlayerStatePowerUp.FirePowerUp && _canAttack:
                    _canAttack = false;
                    Debug.Log("Fire Attack");
                    break;
                case true when _playerPowerUp == PlayerStatePowerUp.WindPowerUp && _canAttack:
                    _canAttack = false;
                    Debug.Log("Wind Attack");
                    break;
                case true when _playerPowerUp == PlayerStatePowerUp.ElectricPowerUp && _canAttack:
                    _canAttack = false;
                    Debug.Log("Electric Attack");
                    break;
                case true when _playerPowerUp == PlayerStatePowerUp.SlimePowerUp && _canAttack:
                    _canAttack = false;
                    Debug.Log("Slime Attack");
                    break;
            }
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

    }
}
