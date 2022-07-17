using System.Collections;
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

        private void Awake()
        {
            transform.tag = "WindProjectile";
        }
        
        // Checks if the projectile has not crashed and moves it at the set speed.
        private void Update()
        {
            transform.position += transform.right * (projectileSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            StartCoroutine(DestroyProjectile());
        }

        private IEnumerator DestroyProjectile()
        {
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }
        
    }
}
