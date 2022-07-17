
using Interfaces;
using UnityEngine;

namespace Platforms
{
    // Component required for this Script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Animator))]
    
    public class Bush : MonoBehaviour, IFireObject
    {
        
        private void OnCollisionEnter2D (Collision2D collision)
        {
            if (collision.gameObject.CompareTag("FireProjectile"))
            {
                FireObjectInteraction();
            }
        }
        
        public void FireObjectInteraction()
        {
            Destroy(gameObject);
        }
    }
}
