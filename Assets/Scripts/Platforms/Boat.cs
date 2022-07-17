
using Interfaces;
using UnityEngine;

namespace Platforms
{
    // Component required for this Script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    
    public class Boat : MonoBehaviour, IWindObject
    {
        private void OnCollisionEnter2D (Collision2D collision)
        {
            if (collision.gameObject.CompareTag("WindProjectile"))
            {
                WindObjectInteraction();
            }
        }
        
        public void WindObjectInteraction()
        {
            Debug.Log("Boat Move");
        }
    }
}
