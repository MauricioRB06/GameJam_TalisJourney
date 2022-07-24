using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class HUDGameplay : MonoBehaviour
    {
        public static HUDGameplay Instance;
        
        [Header("HUD Settings")] [Space(5)]
        [Tooltip("")]
        [SerializeField] private GameObject gameplayPanel;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject deadPanel;
        [Space(15)]
        
        [Header("Player Stats Settings")] [Space(5)]
        [Tooltip("")]
        [SerializeField] private Image healthBar;
        [SerializeField] private Image healthIcon;
        [Space(15)]
        
        [Header("PowerUps Settings")] [Space(5)]
        [Tooltip("")]
        [SerializeField] private Sprite frameEnable;
        [SerializeField] private Sprite frameDisable;
        [SerializeField] private Image fireFrame;
        [SerializeField] private Image windFrame;
        [SerializeField] private Image waterFrame;
        [SerializeField] private Image electricFrame;
        
        // 
        private bool _lockPause;

        // 
        private void OnEnable()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 1;
            PlayerHealth.PlayerHealthDelegate += Player;
            PlayerController.PlayerPowerUp += PowerUps;
            PlayerController.PlayerPause += GameState;
        }

        private void Awake()
        {
            // 
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        // 
        private void Player(float healthAmount)
        {
            healthBar.fillAmount = healthAmount / 100;
            healthIcon.fillAmount = healthAmount / 100;
        }
        
        // 
        private void PowerUps(int powerUp)
        {
            switch (powerUp)
            {
                case 1:
                    fireFrame.sprite = frameEnable;
                    windFrame.sprite = frameDisable;
                    waterFrame.sprite = frameDisable;
                    electricFrame.sprite = frameDisable;
                    break;
                
                case 2:
                    fireFrame.sprite = frameDisable;
                    windFrame.sprite = frameEnable;
                    waterFrame.sprite = frameDisable;
                    electricFrame.sprite = frameDisable;
                    break;
                
                case 3:
                    fireFrame.sprite = frameDisable;
                    windFrame.sprite = frameDisable;
                    waterFrame.sprite = frameEnable;
                    electricFrame.sprite = frameDisable;
                    break;
                
                case 4:
                    fireFrame.sprite = frameDisable;
                    windFrame.sprite = frameDisable;
                    waterFrame.sprite = frameDisable;
                    electricFrame.sprite = frameEnable;
                    break;
                
                default:
                    fireFrame.sprite = frameDisable;
                    windFrame.sprite = frameDisable;
                    waterFrame.sprite = frameDisable;
                    electricFrame.sprite = frameDisable;
                    break;
            }
        }
        
        // 
        public void RestartLevel()
        {
            _lockPause = false;
            PlayerController.Instance.KillPlayer();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Destroy(gameObject);
        }
        
        // 
        public void ExitToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
        
        // 
        public void Dead()
        {
            Cursor.visible = true;
            _lockPause = true;
            gameplayPanel.SetActive(false);
            deadPanel.SetActive(true);
            Time.timeScale = 0;
        }
        
        // 
        public void GameState(bool gameState)
        {
            if (_lockPause) return;
            
            if (gameState)
            {
                Cursor.visible = false;
                gameplayPanel.SetActive(true);
                pausePanel.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                Cursor.visible = true;
                gameplayPanel.SetActive(false);
                pausePanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
        
        // 
        private void OnDisable()
        {
            PlayerHealth.PlayerHealthDelegate -= Player;
            PlayerController.PlayerPowerUp -= PowerUps;
            PlayerController.PlayerPause -= GameState;
        }
        
    }
}
