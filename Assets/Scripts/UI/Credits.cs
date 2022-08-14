
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
//  The Purpose Of This Script Is:
//
//  Defines the layout of the buttons available on the credits map.
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class Credits : MonoBehaviour
    {
        
        // Initial Settings.
        private void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        
        // Loads the main menu level when called.
        public void GoToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
        
    }
}
