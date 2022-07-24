
using System.Collections;
using Player;
using UnityEngine;

namespace PowerUps
{
    // Components required for this Script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CircleCollider2D))]

    public class PowerUpFire : MonoBehaviour
    {
        
        // To configure the projectile properties, based on the launcher settings.
        [SerializeField][Range(1.0f,20.0f)] private float projectileSpeed = 8.0f;
        [SerializeField][Range(2f,8.0f)] private float projectileLife = 5.0f;
        
        // 
        private Vector3 _movementDirection;
        
        // 
        private void Awake()
        {
            if (PlayerController.Instance.FacingDirection == 1)
            {
                _movementDirection = transform.right;
            }
            else
            {
                _movementDirection = -transform.right;
                transform.GetComponent<SpriteRenderer>().flipX = true;
            }
            
            transform.tag = "FireProjectile";
            StartCoroutine(DestroyProjectile());
        }
        
        // Checks if the projectile has not crashed and moves it at the set speed.
        private void Update()
        {
            transform.position += _movementDirection * (projectileSpeed * Time.deltaTime);
        }
        
        // 
        private void OnCollisionEnter2D(Collision2D collision)
        {
            StopCoroutine(DestroyProjectile());
            Destroy(gameObject);
        }
        
        // 
        private IEnumerator DestroyProjectile()
        {
            yield return new WaitForSeconds(projectileLife);
            Destroy(gameObject);
        }
        
    }
}
