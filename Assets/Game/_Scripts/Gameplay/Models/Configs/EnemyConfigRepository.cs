using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create EnemyConfigRepository", fileName = "EnemyConfigRepository", order = 0)]
public class EnemyConfigRepository : ScriptableObject
{
    [SerializeField]
    private List<CharacterConfig> m_Enemies = new();
    
    private Queue<CharacterConfig> Enemies = new();
    
    /*public CharacterConfig GetRandom()
    {
        return m_Enemies[Random.Range(0, m_Enemies.Count)];
    }*/
    
    public CharacterModel GetRandom()
    {
        var hp = Random.Range(20, 60);
        var characterStats = new CharacterStats()
        {
            AttackSpeed = Random.Range(1, 3),
            MaxHealth = hp,
            Health = hp,
            Damage = Random.Range(2, 8),
        };
        
        var characterModel = new CharacterModel(characterStats);
        return characterModel;
    }
}