
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    
    public class MessagePopUp : MonoBehaviour
    {
        
        [Header("Message")]
        [Space(5)]
        [Tooltip("Message to display")]
        [SerializeField] private GameObject message;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                message.SetActive(true);
            }
        }
        
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                message.SetActive(false);
            }
        }
        
    }
}
