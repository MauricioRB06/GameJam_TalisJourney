
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class Credits : MonoBehaviour
    {
        public void GoToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
        
    }
}
