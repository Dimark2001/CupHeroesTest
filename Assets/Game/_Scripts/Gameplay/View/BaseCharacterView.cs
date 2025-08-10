using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class BaseCharacterView : MonoBehaviour, ICharacterView
{
    public abstract CharacterModel CharacterModel { get; protected set; }
    protected ICharacterAnimation Animation;
    protected IHpBar HpBar;

    protected virtual void Awake()
    {
        Animation = GetComponentInChildren<ICharacterAnimation>();
        HpBar = GetComponentInChildren<IHpBar>();

        HpBar.SetMaxValue(CharacterModel.Stats.Value.MaxHealth);
        HpBar.SetValue(CharacterModel.Stats.Value.Health);
    }

    public virtual void UpdateStats(CharacterStats stats)
    {
        HpBar.SetValue(stats.Health);
        HpBar.SetMaxValue(stats.MaxHealth);
    }

    public virtual UniTask PlayAttackAnimation(ICharacterPresenter target) => Animation.AttackAnimation(target);
    public virtual UniTask PlayDamageAnimation() => Animation.DamagedAnimation();
    public virtual UniTask PlayDeathAnimation() => Animation.DeathAnimation();
    public virtual UniTask PlayMoveAnimation() => Animation.MoveAnimation();
    public UniTask PlayIdleAnimation() => Animation.IdleAnimation();
}