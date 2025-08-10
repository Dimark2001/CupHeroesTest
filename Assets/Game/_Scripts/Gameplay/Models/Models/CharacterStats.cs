using System;

[Serializable]
public class CharacterStats
{
    public float Health;
    public float MaxHealth;
    public float Damage;
    public float AttackSpeed;

    public static CharacterStats operator +(CharacterStats a, CharacterStats b)
    {
        var health = a.Health + b.Health;
        var maxHealth = a.MaxHealth + b.MaxHealth;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        return new CharacterStats
        {
            Health = health,
            Damage = a.Damage + b.Damage,
            AttackSpeed = a.AttackSpeed + b.AttackSpeed,
            MaxHealth = maxHealth,
        };
    }
}