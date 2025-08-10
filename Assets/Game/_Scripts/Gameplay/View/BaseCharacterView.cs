using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class BaseCharacterView : MonoBehaviour, ICharacterView
{
    public abstract CharacterModel CharacterModel { get; protected set; }
    protected ICharacterAnimation Animation;
    protected IStatBar StatBar;

    protected virtual void Awake()
    {
        Animation = GetComponentInChildren<ICharacterAnimation>();
        StatBar = GetComponentInChildren<IStatBar>();

        StatBar.UpdateStats(CharacterModel.Stats.Value);
    }

    public virtual void UpdateStats(CharacterStats stats)
    {
        StatBar.UpdateStats(stats);
    }

    public virtual UniTask PlayAttackAnimation(ICharacterPresenter target) => Animation.AttackAnimation(target);
    public virtual UniTask PlayDamageAnimation() => Animation.DamagedAnimation();
    public virtual UniTask PlayDeathAnimation() => Animation.DeathAnimation();
    public virtual UniTask PlayMoveAnimation() => Animation.MoveAnimation();
    public UniTask PlayIdleAnimation() => Animation.IdleAnimation();
}