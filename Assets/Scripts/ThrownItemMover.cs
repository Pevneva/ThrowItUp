using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class ThrownItemMover : MonoBehaviour
{
    private ThrowItem _throwItem;
    private Rigidbody _rigidbody;
    private Ray _ray;
    private Vector3 _failTargetVector;
    private float _duration;
    private bool _isFailRotation;
    
    public event UnityAction LevelPassed; 
    public event UnityAction LevelFailed;

    private void Start()
    {
        _throwItem = GetComponent<ThrowItem>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _duration = 0.5f;
        _rigidbody.mass = 5;
    }

    public void MovePass(Vector3 middlePoint, Vector3 targetPoint, float angle)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(middlePoint, _duration).SetEase(Ease.Linear));
        
        seq.Append(transform.DOMove(new Vector3(targetPoint.x, targetPoint.y + _throwItem.PassOffsetY, targetPoint.z), _duration).SetEase(Ease.Linear).OnComplete(() => DoWin())); 
        
        if (_throwItem.IsRotate)
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x,angle, transform.localRotation.eulerAngles.z); 
            seq.Insert(0, transform.DOLocalRotate(new Vector3(-180, angle, 0), _duration/5f).SetLoops(9, LoopType.Incremental).SetEase(Ease.Linear)); 
        }
    }

    private Vector3 GetFirstColliderPoint(Vector3 startRay, Vector3 endRay, float offset)
    {
        var resultPoint = Vector3.zero;
        
        _ray = new Ray(startRay, endRay);
        Physics.Raycast(_ray, out RaycastHit hit);
        
        if (hit.collider != null)
            resultPoint = VectorUtils.GetPointOnVectorByDistance(startRay, endRay, hit.distance - offset);

        return resultPoint;
    }
    
    public void MoveFail(Vector3 middlePoint, Vector3 targetPoint, float targetAngle, float swipeAngle, bool isRightMoving )
    {
        _failTargetVector = targetPoint - middlePoint;
        var angle = isRightMoving  ? targetAngle + swipeAngle : targetAngle - swipeAngle;
        var resultPoint = GetFirstColliderPoint(middlePoint, _failTargetVector, _throwItem.FailOffsetY);
                
        Debug.Log("resultPoint : " + resultPoint);
        
        if (resultPoint == Vector3.zero)
            resultPoint = targetPoint;          
        
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(middlePoint, _duration).SetEase(Ease.Linear));
        seq.Append(transform.DOMove(resultPoint, _duration).SetEase(Ease.Linear).OnComplete(DoFail));
        
        if (_throwItem.IsRotate)
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x,angle, transform.localRotation.eulerAngles.z); //to do angle
            seq.Insert(0, transform.DOLocalRotate(new Vector3(-180, angle, 0), _duration/5).SetLoops(9, LoopType.Incremental).SetEase(Ease.Linear));
        }
        
        seq.OnComplete(() =>
        {
            _rigidbody.isKinematic = false;
            Physics.gravity = new Vector3(0, -23F, 0);
            
            var koef = 1;
            Debug.Log("seq - OnComplete");
            
            if (_throwItem.IsRotate)
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