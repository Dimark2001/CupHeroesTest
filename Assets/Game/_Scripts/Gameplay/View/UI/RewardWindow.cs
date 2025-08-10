using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class RewardWindow : BaseWindow
{
    [SerializeField]
    private Button _damageButton;

    [SerializeField]
    private Button _healthButton;

    [SerializeField]
    private Button _speedButton;

    private HeroUpgradeService _hus;

    [Inject]
    public void Construct(HeroUpgradeService heroUpgradeService)
    {
        _hus = heroUpgradeService;
    }

    private void Start()
    {
        _damageButton.onClick.AddListener(UpgradeDamage);
        _healthButton.onClick.AddListener(UpgradeHealth);
        _speedButton.onClick.AddListener(UpgradeSpeed);
    }

    private void OnDestroy()
    {
        _damageButton.onClick.RemoveListener(UpgradeDamage);
        _healthButton.onClick.RemoveListener(UpgradeHealth);
        _speedButton.onClick.RemoveListener(UpgradeSpeed);
    }

    private void UpgradeDamage()
    {
        _hus.TryUpgradeDamage();
        Hide(true);
    }

    private void UpgradeHealth()
    {
        _hus.TryUpgradeHealth();
        Hide(true);
    }

    private void UpgradeSpeed()
    {
        _hus.TryUpgradeAttackSpeed();
        Hide(true);
    }
}