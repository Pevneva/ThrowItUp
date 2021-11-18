using System;
using System.Collections;
using System.Collections.Generic;
using Ara;
using UnityEngine;
using UnityEngine.Events;

public class LevelUtils : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private GameObject[] _throwItemsPrefabs;
    [SerializeField] private GameObject[] _targetItemsPrefabs;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private int _amountAttemtp;

    // [SerializeField] private ParticleSystem _flyFxPrefab;

    public int Level { get; private set; }
    public Vector3 StartPosition => _startPosition;
    public Vector3 TargetPosition => _targetPosition.position;
    public event UnityAction<int> LevelChanged;
    
    private ThrownItem _thrownItem;
    private AraTrail _trail;
    private GameObject _targetItem;
    private DialogsContainer _dialogsContainer;
    private FXUtils _fxUtils;
    private List<GameObject> _failThrownItems;
    private ParticleSystem _flyFx;
    private bool _isBluring;
    private MobileBlur _mobileBlur;
    private int _startAttempts;
    private LineRenderer _lineRenderer;
    
    private void Start()
    {
        if (Level == 0)
        {
            LevelChanged?.Invoke(1);
            Level = 1;          
        }

        _dialogsContainer = FindObjectOfType<DialogsContainer>();
        _fxUtils = FindObjectOfType<FXUtils>();
        _lineRenderer = FindObjectOfType<LineRenderer>();
        _failThrownItems = new List<GameObject>();
        InitLevelItems();
        _fxUtils.HideWinFxImmediately();
        _isBluring = false;
        _mobileBlur = Camera.main.GetComponent<MobileBlur>();
        _startAttempts = _amountAttemtp;
    }

    private void Update()
    {
        if (_isBluring)
        {
            _mobileBlur.BlurAmount = Mathf.Lerp(0f, 2, 1f);
        }
    }

    public void InitLevelItems()
    {
        _targetItem = Instantiate(_targetItemsPrefabs[(Level-1) % _throwItemsPrefabs.Length], _targetPosition.position, Quaternion.identity);
        var _throwItemGO = Instantiate(_throwItemsPrefabs[(Level-1) % _targetItemsPrefabs.Length], _startPosition, Quaternion.identity);
        _thrownItem = _throwItemGO.GetComponentInChildren<ThrownItem>();
        _trail = _throwItemGO.GetComponentInChildren<AraTrail>();

        _thrownItem.GetComponent<ThrownItemMover>().LevelPassed += OnLevelPassed;
        _thrownItem.GetComponent<ThrownItemMover>().LevelFailed += OnLevelFailed;
        _thrownItem.GetComponent<ThrownItemInput>().SwipeDone += PlayFlyEffects;
    }

    private void OnLevelPassed()
    {
        _amountAttemtp = _startAttempts;
        // DestroyAllFailThrownItems();
        _dialogsContainer.ShowWinPanelDialog();
        _menuPanel.SetActive(false);
        _fxUtils.ShowWinFx();
    }

    private void OnLevelFailed()
    {
        _amountAttemtp--;
        _failThrownItems.Add(_thrownItem.gameObject);
        
        if (_amountAttemtp <= 0) 
            DoEndGame();
        else
            SetNewThrowItem();
    }

    private void DoEndGame()
    {
        Level = 0;
        _isBluring = true;
        _dialogsContainer.ShowFailScreen();
        _menuPanel.SetActive(false);
        Invoke(nameof(RestartGame), 5);
        _lineRenderer.gameObject.SetActive(false);
    }

    public void IncreaseLevel()
    {
        _thrownItem.GetComponent<ThrownItemMover>().LevelPassed -= OnLevelPassed;
        _thrownItem.GetComponent<ThrownItemMover>().LevelFailed -= OnLevelFailed;
        _thrownItem.GetComponent<ThrownItemInput>().SwipeDone -= PlayFlyEffects;
        
        Level++;
        LevelChanged?.Invoke(Level);
        _menuPanel.SetActive(true);
    }

    private void SetNewThrowItem()
    {
        _thrownItem.GetComponent<ThrownItemMover>().LevelPassed -= OnLevelPassed;
        _thrownItem.GetComponent<ThrownItemMover>().LevelFailed -= OnLevelFailed;
        _thrownItem.GetComponent<ThrownItemInput>().SwipeDone -= PlayFlyEffects;
        
        var _throwItemGO = Instantiate(_throwItemsPrefabs[(Level-1) % _targetItemsPrefabs.Length], _startPosition, Quaternion.identity);
        _thrownItem = _throwItemGO.GetComponentInChildren<ThrownItem>();
        _trail = _throwItemGO.GetComponentInChildren<AraTrail>();
        // _flyFx = Instantiate(_flyFxPrefab, _thrownItem.transform);

        _thrownItem.GetComponent<ThrownItemMover>().LevelPassed += OnLevelPassed;
        _thrownItem.GetComponent<ThrownItemMover>().LevelFailed += OnLevelFailed;
        _thrownItem.GetComponent<ThrownItemInput>().SwipeDone += PlayFlyEffects;
    }

    public void RemoveTargetItems()
    {
        Destroy(_thrownItem.gameObject);
        Destroy(_targetItem);
    }
    
    public void DestroyAllFailThrownItems()
    {
        foreach (var item in _failThrownItems)
            Destroy(item);
        
        _failThrownItems.Clear();
    }

    private void PlayFlyEffects(Vector2 direction)
    {
        // _trail.emit = true;
        // _flyFx.Play();
    }

    private void RestartGame()
    {
        Level = 1;
        LevelChanged?.Invoke(1);
        _fxUtils.HideWinFxImmediately();
        _isBluring = false;
        _mobileBlur.BlurAmount = 0;
        _dialogsContainer.HideFailStamp();
        DestroyAllFailThrownItems();
        RemoveTargetItems();
        _amountAttemtp = _startAttempts;
        InitLevelItems();
        _lineRenderer.gameObject.SetActive(true);
        _menuPanel.SetActive(true);
    }
}
