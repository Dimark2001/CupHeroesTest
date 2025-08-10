using R3;

public class CoinStorage
{
    public ReactiveProperty<int> CurrentCoins = new(0);

    public void AddCoins(int amount)
    {
        CurrentCoins.Value += amount;
    }

    public bool TrySpendCoins(int amount)
    {
        if (CurrentCoins.Value >= amount)
        {
            CurrentCoins.Value -= amount;
            return true;
        }
        return false;
    }
}