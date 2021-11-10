using System;
using UnityEngine;

public class FXUtils : MonoBehaviour
{
    [SerializeField] private GameObject _winFx1;
    [SerializeField] private GameObject _winFx2;

    private bool _isSecondWinFXShowing;

    public void ShowWinFx()
    {
        _isSecondWinFXShowing = true;
        ShowWinFxPart1();
        Invoke(nameof(TryShowWinFxPart2), 1);
    }

    public void HideWinFxImmediately()
    {
        _isSecondWinFXShowing = false;
        HideWinFx1();
        HideWinFx2();
    }
    
    private void ShowWinFxPart1()
    {
        _winFx1.SetActive(true);
    }
    
    public void TryShowWinFxPart2()
    {
        if (_isSecondWinFXShowing)
            _winFx2.SetActive(true);
    }

    public void HideWinFx1()
    {
        _winFx1.SetActive(false);
    }
    
    public void HideWinFx2()
    {
        _winFx2.SetActive(false);
    }
}
