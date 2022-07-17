
using Interfaces;
using UnityEngine;

namespace Platforms
{
    // Component required for this Script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    
    public class Boat : MonoBehaviour, IWindObject
    {
        
        private Rigidbody2D _rigidbody;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("WindProjectile"))
            {
                WindObjectInteraction();
            }
        }
        
        public void WindObjectInteraction()
        {
            _rigidbody.AddForce(new Vector2(5, 0), ForceMode2D.Impulse);
        }
        
        // 
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.transform.CompareTag("Player")) return;
            collision.transform.SetParent(transform);
        }
        
        // If an entity stops colliding with the platform, it disengages from the entity.
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!collision.transform.CompareTag("Player")) return;
            collision.transform.SetParent(null);
        }
        
    }
}
