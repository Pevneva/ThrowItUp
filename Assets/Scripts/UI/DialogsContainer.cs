using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogsContainer : MonoBehaviour
{
    [SerializeField] private WinPanel _winPanelPrefab;
    [SerializeField] private FailScreen _failScreen;

    private Canvas _canvas;
    private FailScreen _currentFailScreen;


    private void Start()
    {
        _canvas = FindObjectOfType<Canvas>();
    }

    public void ShowWinPanelDialog()
    {
        var winPanel = Instantiate(_winPanelPrefab, _canvas.transform);
    }
    
    public void ShowFailScreen()
    {
        Invoke(nameof(CreateFailStamp), 0.35f);
    }

    private void CreateFailStamp()
    {
        _currentFailScreen = Instantiate(_failScreen, _canvas.transform);
    }

    public void HideFailStamp()
    {
        Destroy(_currentFailScreen.gameObject);
    } 
        
}
