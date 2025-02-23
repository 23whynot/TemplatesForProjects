using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.CompositionRoot.SubInstallers;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.SceneMenegment;
using CodeBase.Infrastructure.States;
using CodeBase.Infrastructure.UI.LoadingCurtain;
using CodeBase.Infrastructure.UI.LoadingCurtain.Proxy;
using CodeBase.Services.Log;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.CompositionRoot
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameStateMachine();
            
            BindGameBootstrapperFactory();
            
            BindAssetProvider();
            
            BindGameFactory();
            
            BindLoadingCurtains();
            
            BindLogService();
            
            BindSceneLoader();
        }

        private void BindSceneLoader() => Container.BindInterfacesTo<SceneLoader>().AsSingle();

        private void BindLogService() => Container.BindInterfacesTo<LogService>().AsSingle();

        private void BindAssetProvider() => 
            Container.BindInterfacesTo<AssetsProvider>().AsSingle();
        
        private void BindGameStateMachine() =>
            GameStateMachineInstaller.Install(Container);

        private void BindGameFactory()
        {
            Container
                .Bind<IGameFactory>()
                .FromSubContainerResolve()
                .ByInstaller<GameFactoryInstaller>()
                .AsSingle();
        }
        
        private void BindGameBootstrapperFactory()
        {
            Container
                .BindFactory<GameBootstrapper, GameBootstrapper.Factory>()
                .FromComponentInNewPrefabResource(InfrastructureAssetPath.GameBootstraper);
        }
        
        private void BindLoadingCurtains()
        {
            Container.BindFactory<string, UniTask<LoadingCurtain>, LoadingCurtain.Factory>()
                .FromFactory<PrefabFactoryAsync<LoadingCurtain>>();

            Container.BindInterfacesAndSelfTo<LoadingCurtainProxy>().AsSingle(); // 👈 Это биндинг ILoadingCurtainProxy
        }

    }
}