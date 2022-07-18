
using Cinemachine;
using UnityEngine;

namespace Items
{
    public class CameraFinder : MonoBehaviour
    {
        private CinemachineVirtualCamera _virtualCamera;
        private GameObject _playerReference;

        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
        
        private void Start()
        {
            _playerReference = GameObject.FindWithTag("Player");
        }
        
         private void Update() 
         {
             
         if (_virtualCamera.Follow != null) return; 
         
         _virtualCamera.Follow = _playerReference.transform;
         
         }
    }
}
