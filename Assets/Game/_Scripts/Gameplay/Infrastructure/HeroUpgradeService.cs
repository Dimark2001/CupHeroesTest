using VContainer;

public class HeroUpgradeService
{
    private readonly CharacterModel _heroModel;
    private readonly CoinStorage _coinStorage;

    [Inject]
    public HeroUpgradeService(HeroView heroView, CoinStorage coinStorage)
    {
        _heroModel = heroView.CharacterModel;
        _coinStorage = coinStorage;
    }

    public void TryUpgradeHealth()
    {
        var stats = new CharacterStats()
        {
            MaxHealth = 10,
            Health = 10,
        };
        _heroModel.Stats.Value += stats;
    }

    public void TryUpgradeDamage()
    {
        var stats = new CharacterStats()
        {
            Damage = 5
        };
        _heroModel.Stats.Value += stats;
    }

    public void TryUpgradeAttackSpeed()
    {
        var stats = new CharacterStats()
        {
            AttackSpeed = 1f
        };
        _heroModel.Stats.Value += stats;
    }
}