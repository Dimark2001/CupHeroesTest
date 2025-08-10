using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public class CharacterPresenter : ICharacterPresenter
{
    public CharacterModel Model { get; protected set; }
    public event Action OnDeath;
    public ICharacterView View { get; protected set; }

    protected readonly CompositeDisposable Disposables = new();

    public CharacterPresenter(ICharacterView view)
    {
        View = view;
        Model = view?.CharacterModel;

        Model?.Stats
            .Subscribe(stats => View.UpdateStats(stats))
            .AddTo(Disposables);
    }

    public async UniTask Attack(ICharacterPresenter target)
    {
        await View.PlayAttackAnimation(target);
        await target.TakeDamage(Model.Stats.Value.Damage);
    }

    public async UniTask TakeDamage(float damage)
    {
        var newStats = new CharacterStats
        {
            Health = -damage,
        };
        Model.Stats.Value += newStats;
        
        if (Model.Stats.Value.Health <= 0)
        {
            await View.PlayDeathAnimation();
            OnDeath?.Invoke();
        }
        else
        {
            await View.PlayDamageAnimation();
        }
    }

    public UniTask Run() => View.PlayMoveAnimation();

    public UniTask Stop() => View.PlayIdleAnimation();
    

    public void Dispose() => Disposables.Dispose();
}

public class HeroPresenter : CharacterPresenter
{
    public HeroPresenter(HeroView view) : base(view)
    {
    }
}

public class EnemyPresenter : CharacterPresenter
{
    public EnemyPresenter() : base(null)
    {

    }

    public void Initialize(EnemyView view)
    {
        View = view;
        Model = view.CharacterModel;

        Model.Stats
            .Subscribe(stats => View.UpdateStats(stats))
            .AddTo(Disposables); 
        
        Model.Stats.Value += new CharacterStats(){Health = -1};
        Model.Stats.Value += new CharacterStats(){Health = 1};
    }
}