using System;
using R3;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using VContainer;

public class GamePresenter : IDisposable
{
    private readonly EnemyFactory _factory;
    private readonly HeroPresenter _hero;
    private readonly CoinStorage _coinStorage;
    private InitiativeService _initiativeService;

    private List<EnemyPresenter> _enemies;
    private CompositeDisposable _disposables = new();
    private bool _isHeroDead;

    [Inject]
    public GamePresenter(HeroPresenter hero, EnemyFactory factory, CoinStorage coinStorage)
    {
        _factory = factory;
        _hero = hero;
    }

    public async UniTask SetupBattleSystem()
    {
        _isHeroDead = false;
        _hero.OnDeath += () => _isHeroDead = true;

        _initiativeService = new InitiativeService();
        _initiativeService.RegisterParticipant(_hero);


        var enemy = _factory.SpawnEnemy();
        enemy.OnDeath += () =>
        {
            _enemies.Remove(enemy);
            _initiativeService.UnregisterParticipant(enemy);
            _coinStorage.AddCoins(10);  
        };
        _enemies = new List<EnemyPresenter> { enemy, };
        _initiativeService.RegisterParticipant(enemy);

        await UniTask.WaitForSeconds(1f);


        await UpdateBattle();
    }


    private async UniTask UpdateBattle()
    {
        while (!IsBattleEnd())
        {
            var current = _initiativeService.GetNextTurnCharacter();

            var enemiesCopy = new List<EnemyPresenter>(_enemies);

            if (current == _hero)
            {
                foreach (var enemy in enemiesCopy)
                {
                    if (_enemies.Contains(enemy))
                    {
                        await current.Attack(enemy);
                        if (IsBattleEnd()) return;
                    }
                }
            }
            else
            {
                if (_enemies.Contains((EnemyPresenter)current))
                {
                    await current.Attack(_hero);
                }
            }
        }
    }

    private bool IsBattleEnd()
    {
        if (_enemies.Count == 0 || _isHeroDead)
        {
            return true;
        }

        return false;
    }

    public void StartHeroMovement() => _hero.Run();
    public void StopHeroMovement() => _hero.Stop();

    public void Dispose() => _disposables?.Dispose();
}