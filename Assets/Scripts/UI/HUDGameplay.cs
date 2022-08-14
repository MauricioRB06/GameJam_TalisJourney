
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
// The Purpose Of This Script Is:
//
//  Sets the behavior of the player's user interface.
//
// Documentation and References:
//
//  Unity OnEnable: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnEnable.html
//  Unity OnDisable: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDisable.html
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class HUDGameplay : MonoBehaviour
    {
        // Singleton Instance.
        public static HUDGameplay Instance;
        
        [Header("HUD Settings")] [Space(5)]
        [Tooltip("Place here the panel that will be shown during the Gameplay.")]
        [SerializeField] private GameObject gameplayPanel;
        [Tooltip("Place here the panel that will be displayed when the game is paused.")]
        [SerializeField] private GameObject pausePanel;
        [Tooltip("Place here the panel that will be displayed when the player dies.")]
        [SerializeField] private GameObject deadPanel;
        [Space(15)]
        
        [Header("Player Stats Settings")] [Space(5)]
        [Tooltip("Place the player's life bar here.")]
        [SerializeField] private Image healthBar;
        [Tooltip("Place the player's life icon here.")]
        [SerializeField] private Image healthIcon;
        [Space(15)]
        
        [Header("PowerUps Settings")] [Space(5)]
        [Tooltip("Place here the sprite when a power is active.")]
        [SerializeField] private Sprite frameEnable;
        [Tooltip("Place here the sprite when a power is inactive.")]
        [SerializeField] private Sprite frameDisable;
        [Tooltip("Place here the PowerUp Fire sprite.")]
        [SerializeField] private Image fireFrame;
        [Tooltip("Place here the PowerUp Wind sprite.")]
        [SerializeField] private Image windFrame;
        [Tooltip("Place here the PowerUp Water sprite.")]
        [SerializeField] private Image waterFrame;
        [Tooltip("Place here the PowerUp Electric sprite.")]
        [SerializeField] private Image electricFrame;
        
        // Variable to block the game pause.
        private bool _lockPause;

        // Initial configurations and subscribing functions to the delegates.
        private void OnEnable()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 1;
            PlayerHealth.PlayerHealthDelegate += Player;
            PlayerController.PlayerPowerUp += PowerUps;
            PlayerController.PlayerPause += GameState;
        }
        
        // Singleton creation.
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
        
        // Function that updates the player's life bar and icon.
        private void Player(float healthAmount)
        {
            healthBar.fillAmount = healthAmount / 100;
            healthIcon.fillAmount = healthAmount / 100;
        }
        
        // Function that updates the icon displayed when changing power.
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
        
        // Restart the level and destroy the object containing this script.
        public void RestartLevel()
        {
            _lockPause = false;
            PlayerController.Instance.KillPlayer();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Destroy(gameObject);
        }
        
        // Exit to the main menu.
        public void ExitToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
        
        // Activates the sequence when the player dies.
        public void Dead()
        {
            Cursor.visible = true;
            _lockPause = true;
            gameplayPanel.SetActive(false);
            deadPanel.SetActive(true);
            Time.timeScale = 0;
        }
        
        // Changes the status of the interface when the pause button is pressed.
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
        
        // Desuscription of functions to the delegates.
        private void OnDisable()
        {
            PlayerHealth.PlayerHealthDelegate -= Player;
            PlayerController.PlayerPowerUp -= PowerUps;
            PlayerController.PlayerPause -= GameState;
        }
        
    }
}
