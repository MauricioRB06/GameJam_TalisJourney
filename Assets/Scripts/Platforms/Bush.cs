
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
        private static readonly int Burn = Animator.StringToHash("Burn");
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        private void OnCollisionEnter2D (Collision2D collision)
        {
            if (collision.gameObject.CompareTag("FireProjectile"))
            {
                FireObjectInteraction();
            }
        }
        
        public void FireObjectInteraction()
        {
            _animator.SetTrigger(Burn);
        }

        public void BushFire()
        {
            Destroy(gameObject);
        }
    }
}
