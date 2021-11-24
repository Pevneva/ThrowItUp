using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(ThrownItem), typeof(Rigidbody))]
public class ThrownItemMover : MonoBehaviour
{
    [SerializeField] private float _speedHorizontalRotation;
    [SerializeField] private float _speedVerticalRotation;
    [SerializeField] private float _speedMoving;
    
    private ThrownItem _thrownItem;
    private Rigidbody _rigidbody;
    private CameraMoving _cameraMoving;
    private Ray _ray;
    private Vector3 _failTargetVector;
    private float _duration;
    private float _duration2;
    private bool _isFailRotation;
    private Vector3 _startPoint;
    private Quaternion _startQuaternion;
    private bool _isDoubleRotations;
    private float  _angle1;
    private float _angle2;
    private int _countHorizontalRotationsInt;
    private float _countHorizontalRotationsFloat;
    private float _speedHorizontalRotation2;
    private int counter;
    private int _durationInFrames;
    private AudioPlayer _audio;

    public event UnityAction LevelPassed; 
    public event UnityAction LevelFailed;

    private void Start()
    {
        _thrownItem = GetComponent<ThrownItem>();
        _rigidbody = GetComponent<Rigidbody>();
        _cameraMoving = FindObjectOfType<CameraMoving>();
        _audio = FindObjectOfType<AudioPlayer>();
        _rigidbody.isKinematic = true;
        // _duration = 0.5f;
        _rigidbody.mass = 5;
        // _speedMoving = 10;
        // _speedHorizontalRotation = 0.03f;
    }

    public void MovePass(Vector3 middlePoint, Vector3 targetPoint, float angle)
    {
        Sequence seq = DOTween.Sequence();
        _duration = Vector3.Distance(transform.position, middlePoint) / _speedMoving;
        _startQuaternion = transform.rotation;

        var durationInFrames = _duration * 2 / Time.fixedDeltaTime;
        _durationInFrames = Mathf.FloorToInt(durationInFrames);

        Vector3 targetPointResult = new Vector3(targetPoint.x, targetPoint.y + _thrownItem.PassOffsetY, targetPoint.z);
        _cameraMoving.MoveIfWin(_duration * 2, targetPointResult);
        
        seq.Append(transform.DOMove(middlePoint, _duration).SetEase(Ease.Linear));
        seq.Append(transform.DOMove(targetPointResult, _duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => DoWin())); 
        
        if (_thrownItem.IsVerticalRotate)
        {
            var countRotations = Mathf.FloorToInt(_duration * _speedVerticalRotation);
            var tempDuration = 2 * _duration / (countRotations * 2 + 1);
            Debug.Log(" Loops: " + countRotations);
            
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x,angle, transform.localRotation.eulerAngles.z); 
            seq.Insert(0, transform
                .DOLocalRotate(new Vector3(-180, angle, 0), tempDuration)
                // .DOLocalRotate(new Vector3(-180, angle, 0), (float) 1 / countRotations)
                .SetLoops(countRotations * 2 + 1, LoopType.Incremental)
                .SetEase(Ease.Linear)); 
        }

        if (_thrownItem.IsGorizontalRotate && _thrownItem.IsNeedRotateToPass == false)
        {
            _countHorizontalRotationsInt = Mathf.FloorToInt(_duration / _speedHorizontalRotation);
           
            Debug.Log(" Loops: " + _countHorizontalRotationsInt);
            
            seq.Insert(0, transform
                .DOLocalRotate(new Vector3(0, -180, 0), _speedHorizontalRotation)
                .SetLoops(_countHorizontalRotationsInt * 2, LoopType.Incremental)
                .SetEase(Ease.Linear));
        }

        if (_thrownItem.IsGorizontalRotate && _thrownItem.IsNeedRotateToPass)
        {
            _countHorizontalRotationsInt = Mathf.FloorToInt(_duration * 2 / _speedHorizontalRotation);
            
            Debug.Log("AAA BBB CCC duration : " + (_duration * 2));
            Debug.Log("AAA BBB CCC durationInFrames FLOAT: " + durationInFrames);
            Debug.Log("AAA BBB CCC _durationInFrames INT : " + _durationInFrames);
            Debug.Log("AAA BBB CCC  _countHorizontalRotationsInt : " +  _countHorizontalRotationsInt);
            Debug.Log("AAA BBB CCC  180 * _countHorizontalRotationsInt : " +  (180 * _countHorizontalRotationsInt));
            Debug.Log("AAA BBB CCC  RESULT : " +  ( 180 * _countHorizontalRotationsInt)/_durationInFrames);
           _isDoubleRotations = true;
        }
    }

    private void FixedUpdate()
    {
        if (_isDoubleRotations && _angle1 < 180 * _countHorizontalRotationsInt)
        {
            Debug.Log("_angle1 : " + _angle1);
            counter++;
            _angle1 +=  (float)(180 * _countHorizontalRotationsInt)/(_durationInFrames);
            _angle2 += Time.fixedDeltaTime * 90 / (_duration * 2);
            // _angle2 += Time.fixedDeltaTime * 90 / (_duration * 2);
            Quaternion q1 = Quaternion.AngleAxis(_angle1, Vector3.up);
            Quaternion q2 = Quaternion.AngleAxis(_angle2, Vector3.forward);
            transform.rotation = _startQuaternion * q2 * q1;
        }
    }

    private Vector3 GetFirstColliderPoint(Vector3 startPoint, Vector3 direction, float offset)
    {
        var resultPoint = Vector3.zero;
        var collider = GetComponent<Collider>();

        if (_thrownItem.IsGorizontalMoving)
            direction = new Vector3(direction.x, 0, direction.z);

        if (collider != null)
        {
            CastType castType;
            
            if (GetComponent<BoxCollider>() != null)
                castType = CastType.BOX;
            else if (GetComponent<SphereCollider>() != null)
                castType = CastType.SPHERE;
            else 
                castType = CastType.NONE;

            resultPoint = HitCastUtils.GetHitPoint(castType, collider, startPoint, direction, offset);
            Debug.Log(" TRUE resultPoint : " + resultPoint);
        }
        
        return resultPoint;
    }
    
    public void MoveFail(Vector3 middlePoint, Vector3 targetPoint, float targetAngle, float swipeAngle, bool isRightMoving )
    {
        _startPoint = transform.position;
        _failTargetVector = targetPoint - middlePoint;
        var angle = isRightMoving  ? targetAngle + swipeAngle : targetAngle - swipeAngle;
        var resultPoint = GetFirstColliderPoint(middlePoint, _failTargetVector, _thrownItem.FailOffset);
                
        Debug.Log("AAA _failTargetVector : " + _failTargetVector);
        Debug.Log("AAA _thrownItem.FailOffset : " + _thrownItem.FailOffset);
        Debug.Log("AAA middlePoint : " + middlePoint);
        Debug.Log("AAA resultPoint : " + resultPoint);
        Debug.Log("AAA targetPoint : " + targetPoint);
        
        if (resultPoint == Vector3.zero && _thrownItem.IsGorizontalMoving == false)
            resultPoint = targetPoint;  
        
        else if (resultPoint == Vector3.zero && _thrownItem.IsGorizontalMoving)
            resultPoint = VectorUtils.GetPointOnVectorByDistance(middlePoint, new Vector3(_failTargetVector.x, 0f, _failTargetVector.z), 1000);
        
        var duration_1 = Vector3.Distance(transform.position, middlePoint) / _speedMoving;
        var duration_2 = Vector3.Distance(resultPoint, middlePoint) / _speedMoving;
        _duration = duration_1 + duration_2;
        
        Invoke(nameof(DoFail), duration_1 * 2 + 0.1f);
        
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMove(middlePoint, duration_1).SetEase(Ease.Linear));
        seq.Append(transform.DOMove(resultPoint, duration_2).SetEase(Ease.Linear));

        if (_thrownItem.IsVerticalRotate)
        {
            var countRotations = Mathf.FloorToInt(_duration * _speedVerticalRotation);
            Debug.Log(" Loops: " + countRotations);
            
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x,angle, transform.localRotation.eulerAngles.z); //to do angle
            seq.Insert(0, transform
                .DOLocalRotate(new Vector3(-180, angle, 0), (float) 1 / countRotations) //to do
                .SetLoops(countRotations, LoopType.Incremental)// to do
                .SetEase(Ease.Linear));
        }

        if (_thrownItem.IsGorizontalRotate)
        {
            var countRotations = Mathf.FloorToInt(_duration / _speedHorizontalRotation);
            Debug.Log(" Loops: " + countRotations);
            
            seq.Insert(0, transform
                .DOLocalRotate(new Vector3(0, -180, 0), _speedHorizontalRotation)
                .SetLoops(countRotations + 1, LoopType.Incremental)
                .SetEase(Ease.Linear));
        }
        
        seq.OnComplete(() =>
        {
            _audio.PlayHit();
            _rigidbody.isKinematic = false;
            Physics.gravity = new Vector3(0, -23F, 0);
            
            var koef = 1;
            
            if (_thrownItem.IsVerticalRotate)
                _rigidbody.AddRelativeTorque( new Vector3(-1, 0,0 ) * 2, ForceMode.Impulse);

            if (_thrownItem.IsGorizontalMoving)
                koef = 2;
            
            var forceVector = new Vector3(_failTargetVector.x, 0, _failTargetVector.z);
            Debug.Log("TRUE AddForce : " + forceVector.normalized * 150 * koef);
            Debug.Log("TRUE AddRelativeTorque : " + new Vector3(-1, 0,0 ) * 2);
            _rigidbody.AddForce(forceVector.normalized * 150 * koef);
        });
        
        Destroy(gameObject,10);
    }

    private void DoWin()
    {
        Debug.Log("WIN _angle1 : " + _angle1);
        Debug.Log("WIN counter : " + counter);
        _isDoubleRotations = false;
        _angle1 = 0;
        LevelPassed?.Invoke();
    }    
    
    private void DoFail()
    {
        Debug.Log("FAIL");
        LevelFailed?.Invoke();
    }
}