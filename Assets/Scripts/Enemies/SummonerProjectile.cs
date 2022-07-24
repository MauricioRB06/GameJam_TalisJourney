using System.Collections;
using Player;
using UnityEngine;

namespace Enemies
{
    // Components required for this Script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CircleCollider2D))]
    
    public class SummonerProjectile : MonoBehaviour
    {
        // To configure the projectile properties, based on the launcher settings.
        [SerializeField][Range(2.0f,7.0f)] private float projectileLife = 5.0f;
        [SerializeField][Range(2.0f,10.0f)] private float projectileDamage = 5.0f;
        
        // 
        private float _projectileSpeed;
        private Vector3 _projectileAngle;
        
        // 
        private Vector3 _movementDirection;
        
        // 
        private void Awake()
        {
            transform.tag = "ElectricProjectile";
            _projectileSpeed = Random.Range(4f, 9f);
            _projectileAngle = new Vector3(-1.0f, Random.Range(0.75f,-0.75f),0.0f);
            StartCoroutine(DestroyProjectile());
        }
        
        // Checks if the projectile has not crashed and moves it at the set speed.
        private void Update()
        {
            transform.position += _projectileAngle * (_projectileSpeed * Time.deltaTime);
        }
        
        // 
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.transform.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(projectileDamage);
            }
            
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