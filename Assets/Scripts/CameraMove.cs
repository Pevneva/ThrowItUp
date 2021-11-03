using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Vector3 _endPosition;
    [SerializeField] private Vector3 _startRotation;
    [SerializeField] private Vector3 _endRotation;
    [SerializeField] private Vector3[] _wayPoints;
    [SerializeField] private Transform _targetPosition;
    
    private ThrownItemMover _thrownItemMover;
    private float _duration;
    
    private void Start()
    {
        _duration = 1.4f; //to do mover duration + 1 sec
        InitCamera();
    }

    public void InitCamera()
    {
        transform.position = _startPosition;
        transform.rotation = Quaternion.Euler(_startRotation);
    }

    public void MoveIfWin(float duration)
    {
        Sequence seq = DOTween.Sequence();
        Tween moving = transform.DOPath(_wayPoints, duration + 1, PathType.CatmullRom).SetEase(Ease.Linear);//.SetLookAt(0.5f);
        Tween rotation = transform.DORotate(_endRotation, 0.6f);
        seq.Append(moving);
        seq.Insert(duration + 1 - 0.2f, rotation);
    }
}
