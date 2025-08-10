using DG.Tweening;
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
        coinStorage.CurrentCoins
            .Subscribe(SetValue)
            .AddTo(_disposables);
    }

    private void SetValue(int value)
    {
        _text.text = value.ToString();
        ShowPopup(value);
    }

    private void ShowPopup(float diff)
    {
        var popup = new GameObject("Popup", typeof(TextMeshProUGUI));
        popup.transform.SetParent(transform, false);

        var text = popup.GetComponent<TextMeshProUGUI>();
        text.text = $"{diff:F0}";
        text.color = diff > 0 ? Color.yellow : Color.gray;
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = 6;

        popup.transform.DOLocalMoveY(-10, 0.8f).SetEase(Ease.OutQuad).OnComplete(() => Destroy(popup));
        text.DOFade(0, 0.8f).SetEase(Ease.InQuad);
    }
}