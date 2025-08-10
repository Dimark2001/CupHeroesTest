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
    
    public CharacterConfig GetRandom()
    {
        Enemies = new Queue<CharacterConfig>(m_Enemies);
        var getNext = Enemies.Dequeue();
        Enemies.Enqueue(getNext);
        return getNext;
    }
}