using System;
using Cysharp.Threading.Tasks;

public interface ICharacterPresenter : IDisposable
{
    public event Action OnDeath;
    UniTask Attack(ICharacterPresenter target);
    UniTask TakeDamage(float damage);
    UniTask Run();
    UniTask Stop();
}