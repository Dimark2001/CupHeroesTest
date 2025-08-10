using NaughtyAttributes;
using UnityEngine;
using VContainer;

public class BattleTest : MonoBehaviour
{
    [Inject]
    private HeroPresenter _heroPresenter;

    [Inject]
    private EnemyFactory _enemyFactory;
    private EnemyPresenter _enemyPresenter;

    [Button]
    private void StartA()
    {
        _enemyPresenter = _enemyFactory.SpawnEnemy();
    }

    [Button]
    public void Test()
    {
        _heroPresenter.Attack(_enemyPresenter);
    }

    [Button]
    public void TestEnemyAttack()
    {
        _enemyPresenter.Attack(_heroPresenter);
    }
}