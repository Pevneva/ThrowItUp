using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ThrownItem), typeof(ThrownItemChecker), typeof(ThrownItemMover))]
public class ThrownItemChecker : MonoBehaviour
{
    private Vector3 _targetPoint;
    private Vector3 _startPoint;
    private Vector2 _targetScreenPoint; 
    private Vector2 _startScreenPoint; 
    private Vector3 _targetFailVector;
    private Vector2 _targetWorldDirection;
    private Vector2 _targetScreenDirection;
    
    private LevelUtils _levelUtils;
    private ThrownItem _thrownItem;
    private ThrownItemInput _throwItemInput;
    private ThrownItemMover _itemMover;
    
    private bool _isPassedMoving;
    private bool _isFailedMoving;
    private bool _isClockWiseSwiping;
    
    private float _angleDeviation;
    private float _allowedAngleDeviation;
    private float _targetAngleWithYAxe;
    private Vector2 _middleVectorPoint;
    
    private void Start()
    {
        _allowedAngleDeviation = 2.5f;
        _levelUtils = FindObjectOfType<LevelUtils>();
        _thrownItem = GetComponent<ThrownItem>();
        _throwItemInput = GetComponent<ThrownItemInput>();
        _itemMover = GetComponent<ThrownItemMover>();
        
        InitPoints();

        _middleVectorPoint = VectorUtils.GetMiddleVectorPoint(new Vector2(_startPoint.x,_startPoint.z), new Vector2(_targetPoint.x, _targetPoint.z));

        _throwItemInput.SwipeDone += OnSwipeEnded;
    }
    
    private void InitPoints()
    {
        _startPoint = _levelUtils.StartPosition;
        _targetPoint = _levelUtils.TargetPosition;
        _startScreenPoint = Camera.main.WorldToScreenPoint(_startPoint);
        _targetScreenPoint = Camera.main.WorldToScreenPoint(_targetPoint);
        _targetWorldDirection = new Vector2(_targetPoint.x - _startPoint.x, _targetPoint.z - _startPoint.z);
        _targetScreenDirection = _targetScreenPoint - _startScreenPoint;
        _targetAngleWithYAxe = Vector2.Angle(Vector2.up, _targetWorldDirection);
    }
    
    private void OnSwipeEnded(Vector2 swipeDirection)
    { 
        // _isLeftMoving = swipeDirection.x >= 0 && swipeDirection.y>= 0 || swipeDirection.x < 0 && swipeDirection.y < 0 ;
        _isClockWiseSwiping = swipeDirection.x >= 0;
        _angleDeviation = Vector2.Angle(_targetScreenDirection, swipeDirection);
        _angleDeviation = swipeDirection.y >= 0  ? _angleDeviation : 180 -_angleDeviation;
        
        var middleY = _thrownItem.IsGorizontalMoving ? _targetPoint.y + _thrownItem.PassOffsetY / 2 : _targetPoint.y + _thrownItem.HeightThrownIfVertical;
        var middleOfTargetVector = VectorUtils.GetRotatedVector(new Vector2(_startPoint.x, _startPoint.z), _middleVectorPoint, _angleDeviation, _isClockWiseSwiping);
        var middlePoint = new Vector3(middleOfTargetVector.x, middleY, middleOfTargetVector.y);
        
        if (IsPassed(_angleDeviation))
        {
            _itemMover.MovePass(middlePoint, _targetPoint, _angleDeviation + _targetAngleWithYAxe);
        }
        else
        {
            var targetFailY = _thrownItem.IsGorizontalMoving ? _targetPoint.y + _thrownItem.PassOffsetY : _targetPoint.y;
            var failTargetVector = 
                VectorUtils.GetRotatedVector(new Vector2(_startPoint.x, _startPoint.z), new Vector2(_targetPoint.x, _targetPoint.z), _angleDeviation, _isClockWiseSwiping);
            var failTargetPoint = new Vector3(failTargetVector.x, targetFailY, failTargetVector.y);

            _itemMover.MoveFail(middlePoint, failTargetPoint, _targetAngleWithYAxe, _angleDeviation, _isClockWiseSwiping);
        }
        
        _throwItemInput.SwipeDone -= OnSwipeEnded;
    }

    private bool IsPassed(float angleDeviation)
    {
        return angleDeviation <= _allowedAngleDeviation;
    }
}
