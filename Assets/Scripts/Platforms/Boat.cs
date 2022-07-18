
using Interfaces;
using PowerUps;
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

        [SerializeField][Range(5, 20)] private int boatVelocity = 10;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("WindProjectile")) return;
            
            col.GetComponent<PowerUpWind>().BoatCollision();
            WindObjectInteraction();
        }
        
        public void WindObjectInteraction()
        {
            transform.position += transform.right * (boatVelocity * Time.deltaTime);
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

        public void DestroyBoat()
        {
            Destroy(gameObject);
        }
        
    }
}
