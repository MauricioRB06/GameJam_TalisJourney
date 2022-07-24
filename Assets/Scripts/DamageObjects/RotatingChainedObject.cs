
using Player;
using UnityEngine;

namespace DamageObjects
{
    // Component required for this Script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CircleCollider2D))]

    public class RotatingChainedObject : MonoBehaviour
    {
        [Header("Rotation Settings")] [Space(5)] 
        [Tooltip("Sets whether the object rotates or not")]
        [SerializeField] private bool isItRotating = true;
        [Tooltip("True to rotate left and False to rotate right")]
        [SerializeField] private bool rotationDirection;
        [Tooltip("Sets the rotation speed")]
        [Range(100f, 300f)] [SerializeField] private float rotationSpeed = 100f;
        [Space(15)]
        
        [Header("Damage Settings")] [Space(5)]
        [Tooltip("Sets the amount of damage it causes when it collides")]
        [Range(5f, 50f)] [SerializeField] private float damageToGive = 10f;
        [Tooltip("If the damage applied is greater than this strength, a HighKnockback will be applied to the player")]
        [Range(20.0f, 100.0f)][SerializeField] private float knockbackForce = 80;
        [Tooltip("It is the duration that the Knockback will last.")]
        [Range(1.0F, 2.0f)][SerializeField] private float knockbackDuration = 1;
        [Space(15)]
        
        [Header("VFX Settings")] [Space(5)]
        [Tooltip("Determine the pivot point of rotation of the pendulum.")]
        [SerializeField] private Transform rotationAxis;
        [Space(15)]

        // 
        private Vector3 _currentRotationDirection;

        // Check that the components necessary for the operation are not empty and instantiate the sound effect.
        private void Awake()
        {
            if (rotationAxis == null)
            {
                Debug.LogError(
                    "<color=#D22323><b>The chained object axis cannot be empty, please add one</b></color>");
            }
        }

        // While the object is rotating, it spins around its axis of rotation at the set speed.
        private void  FixedUpdate()
        {
            if (!isItRotating) return;

            _currentRotationDirection = rotationDirection ? Vector3.forward : Vector3.back;
            transform.RotateAround(rotationAxis.position, _currentRotationDirection,
                rotationSpeed * Time.deltaTime);
        }

        // Check if it collided with the player, to cause the established damage.
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            
            collision.transform.GetComponent<PlayerController>().PlayerHealth.TakeDamage(damageToGive);
            collision.transform.GetComponent<PlayerController>().KnockBackAnimation();
            collision.transform.GetComponent<PlayerController>().KnockBack(knockbackDuration,
                knockbackForce, transform);
        }

    }
}
