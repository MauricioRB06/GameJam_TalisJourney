
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Items
{
    // 
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BoxCollider2D))]
    
    public class LevelChangeTrigger : MonoBehaviour
    {
        private enum Levels{
            Credits,
            Level_0,
            Level_1
        }
        
        [Header("Name Scene To Move")] [Space(10)]
        [Tooltip("Here you must enter the name of the scene you are going to change to")]
        [SerializeField] private Levels sceneToMove;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Player")) return;

            SceneManager.LoadScene(sceneToMove.ToString());
        }
        
    }
}
