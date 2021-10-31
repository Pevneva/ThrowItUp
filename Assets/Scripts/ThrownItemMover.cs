using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class ThrownItemMover : MonoBehaviour
{
    [SerializeField] private float _speedHorizontalRotation;
    [SerializeField] private float _speedVerticalRotation;
    [SerializeField] private float _speedMoving;
    
    private ThrowItem _throwItem;
    private Rigidbody _rigidbody;
    private Ray _ray;
    private Vector3 _failTargetVector;
    private float _duration;
    private bool _isFailRotation;
    private Vector3 _startPoint;

    public event UnityAction LevelPassed; 
    public event UnityAction LevelFailed;

    private void Start()
    {
        _throwItem = GetComponent<ThrowItem>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _duration = 0.5f;
        _rigidbody.mass = 5;
        _speedMoving = 200;
    }

    public void MovePass(Vector3 middlePoint, Vector3 targetPoint, float angle)
    {
        Sequence seq = DOTween.Sequence();
        var duration = Vector3.Distance(transform.position, middlePoint) / _speedMoving;
        var startQuaternion = transform.rotation;
        seq.Append(transform.DOMove(middlePoint, duration).SetEase(Ease.Linear));
        seq.Append(transform.DOMove(new Vector3(targetPoint.x, targetPoint.y + _throwItem.PassOffsetY, targetPoint.z), duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => DoWin())); 
        
        if (_throwItem.IsVerticalRotate)
        {
            var countRotations = Mathf.FloorToInt(duration * _speedVerticalRotation);
            var tempDuration = 2 * duration / (countRotations * 2 + 1);
            Debug.Log(" Loops: " + countRotations);
            
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x,angle, transform.localRotation.eulerAngles.z); 
            seq.Insert(0, transform
                .DOLocalRotate(new Vector3(-180, angle, 0), tempDuration)
                // .DOLocalRotate(new Vector3(-180, angle, 0), (float) 1 / countRotations)
                .SetLoops(countRotations * 2 + 1, LoopType.Incremental)
                .SetEase(Ease.Linear)); 
        }

        if (_throwItem.IsGorizontalRotate)// && _throwItem.IsNeedRotateToPass == false)
        {
            var countRotations = Mathf.FloorToInt(duration * _speedHorizontalRotation);
            Debug.Log(" Loops: " + countRotations);
            
            seq.Insert(0, transform
                .DOLocalRotate(new Vector3(0, -180, 0), 1/_speedHorizontalRotation)
                .SetLoops(countRotations * 2 - 1, LoopType.Incremental)
                .SetEase(Ease.Linear));
        }

        if (_throwItem.IsNeedRotateToPass)
        {
            Debug.Log("AAA BBB CCC");
            var q1 = Quaternion.Euler(0, 0, -90);
            var q2 = Quaternion.Euler(0, -180, 0);
            transform.DOLocalRotateQuaternion(startQuaternion * q1 * q2, duration * 2);
            
            // seq.Insert(0, transform
            //     .DOLocalRotateQuaternion(startQuaternion * q1 * q2, duration * 2)
            //     .SetEase(Ease.Linear));     
            
            
            // seq.Insert(0, transform
            //     .DOLocalRotate(new Vector3(0, 0, -90), duration * 2)
            //     .SetEase(Ease.Linear));            
        }
    }

    private void FixedUpdate()
    {
        // var q1 = Quaternion.Euler(0, 0, -90);
        // var q2 = Quaternion.Euler(0, -180, 0);
    }

    private Vector3 GetFirstColliderPoint(Vector3 startRay, Vector3 endRay, float offset)
    {
        var resultPoint = Vector3.zero;
        
        _ray = new Ray(startRay, endRay);
        Physics.Raycast(_ray, out RaycastHit hit);
        
        if (hit.collider != null)
        {
            resultPoint = VectorUtils.GetPointOnVectorByDistance(startRay, endRay, hit.distance - offset);
        }
        else if (_throwItem.IsGorizontalMoving)
        {
            var endVector = new Vector3(endRay.x, 0, endRay.z);
            
            _ray = new Ray(startRay, endVector);
            Physics.Raycast(_ray, out RaycastHit hit2);
            
            if (hit2.collider != null)
                resultPoint = VectorUtils.GetPointOnVectorByDistance(startRay, endVector, hit2.distance - offset);
        }
        return resultPoint;
    }
    
    public void MoveFail(Vector3 middlePoint, Vector3 targetPoint, float targetAngle, float swipeAngle, bool isRightMoving )
    {
        _startPoint = transform.position;
        _failTargetVector = targetPoint - middlePoint;
        var angle = isRightMoving  ? targetAngle + swipeAngle : targetAngle - swipeAngle;
        var resultPoint = GetFirstColliderPoint(middlePoint, _failTargetVector, _throwItem.FailOffset);
                
        Debug.Log("AAA _failTargetVector : " + _failTargetVector);
        Debug.Log("AAA middlePoint : " + middlePoint);
        Debug.Log("AAA resultPoint : " + resultPoint);
        Debug.Log("AAA targetPoint : " + targetPoint);
        
        if (resultPoint == Vector3.zero && _throwItem.IsGorizontalMoving == false)
            resultPoint = targetPoint;  
        
        else if (resultPoint == Vector3.zero && _throwItem.IsGorizontalMoving)
            resultPoint = VectorUtils.GetPointOnVectorByDistance(middlePoint, new Vector3(_failTargetVector.x, 0f, _failTargetVector.z), 1000);    
        
        Invoke(nameof(DoFail), _duration * 2 + 0.1f);
        
        Sequence seq = DOTween.Sequence();
        var duration_1 = Vector3.Distance(transform.position, middlePoint) / _speedMoving;
        var duration_2 = Vector3.Distance(resultPoint, middlePoint) / _speedMoving;
        seq.Append(transform.DOMove(middlePoint, duration_1).SetEase(Ease.Linear));
        seq.Append(transform.DOMove(resultPoint, duration_2).SetEase(Ease.Linear));

        if (_throwItem.IsVerticalRotate)
        {
            var countRotations = Mathf.FloorToInt((duration_1 + duration_2) * _speedVerticalRotation);
            Debug.Log(" Loops: " + countRotations);
            
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x,angle, transform.localRotation.eulerAngles.z); //to do angle
            seq.Insert(0, transform
                .DOLocalRotate(new Vector3(-180, angle, 0), (float) 1 / countRotations) //to do
                .SetLoops(countRotations, LoopType.Incremental)// to do
                .SetEase(Ease.Linear));
            // seq.Insert(0, transform
            //     .DOLocalRotate(new Vector3(-180, angle, 0), _duration/5) //to do
            //     .SetLoops(9, LoopType.Incremental)// to do
            //     .SetEase(Ease.Linear));
        }

        if (_throwItem.IsGorizontalRotate)
        {
            var countRotations = Mathf.FloorToInt((duration_1 + duration_2) * _speedHorizontalRotation);
            Debug.Log(" Loops: " + countRotations);
            
            seq.Insert(0, transform
                .DOLocalRotate(new Vector3(0, -180, 0), 1/_speedHorizontalRotation)
                .SetLoops(countRotations + 1, LoopType.Incremental)
                .SetEase(Ease.Linear));
        }
        
        
        seq.OnComplete(() =>
        {
            _rigidbody.isKinematic = false;
            Physics.gravity = new Vector3(0, -23F, 0);
            
            var koef = 1;
            Debug.Log("seq - OnComplete");
            
            if (_throwItem.IsVerticalRotate)
                _rigidbody.AddRelativeTorque( new Vector3(-1, 0,0 ) * 1500, ForceMode.Impulse);

            if (_throwItem.IsGorizontalMoving)
                koef = 2;
            
            var forceVector = new Vector3(_failTargetVector.x, 0, _failTargetVector.z);
            _rigidbody.AddForce(forceVector.normalized * 15000 * koef);
        });
        
        Destroy(gameObject,10);
    }

    private void DoWin()
    {
        Debug.Log("WIN");
        LevelPassed?.Invoke();
    }    
    
    private void DoFail()
    {
        Debug.Log("FAIL");
        LevelFailed?.Invoke();
    }
}