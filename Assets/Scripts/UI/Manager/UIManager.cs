using System;
using System.Collections.Generic;
using UI.MainScene;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace UI.Manager
{
    public class UIManager : MonoBehaviour, IUIManager, IDisposable
    {
        [SerializeField] private Transform _canvas;
        [SerializeField] private Button _debugDispose;
        [SerializeField] private Button _debugLoad;

        [Inject] private DiContainer _container;
        
        private List<MainSceneViewController> _instantiatedControllers = new();
        private AsyncOperationHandle<GameObject> _sceneAsyncHandle;
        public Transform GetUIRoot => _canvas;
        
        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            _debugDispose.onClick.AddListener(DebugDispose);
            _debugLoad.onClick.AddListener(DebugLoad);
            _debugDispose.gameObject.SetActive(true);
            _debugLoad.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            foreach (var controller in _instantiatedControllers)
            {
                controller.Dispose();
            }
            _instantiatedControllers.Clear();
            
            if (_sceneAsyncHandle.IsValid())
            {
                Addressables.Release(_sceneAsyncHandle);
            }
            
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode = default)
        {
            InstantiateSceneAsset(scene.name);
        }
        
        private void InstantiateSceneAsset(string sceneName)
        {
            string addressableKey = sceneName + "UI";
            _sceneAsyncHandle = Addressables.InstantiateAsync(addressableKey);
            _sceneAsyncHandle.Completed += onCompleted =>
            {
                if (onCompleted.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject resultGO = onCompleted.Result;
                    MainSceneView mainSceneView = resultGO.GetComponent<MainSceneView>();
                    
                    if (mainSceneView != null)
                    {
                        resultGO.transform.SetParent(_canvas);
                        _instantiatedControllers.Add(_container.Instantiate<MainSceneViewController>(new object[] { mainSceneView }));
                    }
                    else
                    {
                        Destroy(resultGO);
                    }
                }
                else
                {
                    Debug.LogError($"Failed to instantiate Addressable with key: {addressableKey}");
                }
            };
        }

        private void DebugDispose()
        {
            _debugDispose.gameObject.SetActive(false);
            _debugLoad.gameObject.SetActive(true);
            Dispose();
        }

        private void DebugLoad()
        {
            _debugLoad.gameObject.SetActive(false);
            _debugDispose.gameObject.SetActive(true);
            SceneManager.LoadScene(0);
        }
    }
}