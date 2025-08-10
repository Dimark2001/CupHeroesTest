using R3;

public class CharacterModel
{
    public ReactiveProperty<CharacterStats> Stats = new(new CharacterStats());
    
    public CharacterModel(CharacterStats stats)
    {
        Stats.Value = stats;
    }
}