using UnityEngine;

[CreateAssetMenu(menuName = "Create CharacterConfig", fileName = "CharacterConfig", order = 0)]
public class CharacterConfig : ScriptableObject
{
    [SerializeField]
    private CharacterStats m_CharacterModel;

    public CharacterModel GetModel() => new(m_CharacterModel);

    private void OnValidate()
    {
        if (m_CharacterModel.MaxHealth < m_CharacterModel.Health)
        {
            m_CharacterModel.MaxHealth = m_CharacterModel.Health;
        }
    }
}