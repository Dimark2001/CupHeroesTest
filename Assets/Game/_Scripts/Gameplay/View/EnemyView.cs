using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyView : BaseCharacterView
{
    public override CharacterModel CharacterModel
    {
        get
        {
            if (_model == null)
            {
                CharacterModel = GameResources.EnemyConfigRepository.GetRandom();
            }

            return _model;
        }
        protected set => _model = value;
    }

    private CharacterModel _model;

    private void OnEnable()
    {
        ResetState();
        StatBar.UpdateStats(CharacterModel.Stats.Value);
        transform.MoveToScreenPercent(new Vector2(83.45f,70.3f));
    }

    public override async UniTask PlayDeathAnimation()
    {
        await base.PlayDeathAnimation();
    }

    public void ResetState()
    {
        UpdateStats(new CharacterStats()
        {
            AttackSpeed = CharacterModel.Stats.Value.AttackSpeed,
            Damage = CharacterModel.Stats.Value.Damage,
            MaxHealth = CharacterModel.Stats.Value.MaxHealth,
            Health = 1000,
        });
    }
}