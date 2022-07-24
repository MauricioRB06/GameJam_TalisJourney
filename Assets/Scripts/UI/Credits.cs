
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class Credits : MonoBehaviour
    {
        
        // 
        private void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        
        // 
        public void GoToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
        
    }
}
