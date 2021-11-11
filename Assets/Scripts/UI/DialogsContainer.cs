using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogsContainer : MonoBehaviour
{
    [SerializeField] private WinPanel _winPanelPrefab;

    private Canvas _canvas;


    private void Start()
    {
        _canvas = FindObjectOfType<Canvas>();
    }

    public void ShowWinPanelDialog()
    {
        var winPanel = Instantiate(_winPanelPrefab, _canvas.transform);
    }
}
