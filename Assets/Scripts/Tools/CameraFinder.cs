
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
// The Purpose Of This Script Is:
//
//  Define the behavior of the camera plug-in.
//
// Documentation and References:
//
//  Unity Awake: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
//  Unity Start: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
//  Unity Update: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using Cinemachine;
using UnityEngine;

namespace Tools
{
    public class CameraFinder : MonoBehaviour
    {
        
        // Reference to the Cinemachine component.
        private CinemachineVirtualCamera _virtualCamera;
        
        // Reference to the player to be used as target.
        private GameObject _playerReference;
        
        // Variables to know if we are following the player or not.
        private bool _canSeePlayer;
        
        // Initial Settings.
        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
        
        // Search for the player when the level starts.
        private void Start()
        {
            _playerReference = GameObject.FindWithTag("Player");
        }
        
        // If we are not following the player, search for him among all the objects in the scene.
         private void Update() 
         {
             if (_virtualCamera.Follow == null)
             {
                 _virtualCamera.Follow = _playerReference.transform;
             }
         }
         
    }
}
