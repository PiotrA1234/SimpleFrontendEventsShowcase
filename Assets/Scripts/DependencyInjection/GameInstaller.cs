using Connection;
using LOEvents;
using Player;
using UI.Manager;
using UnityEngine;
using Zenject;

namespace DependencyInjection
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private UIManager _uiManager;
        
        public override void InstallBindings()
        {
            Container.Bind<IConnectionManager>().To<ConnectionManager>().AsSingle();
            Container.BindInterfacesTo<PlayerDataManager>().AsSingle();
            Container.BindInterfacesTo<LOEventsManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<IUIManager>().FromInstance(_uiManager).AsSingle();
        }
    }
}