using Cysharp.Threading.Tasks;

public interface ICharacterView
{
    public CharacterModel CharacterModel { get; }
    void UpdateStats(CharacterStats stats);
    
    UniTask PlayAttackAnimation(ICharacterPresenter target);
    UniTask PlayDamageAnimation();
    UniTask PlayDeathAnimation();
    UniTask PlayMoveAnimation();
    UniTask PlayIdleAnimation();
}