using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class HeroAnimationProvider : MonoBehaviour, ICharacterAnimation
{
    [SerializeField]
    private Transform _fireball;
    
    private PlayerObj _playerObj;
    private void Awake()
    {
        _playerObj = GetComponentInChildren<PlayerObj>();
    }

    public async UniTask AttackAnimation(ICharacterPresenter target)
    {
        var presenter = (CharacterPresenter)target;
        var view = (BaseCharacterView)presenter.View;
        await _playerObj.AttackAnimation(target);
        await ParabolicJump(view.transform.position, 2f, 0.3f);
    }

    public async UniTask DamagedAnimation() => await _playerObj.DamagedAnimation();
    public async UniTask DeathAnimation() => await _playerObj.DeathAnimation();
    public async UniTask MoveAnimation() => await _playerObj.MoveAnimation();
    public async UniTask IdleAnimation() => await _playerObj.IdleAnimation();

    private async UniTask ParabolicJump(Vector3 targetPosition, float height, float duration)
    {
        targetPosition += new Vector3(0, 0.5f, 0);
        _fireball.position = transform.position + new Vector3(0.1f, 0.5f, 0);
        _fireball.gameObject.SetActive(true);
        await _fireball.DOJump(targetPosition, height, 1, duration).ToUniTask();
        _fireball.gameObject.SetActive(false);
    }
}