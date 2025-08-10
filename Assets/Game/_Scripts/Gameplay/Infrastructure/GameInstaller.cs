using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameInstaller : LifetimeScope
{
    [SerializeField]
    private ParallaxBackground _parallaxBackground;

    [SerializeField]
    private RewardWindow _rewardWindow;

    [SerializeField]
    private HeroView _heroView;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_parallaxBackground);
        builder.RegisterComponent(_rewardWindow);
        
        builder.RegisterComponentInHierarchy<GameStateMachine>();
        builder.RegisterComponentInHierarchy<EnemyFactory>().AsSelf();
        builder.RegisterComponentInHierarchy<CoinLabelView>();
     
        builder.Register<EnemyPresenter>(Lifetime.Transient).As<ICharacterPresenter>().AsSelf();
        builder.Register<GamePresenter>(Lifetime.Singleton);
        builder.Register<CoinStorage>(Lifetime.Singleton).AsSelf();
        builder.Register<HeroUpgradeService>(Lifetime.Singleton);

        RegisterHero();
        RegisterSideTools();

        return;
        void RegisterHero()
        {
            builder.RegisterComponentInHierarchy<HeroView>()
                .As<ICharacterView>()
                .AsSelf();

            builder.Register<HeroPresenter>(Lifetime.Scoped).As<ICharacterPresenter>().AsSelf();
        }

        void RegisterSideTools()
        {
            builder.RegisterEntryPoint(_ => new TimeSpeedController(), Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<BattleTest>();
        }
    }
}