using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Vector3 _startRotation;
    [SerializeField] private Vector3 _endRotation;
    [SerializeField] private Vector3[] _wayPoints;
    
    private ThrownItemMover _thrownItemMover;
    private bool _isMovingEnded;
    private Sequence _seq;
    private bool _isRotation;
    private Quaternion _lookRotation;
    private Vector3 _targetPosition;
    
    private void Start()
    {
        InitCamera();
    }

    public void InitCamera()
    {
        _seq.Kill();
        _isRotation = false;
        transform.position = _startPosition;
        transform.rotation = Quaternion.Euler(_startRotation);
        Camera.main.fieldOfView = 70;
    }

    public void MoveIfWin(float duration, Vector3 targetPosition)
    {
        _seq = DOTween.Sequence();
        Vector3 cameraWinPosition =
            new Vector3(transform.position.x - 0.25f, transform.position.y + 1.05f, transform.position.z);
        _seq.Append(transform.DOMove(cameraWinPosition,0.35f)).SetEase(Ease.Linear);
        _seq.Insert(0f, Camera.main.DOFieldOfView(40, duration + 2f));
        
        _targetPosition = targetPosition;
        Invoke(nameof(StartRotation), duration);
        Invoke(nameof(EndRotation), duration + 1.5f);
    }

    private void StartRotation()
    {
        _isRotation = true;
    }

    private void EndRotation()
    {
        _isRotation = false;
    }

    private void Update()
    {
        if (_isRotation)
        {
            _lookRotation = Quaternion.LookRotation(_targetPosition - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, _lookRotation, Time.deltaTime * 1.35f);
        }
    }
}
