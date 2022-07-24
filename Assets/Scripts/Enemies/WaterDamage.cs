using Player;
using UnityEngine;

namespace Enemies
{
    // 
    [RequireComponent(typeof(BoxCollider2D))]
    
    public class WaterDamage : MonoBehaviour
    {
        // Check if the object collided with the player to apply damage.
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.transform.CompareTag("Player")) return;
            
            col.transform.GetComponent<PlayerController>().PlayerHealth.TakeDamage(100.0f);
            col.transform.GetComponent<PlayerController>().KnockBackAnimation();
            col.transform.GetComponent<PlayerController>().KnockBack(0, 0, 
                transform);
        }
    }
}
