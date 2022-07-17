using UnityEngine;

namespace Platforms
{
    // 
    [RequireComponent(typeof(BoxCollider2D))]
    
    public class BoatZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.transform.CompareTag("Boat")) return;
            
            col.transform.GetComponent<Boat>().DestroyBoat();
        }
        
    }
}
