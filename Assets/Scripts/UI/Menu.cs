
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        
        [SerializeField] private Toggle fullScreenToggle;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private GameObject maxVolume;
        [SerializeField] private GameObject minVolume;
        
        // 
        private bool _fullScreen;
//
        private Resolution[] _resolutionList;
        
        private enum Levels{
            Credits,
            Level_0
        }
        
        private Levels _sceneToMove;
        
        //
        private void OnEnable()
        {
            // 
            _resolutionList = Screen.resolutions;
            
            // 
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
        
        // 
        private void SettingsScreenMode()
        {
            Screen.fullScreen = fullScreenToggle.isOn;
            PlayerPrefs.SetInt("FullScreen", fullScreenToggle.isOn ? 1 : 0);
        }
        
        // 
        private void SettingsResolution()
        {
            if (_resolutionList == null) return;
            
            resolutionDropdown.RefreshShownValue();
                
            Screen.SetResolution(_resolutionList[resolutionDropdown.value].width,
                _resolutionList[resolutionDropdown.value].height, Screen.fullScreen);
            
            PlayerPrefs.SetInt("ScreenResolution", resolutionDropdown.value);
        }
        
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
        
        // 
        public void Play()
        {
            _sceneToMove = Levels.Level_0;
            SceneManager.LoadScene(_sceneToMove.ToString());
        }
        
        // 
        public void Credits()
        {
            _sceneToMove = Levels.Credits;
            SceneManager.LoadScene(_sceneToMove.ToString());
        }
        
        // 
        public void Exit()
        {
            Application.Quit();
        }
        
        // 
        public void DefaultSettings()
        {
            PlayerPrefs.SetString("DefaultSettings","Default");
            PlayerPrefs.SetInt("FullScreen", 1);
            PlayerPrefs.SetInt("Volume", 1);
            PlayerPrefs.SetInt("ScreenResolution", _resolutionList.Length - 1);
            LoadSettings();
        }
        
        // 
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
