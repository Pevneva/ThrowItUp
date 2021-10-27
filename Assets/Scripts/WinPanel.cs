using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private Button _next;
    [SerializeField] private TMP_Text _level;

    private LevelUtils _levelUtils;

    public event UnityAction NextLevelOpened; 

    private void Start()
    {
        _levelUtils = FindObjectOfType<LevelUtils>();
        _level.text = "LEVEL " + _levelUtils.Level;
        _next.onClick.AddListener(OnNextButton);
    }

    private void OnNextButton()
    {
        _levelUtils.RemoveOldThrowItems();
        _levelUtils.IncreaseLevel();
        _levelUtils.InitLevelItems();
        NextLevelOpened?.Invoke();
        Destroy(gameObject);
    }
}
