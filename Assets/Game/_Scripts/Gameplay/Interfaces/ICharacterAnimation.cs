using Cysharp.Threading.Tasks;

public interface ICharacterAnimation
{
    public UniTask AttackAnimation(ICharacterPresenter target);
    public UniTask DamagedAnimation();
    public UniTask DeathAnimation();
    public UniTask MoveAnimation();
    public UniTask IdleAnimation();
}