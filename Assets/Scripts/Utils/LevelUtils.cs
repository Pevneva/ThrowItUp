using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelUtils : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private GameObject[] _throwItemsPrefabs;
    [SerializeField] private GameObject[] _targetItemsPrefabs;
    [SerializeField] private GameObject _menuPanel;

    public int Level { get; private set; }
    public Vector3 StartPosition => _startPosition;
    public Vector3 TargetPosition => _targetPosition.position;
    public event UnityAction<int> LevelChanged;
    
    private ThrowItem _throwItem;
    private GameObject _targetItem;
    private DialogsContainer _dialogsContainer;
    private FXUtils _fxUtils;
    private List<GameObject> _failThrownItems;
    
    private void Start()
    {
        if (Level == 0)
        {
            LevelChanged?.Invoke(1);
            Level = 1;          
        }

        _dialogsContainer = FindObjectOfType<DialogsContainer>();
        _fxUtils = FindObjectOfType<FXUtils>();
        _failThrownItems = new List<GameObject>();
        InitLevelItems();
        _fxUtils.HideWinFxImmediately();
    }

    private void OnDisable()
    {
        foreach (var throwItem in _throwItemsPrefabs)
        {
            throwItem.GetComponent<ThrownItemMover>().LevelPassed -= OnLevelPassed;
            throwItem.GetComponent<ThrownItemMover>().LevelFailed -= OnLevelFailed;
        }
    }

    public void InitLevelItems()
    {
        _targetItem = Instantiate(_targetItemsPrefabs[(Level-1) % _throwItemsPrefabs.Length], _targetPosition.position, Quaternion.identity);
        var _throwItemGO = Instantiate(_throwItemsPrefabs[(Level-1) % _targetItemsPrefabs.Length], _startPosition, Quaternion.identity);
        _throwItem = _throwItemGO.GetComponentInChildren<ThrowItem>();
 
        _throwItem.GetComponent<ThrownItemMover>().LevelPassed += OnLevelPassed;
        _throwItem.GetComponent<ThrownItemMover>().LevelFailed += OnLevelFailed;
    }

    private void OnLevelPassed()
    {
        DestroyAllFailThrownItems();
        _dialogsContainer.ShowWinPanelDialog();
        _menuPanel.SetActive(false);
        _fxUtils.ShowWinFx();
    }

    private void OnLevelFailed()
    {
        _failThrownItems.Add(_throwItem.gameObject);
        SetNewThrowItem();
    }

    public void IncreaseLevel()
    {
        _throwItem.GetComponent<ThrownItemMover>().LevelPassed -= OnLevelPassed;
        _throwItem.GetComponent<ThrownItemMover>().LevelFailed -= OnLevelFailed;
        
        Level++;
        LevelChanged?.Invoke(Level);
        _menuPanel.SetActive(true);
    }

    private void SetNewThrowItem()
    {
        _throwItem.GetComponent<ThrownItemMover>().LevelPassed -= OnLevelPassed;
        _throwItem.GetComponent<ThrownItemMover>().LevelFailed -= OnLevelFailed;
        
        var _throwItemGO = Instantiate(_throwItemsPrefabs[(Level-1) % _targetItemsPrefabs.Length], _startPosition, Quaternion.identity);
        _throwItem = _throwItemGO.GetComponentInChildren<ThrowItem>();

        _throwItem.GetComponent<ThrownItemMover>().LevelPassed += OnLevelPassed;
        _throwItem.GetComponent<ThrownItemMover>().LevelFailed += OnLevelFailed;
    }

    public void RemoveOldThrowItems()
    {
        Destroy(_throwItem.gameObject);
        Destroy(_targetItem);
    }
    
    public void DestroyAllFailThrownItems()
    {
        foreach (var item in _failThrownItems)
            Destroy(item);
        
        _failThrownItems.Clear();
    }    
}
