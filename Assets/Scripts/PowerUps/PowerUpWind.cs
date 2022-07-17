
using System.Collections;
using Player;
using UnityEngine;

namespace PowerUps
{
    // Components required for this Script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CircleCollider2D))]
    
    public class PowerUpWind : MonoBehaviour
    {
        
        // To configure the projectile properties, based on the launcher settings.
        [SerializeField][Range(1.0f,20.0f)] private float projectileSpeed = 10.0f;
        
        private Vector3 _movementDirection;
        
        private void Awake()
        {
            if (PlayerController.Instance.facingDirection == 1)
            {
                _movementDirection = transform.right;
            }
            else
            {
                _movementDirection = -transform.right;
                transform.GetComponent<SpriteRenderer>().flipX = true;
            }
            
            transform.tag = "WindProjectile";
        }
        
        // Checks if the projectile has not crashed and moves it at the set speed.
        private void Update()
        {
            transform.position += _movementDirection * (projectileSpeed * Time.deltaTime);
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Destroy(gameObject);
        }
        
    }
}
