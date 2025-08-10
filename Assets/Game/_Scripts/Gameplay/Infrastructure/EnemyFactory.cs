using UnityEngine;
using System.Collections.Generic;
using VContainer;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField]
    private EnemyView _enemyPrefab;

    private readonly bool _useObjectPooling = true;
    private readonly int _initialPoolSize = 10;
    private readonly Queue<(EnemyView, EnemyPresenter)> _enemyPool = new();
    private Transform _enemyContainer;

    [Inject]
    private IObjectResolver _container;

    public EnemyPresenter SpawnEnemy()
    {
        var spawnParams = new EnemySpawnParams();
        var presenter = GetOrCreateEnemyView(spawnParams.SpawnPosition);
        return presenter;
    }

    private void Awake()
    {
        _enemyContainer = new GameObject("Enemies").transform;

        if (_useObjectPooling)
        {
            PrewarmPool();
        }
    }

    private void PrewarmPool()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            CreatePooledEnemy();
        }
    }

    private void CreatePooledEnemy()
    {
        var enemy = Instantiate(_enemyPrefab, _enemyContainer);
        var presenter = _container.Resolve<EnemyPresenter>();
        presenter.Initialize(enemy);
        presenter.OnDeath += () => ReturnToPool((enemy, presenter));
        enemy.gameObject.SetActive(false);
        _enemyPool.Enqueue((enemy, presenter));
    }

    private void ReturnToPool((EnemyView, EnemyPresenter) tuple)
    {
        if (!_useObjectPooling)
        {
            return;
        }

        tuple.Item1.ResetState();
        tuple.Item1.gameObject.SetActive(false);
        _enemyPool.Enqueue(tuple);
    }

    private EnemyPresenter GetOrCreateEnemyView(Vector3 position)
    {
        (EnemyView, EnemyPresenter) tuple;
        if (_useObjectPooling && _enemyPool.Count > 0)
        {
            tuple = _enemyPool.Dequeue();
            tuple.Item1.transform.position = position;
            tuple.Item1.gameObject.SetActive(true);
        }
        else
        {
            tuple.Item1 = Instantiate(_enemyPrefab, position, Quaternion.identity, _enemyContainer);
            tuple.Item2 = _container.Resolve<EnemyPresenter>();
            tuple.Item2.Initialize(tuple.Item1);
            tuple.Item2.OnDeath += () => ReturnToPool(tuple);
        }

        return tuple.Item2;
    }

    [System.Serializable]
    public struct EnemySpawnParams
    {
        public Vector3 SpawnPosition => new(3.5f, -7f, 0f);
    }
}