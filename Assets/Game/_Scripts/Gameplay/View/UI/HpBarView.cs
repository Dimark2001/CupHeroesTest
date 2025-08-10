using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBarView : MonoBehaviour, IHpBar
{
    [SerializeField]
    private Slider _hpBar;

    [SerializeField]
    private TextMeshProUGUI _hpText;

    public void SetValue(float value)
    {
        var diff = value - _hpBar.value;
        _hpBar.value = value;

        
        
        if (diff != 0 && !Mathf.Approximately(_hpBar.value, _hpBar.maxValue))
        {
            ShowPopup(diff);
        }
    }

    public void SetMaxValue(float value)
    {
        if (_hpBar.value > value) return; // TODO: fix()
        
        _hpBar.maxValue = value;
        _hpBar.value = value;
    }

    private void Awake() => _hpBar.onValueChanged.AddListener(ChangeHpText);

    private void OnDestroy() => _hpBar.onValueChanged.RemoveListener(ChangeHpText);

    private void ChangeHpText(float value) => _hpText.text = $"{value}";

    private void ShowPopup(float diff)
    {
        var popup = new GameObject("Popup", typeof(TextMeshProUGUI));
        popup.transform.SetParent(transform, false);

        var text = popup.GetComponent<TextMeshProUGUI>();
        text.text = $"{diff:F0}";
        text.color = Color.red;
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = 6;

        popup.transform.DOLocalMoveY(10, 0.8f).SetEase(Ease.OutQuad).OnComplete(() => Destroy(popup));
        text.DOFade(0, 0.8f).SetEase(Ease.InQuad);
    }
}