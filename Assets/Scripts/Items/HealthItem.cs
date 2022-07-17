
using Player;
using UnityEngine;

namespace Items
{
    // Components required for this Script to work
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CircleCollider2D))]
    
    public class HealthItem : MonoBehaviour
    {
        [Header("Health Item Settings")][Space(5)]
        [Tooltip("Amount of life the player will gain")]
        [Range(1.0f, 99.0f)][SerializeField] private float healthToGive = 5;
        [Space(15)]

        // To control the behavior of the collider.
        private CircleCollider2D _healthItemCollider;
        
        // Set the initial settings for the item.
        private void Awake()
        {
            _healthItemCollider = GetComponent<CircleCollider2D>();
            
            if (!_healthItemCollider.isTrigger) _healthItemCollider.isTrigger = true;
            
        }
        
        // Check if he collided with the player, to increase his life points.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;

            var healthItemTransform = transform;

            healthItemTransform.GetComponent<SpriteRenderer>().enabled = false;
            _healthItemCollider.enabled = false;

            collision.GetComponent<PlayerHealth>().CureHealth(healthToGive);
            Destroy(gameObject, 0.5f);
        }
    }
}
