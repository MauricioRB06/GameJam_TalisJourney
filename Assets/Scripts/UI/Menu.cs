
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
// The Purpose Of This Script Is:
//
//  Set the behavior of the main menu buttons.
//
// Documentation and References:
//
//  C# Enumerators: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/enum
//  Unity OnEnable: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnEnable.html
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        
        [Header("Player Stats Settings")] [Space(5)]
        [Tooltip("Place here the toggle that activates or deactivates FullScreen.")]
        [SerializeField] private Toggle fullScreenToggle;
        [Tooltip("Place the Dropdown containing the screen resolutions here.")]
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [Tooltip("Place here the button that activates the sound.")]
        [SerializeField] private GameObject maxVolume;
        [Tooltip("Place here the button that deactivates the sound.")]
        [SerializeField] private GameObject minVolume;
        
        // Variable to control the display mode.
        private bool _fullScreen;
        
        // Variable to store the screen resolutions supported by the monitor where the application is running.
        private Resolution[] _resolutionList;
        
        // Enumerator containing the levels that can be accessed from the main menu.
        private enum Levels {
            Credits,
            Level_0
        }
        
        // Variable to store the name of the level.
        private Levels _sceneToMove;
        
        // Initial settings.
        private void OnEnable()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            
            // Gets the resolutions of the monitor where the application is running.
            _resolutionList = Screen.resolutions;
            
            // Adds an event to be executed when modifying the value of the toggle or dropdown.
            fullScreenToggle.onValueChanged.AddListener(delegate { SettingsScreenMode(); });
            resolutionDropdown.onValueChanged.AddListener(delegate { SettingsResolution(); });
            
            foreach (var resolution in _resolutionList)
            {
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString()));
            }

            if (!PlayerPrefs.HasKey("DefaultSettings") || PlayerPrefs.GetString("DefaultSettings") != "Default")
            {
                DefaultSettings();
            }
            else
            {
                LoadSettings();
            }
        }
        
        // Change the display mode.
        private void SettingsScreenMode()
        {
            Screen.fullScreen = fullScreenToggle.isOn;
            PlayerPrefs.SetInt("FullScreen", fullScreenToggle.isOn ? 1 : 0);
        }
        
        // Change the resolution of the application.
        private void SettingsResolution()
        {
            if (_resolutionList == null) return;
            
            resolutionDropdown.RefreshShownValue();
                
            Screen.SetResolution(_resolutionList[resolutionDropdown.value].width,
                _resolutionList[resolutionDropdown.value].height, Screen.fullScreen);
            
            PlayerPrefs.SetInt("ScreenResolution", resolutionDropdown.value);
        }
        
        // Activates or deactivates the sound.
        public void ChangeVolume(int volume)
        {
            AudioListener.volume = volume;
            PlayerPrefs.SetInt("Volume", volume);
            
            if (PlayerPrefs.GetInt("Volume", 1) == 1)
            {
                maxVolume.SetActive(true);
                minVolume.SetActive(false);
            }
            else
            {
                maxVolume.SetActive(false);
                minVolume.SetActive(true);
            }
        }
        
        // Load the turotial level.
        public void Play()
        {
            _sceneToMove = Levels.Level_0;
            SceneManager.LoadScene(_sceneToMove.ToString());
        }
        
        // Loads the level of credits.
        public void Credits()
        {
            _sceneToMove = Levels.Credits;
            SceneManager.LoadScene(_sceneToMove.ToString());
        }
        
        // Close the application.
        public void Exit()
        {
            Application.Quit();
        }
        
        // Sets and applies the default settings.
        public void DefaultSettings()
        {
            PlayerPrefs.SetString("DefaultSettings","Default");
            PlayerPrefs.SetInt("FullScreen", 1);
            PlayerPrefs.SetInt("Volume", 1);
            PlayerPrefs.SetInt("ScreenResolution", _resolutionList.Length - 1);
            LoadSettings();
        }
        
        // Applies the settings according to the values set.
        private void LoadSettings()
        {
            var screenMode = PlayerPrefs.GetInt("FullScreen", 1) == 1;
            fullScreenToggle.isOn = PlayerPrefs.GetInt("FullScreen", 1) == 1;

            if (PlayerPrefs.GetInt("Volume", 1) == 1)
            {
                maxVolume.SetActive(true);
                minVolume.SetActive(false);
            }
            else
            {
                maxVolume.SetActive(false);
                minVolume.SetActive(true);
            }
            
            if (_resolutionList != null)
            {
                Screen.SetResolution(
                    _resolutionList[
                        PlayerPrefs.GetInt("ScreenResolution", _resolutionList.Length - 1)].width,
                    _resolutionList[
                        PlayerPrefs.GetInt("ScreenResolution", _resolutionList.Length - 1)].height,
                    Screen.fullScreen = screenMode);

                resolutionDropdown.value = PlayerPrefs.GetInt("ScreenResolution", 
                    _resolutionList.Length - 1);
            }
        }
        
    }
}
