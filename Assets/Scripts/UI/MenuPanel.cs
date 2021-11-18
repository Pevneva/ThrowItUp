using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private Button _settingsButtton;
    [SerializeField] private Button _restartButtton;
    [SerializeField] private Button _vibrationPhoneButtton;
    [SerializeField] private Button _phoneButtton;
    [SerializeField] private TMP_Text _levelNumbertext;

    private LevelUtils _levelUtils;
    private bool _isVibrationPhoneShowing;
    private bool _isSettingsPanelOpened;
    
    private void OnEnable()
    {
        Debug.Log("ON ENABLE MenuPanel");
        _settingsButtton.onClick.AddListener(OnClickSettingsButton);
        _restartButtton.onClick.AddListener(OnClickRestartButton);
        _vibrationPhoneButtton.onClick.AddListener(OnClickVibrationButton);
        _phoneButtton.onClick.AddListener(OnClickPhoneButton);

        _levelUtils = FindObjectOfType<LevelUtils>();
        SetLevelNumber(_levelUtils.Level);

        _isVibrationPhoneShowing = true;
    }

    private void OnClickSettingsButton()
    {
        _vibrationPhoneButtton.gameObject.SetActive(_isSettingsPanelOpened == false && _isVibrationPhoneShowing);
        _phoneButtton.gameObject.SetActive(_isSettingsPanelOpened == false && _isVibrationPhoneShowing == false);
        _isSettingsPanelOpened = _isSettingsPanelOpened == false;
    }

    private void OnClickRestartButton()
    {
        _levelUtils.DestroyAllFailThrownItems();
    }

    private void OnClickVibrationButton()
    {
        _vibrationPhoneButtton.gameObject.SetActive(false);
        _phoneButtton.gameObject.SetActive(true);
        _isVibrationPhoneShowing = false;
        Handheld.Vibrate();
    }
    
    private void OnClickPhoneButton()
    {
        _vibrationPhoneButtton.gameObject.SetActive(true);
        _phoneButtton.gameObject.SetActive(false);
        _isVibrationPhoneShowing = true;
    }

    private void SetLevelNumber(int level)
    {
        if (level != 0)
            _levelNumbertext.text = "LEVEL " + level;
        else
            _levelNumbertext.text = "LEVEL " + 1;
    }
}
