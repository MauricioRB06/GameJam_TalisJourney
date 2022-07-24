
using Player;
using UnityEngine;

namespace Tools
{
    [RequireComponent(typeof(BoxCollider2D))]
    
    public class PlayerAttackChanger : MonoBehaviour
    {
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Player")) return;
            col.GetComponent<PlayerController>().ChangeAttackState();
            Destroy(gameObject);
        }
        
    }
}
