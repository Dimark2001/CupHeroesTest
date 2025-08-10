using System;
using Cysharp.Threading.Tasks;

public class EnemyView : BaseCharacterView
{
    public override CharacterModel CharacterModel
    {
        get
        {
            if (_model == null)
            {
                CharacterModel = GameResources.EnemyConfigRepository.GetRandom().GetModel();
            }
            return _model; 
        }
        protected set => _model = value;
    }

    private CharacterModel _model;

    private void OnEnable()
    {
        ResetState();
        HpBar.SetValue(CharacterModel.Stats.Value.Health);
        HpBar.SetMaxValue(CharacterModel.Stats.Value.MaxHealth);
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