using R3;
using TMPro;
using UnityEngine;
using VContainer;

public class CoinLabelView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    private readonly CompositeDisposable _disposables = new();
  
    [Inject]
    public void Construct(CoinStorage coinStorage)
    {
        coinStorage.CurrentCoins.Subscribe(SetValue).AddTo(_disposables);
    }

    private void SetValue(int value) => _text.text = value.ToString();
}