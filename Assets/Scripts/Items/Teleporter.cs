
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
// The Purpose Of This Script Is:
//
//  Sets the behavior of the teleporter that allows switching between levels.
//
// Documentation and References:
//
//  Unity OnTriggerEnter2D: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter2D.html
//  C# Enums: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Items
{
    // Components required for this script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BoxCollider2D))]
    
    public class Teleporter : MonoBehaviour
    {
        // Enumerator containing the values to be used as reference to the game maps.
        private enum Levels{
            Credits,
            Level_0,
            Level_1,
            Level_2,
            Level_3
        }
        
        [Header("Name Scene To Move")] [Space(5)]
        [Tooltip("Here you must set the name of the level you are going to change to.")]
        [SerializeField] private Levels sceneToMove;
        
        // 
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Player")) return;
            SceneManager.LoadScene(sceneToMove.ToString());
        }
        
    }
}
