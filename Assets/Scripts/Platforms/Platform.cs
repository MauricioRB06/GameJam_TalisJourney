
using System.Collections;
using UnityEngine;

namespace Platforms
{
    // Components required for this Script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    
    public class Platform : MonoBehaviour
    {
        [Header("Platform Settings")] [Space(5)]
        [Tooltip("Default option")]
        [SerializeField] private bool isStaticPlatform = true;
        [Tooltip("Sets whether the platform moves")]
        [SerializeField] private bool isMovablePlatform;
        [Space(15)]
        
        [Header("Platform Fall Settings")] [Space(5)]
        [Tooltip("Set whether the platform falls or not")]
        [SerializeField] private bool platformFalls;
        [Tooltip("Sets the time it takes for the platform to start falling down (in seconds)")]
        [Range(0.5f, 3f)] [SerializeField] private float fallingTime = 1;
        [Space(15)]
        
        [Header("Platform Vibrate Settings")] [Space(5)]
        [Tooltip("Sets whether the platform vibrates before falling")]
        [SerializeField] private bool vibratesBeforeFalling;
        [Tooltip("Sets the vibration value")]
        [Range(0.04f, 0.1f)] [SerializeField] private float vibrationValue = 0.04f;
        [Space(15)]
        
        [Header("Platform Return Settings")] [Space(5)]
        [Tooltip("Sets whether the platform returns after falling")]
        [SerializeField] private bool platformReturn;
        [Tooltip("Sets the time it takes for the platform to return (in seconds)")]
        [Range(3f, 15f)] [SerializeField] private float returnTime = 5;
        [Space(15)]
        
        [Header("Platform Destroy Settings")] [Space(5)]
        [Tooltip("If the platform does not return after falling, set the time it will be destroyed (in seconds)")]
        [Range(1f, 6f)] [SerializeField] private float destroyTime = 3;
        [Space(15)]
        
        [Header("Movable Platform Settings")] [Space(5)]
        [Tooltip("Platform movement speed")]
        [Range(1F, 10F)] [SerializeField] private float movementSpeed = 1;
        [Tooltip("The Waiting Time Between Each Movement")]
        [Range(0.1F, 5F)] [SerializeField] private float movementWaitingTime = 1;
        [Tooltip("There must be at least 2 points on the route")]
        [SerializeField] private Transform[] movementPoints;
        
        // To control the configuration of the components.
        private SpriteRenderer _platformSpriteRenderer;
        private Rigidbody2D _platformRigidBody2D;
        private Animator _platformAnimator;
        private CapsuleCollider2D _platformTriggerCollider;
        private BoxCollider2D _platformCollisionCollider;
        
        // Controls when to start or stop platform vibration.
        private bool _vibrate;
        
        // Save the default mobility state, to reconfigure the platform after returning.
        private bool _staticSettings;
        private bool _movableSettings;
        
        // Default vibration range.
        private float _vibrateAmount = 0.02f;
        
        // Sets the initial position of the platform to know where we have to return to after falling.
        private Vector2 _initialPositionPlatform;
        
        // Stores and modifies the visibility of the platform, to generate a FadeIn effect on return.
        private Color _platformSpriteColor;
        
        // Navigates through the movementPoints to indicate to the object to which it should move.
        private int _movementPointIterator;
        
        // Checks how much time has elapsed at one point before switching to the next point.
        private float _currentWaitTime;
        
        // ID parameters for the animator.
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int Fall = Animator.StringToHash("Fall");
        
        // FadeIn Coroutine when platform returns.
        private IEnumerator PlatformFadeIn()
        {
            for (var platformAlpha = 0.0f; platformAlpha <= 1.0f; platformAlpha += 0.1f)
            {
                _platformSpriteColor.a = platformAlpha;
                _platformSpriteRenderer.material.color = _platformSpriteColor;
                yield return new WaitForSeconds(0.05f);
            }
            
            isStaticPlatform = _staticSettings;
            isMovablePlatform = _movableSettings;
            _platformSpriteColor.a = 1;
            _platformSpriteRenderer.material.color = _platformSpriteColor;
        }
        
        // Coroutine dropping the platform.
        private IEnumerator PlatformFall()
        {
            yield return new WaitForSeconds(fallingTime);
            isMovablePlatform = false;
            _platformAnimator.SetBool(Falling, false);
            _platformAnimator.SetBool(Fall, true);

            _vibrate = false;
            _platformRigidBody2D.isKinematic = false;
            
            if (!platformReturn) yield break;
            
            yield return new WaitForSeconds(returnTime);
            _platformRigidBody2D.velocity = Vector2.zero;
            transform.gameObject.SetActive(false);
            transform.gameObject.SetActive(true);
            _platformRigidBody2D.isKinematic = true;
            _platformAnimator.SetBool(Fall, false);
            transform.position = _initialPositionPlatform;
            _platformSpriteColor.a = 0;
            _platformSpriteRenderer.material.color = _platformSpriteColor;
            StartCoroutine(PlatformFadeIn());
        }
        
        // Checks whether the platform is mobile or static and performs the component assignment.
        private void Awake()
        {
            if ( isStaticPlatform && isMovablePlatform || !isStaticPlatform && !isMovablePlatform)
            {
                isStaticPlatform = true;
                isMovablePlatform = false; 
            }
            
            _staticSettings = isStaticPlatform;
            _movableSettings = isMovablePlatform;
            _platformSpriteRenderer = GetComponent<SpriteRenderer>();
            _platformAnimator = GetComponent<Animator>();
            _platformRigidBody2D = GetComponent<Rigidbody2D>();
            _platformTriggerCollider = GetComponent<CapsuleCollider2D>();
            _platformCollisionCollider = GetComponent<BoxCollider2D>();
        }
        
        // Sets the default settings of the components and in case it is movable, checks if it has movement points.
        private void Start()
        {
            _initialPositionPlatform = transform.position;
            
            _platformRigidBody2D.isKinematic = true;
            _platformRigidBody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _platformRigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionX |
                                               RigidbodyConstraints2D.FreezeRotation;
            
            _platformTriggerCollider.isTrigger = true;
            _platformCollisionCollider.isTrigger = false;
            
            _platformSpriteColor = _platformSpriteRenderer.material.color;

            if (isMovablePlatform && movementPoints.Length < 2)
            {
                Debug.LogError("The platform is configured as movable, " +
                               "but has no points on the route, please set at least 2 points.");
            }
        }
        
        // Checks if the platform is movable and makes it scroll through the list of movement points.
        private void Update()
        {
            if (!isMovablePlatform) return;
            
            transform.position = Vector2.MoveTowards(transform.position, 
                movementPoints[_movementPointIterator].transform.position, 
                movementSpeed * Time.deltaTime);

            if (!(Vector2.Distance(transform.position, 
                movementPoints[_movementPointIterator].transform.position) < 0.1f)) return;
            
            if (_currentWaitTime <= 0) 
            {
                if (movementPoints[_movementPointIterator] != movementPoints[movementPoints.Length - 1])
                {
                    _movementPointIterator++;
                }
                else
                {
                    _movementPointIterator = 0;
                }

                _currentWaitTime = movementWaitingTime;
            }
            else
            {
                _currentWaitTime -= Time.deltaTime; 
            }
        }
        
        // Check if the platform should vibrate when it is about to fall and for how long.
        private void FixedUpdate()
        {
            if (!vibratesBeforeFalling) return;

            if (!_vibrate) return;
            
            var currentPlatformTransform = transform;
            var currentPosition = currentPlatformTransform.position;
            
            currentPosition = new Vector3(currentPosition.x + _vibrateAmount, currentPosition.y, currentPosition.z);
            currentPlatformTransform.position = currentPosition;

            if (transform.position.x >= _initialPositionPlatform.x + vibrationValue ||
                transform.position.x <= _initialPositionPlatform.x - vibrationValue)
            {
                _vibrateAmount *= -1;
            }
        }
        
        // verify the platform configurations, to determine how it will behave when standing on the platform.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (platformFalls)
            {
                case false:
                    return;
                
                case true when !platformReturn:
                {
                    if(!collision.gameObject.CompareTag("Player")) return;
                    _vibrate = true;
                    _platformAnimator.SetBool(Falling, true);
                    StartCoroutine(PlatformFall());
                    collision.transform.SetParent(null);
                    Destroy(gameObject, destroyTime);
                    break;
                }
            }

            if(!collision.gameObject.CompareTag("Player")) return;
            
            _vibrate = true;    
            _platformAnimator.SetBool(Falling, true);
            StartCoroutine(PlatformFall());
        }
       
        // If an entity collides on the platform, it matches it to move the entity with it.
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.transform.CompareTag("Player")) return;
            if (!platformReturn) return;
            
            collision.transform.SetParent(transform);
        }
        
        // If an entity stops colliding with the platform, it disengages from the entity.
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!collision.transform.CompareTag("Player")) return;
            if (!platformReturn) return;
            
            collision.transform.SetParent(null);
        }
        
        // Sets the platform as static.
        public void SetStaticPlatform()
        {
            isStaticPlatform = true;
            isMovablePlatform = false;
        }
        
        // Sets the platform as movable.
        public void SetMovablePlatform()
        {
            isStaticPlatform = false;
            isMovablePlatform = true;
        }

        // Change the platform drop status.
        public void ChangePlatformFallingState()
        {
            platformFalls = !platformFalls;
        }
        
        // Change the platform return status.
        public void ChangePlatformReturnState()
        {
            platformReturn = !platformReturn;
        }
        
        // Change the platform vibration status.
        public void ChangePlatformVibrationState()
        {
            vibratesBeforeFalling = !vibratesBeforeFalling;
        }
    }
}
