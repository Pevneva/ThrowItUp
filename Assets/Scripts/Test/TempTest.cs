using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TempTest : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _targetMoveVector;
    [SerializeField] private float _angle;

    private ThrownItem _thrownItem;
    private Rigidbody _rigidbody;
    private Vector3 _eulerAngleVelocity;
    private Vector3 _angleVector;
    private bool _isStoppingRotate;
    public Quaternion _startQuaternion;
    public Quaternion _quaternion1;
    public Quaternion _quaternion2;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        var start = new Vector3(2, 2, 2);
        var distance = 5;
        var vector = new Vector3(0, 0, 1);
        
        Debug.Log(" VECTOR : " + GetPointOnVectorByDistance(start, vector, distance));
        
        // transform.rotation = _quaternion;
        _startQuaternion = transform.rotation;
        _quaternion1 = Quaternion.Euler(0, -180, 0);
        _quaternion2 = Quaternion.Euler(0, 0, 90);
        transform.DOLocalRotateQuaternion(_startQuaternion * _quaternion2 * _quaternion1, 2);


        //Set the angular velocity of the Rigidbody (rotating around the Y axis, 100 deg/sec)
        // _eulerAngleVelocity = new Vector3(0, 0, 5);
        // _angleVector = new Vector3(-10000, 0, 0);
        // Debug.Log("Time.fixedDeltaTime : " + Time.fixedDeltaTime);


        // transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,45, transform.rotation.eulerAngles.z);
        // transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x,10, transform.localRotation.eulerAngles.z);
        //
        // Sequence seq = DOTween.Sequence();
        // seq.Append(transform.DOLocalMove(new Vector3(0, 0, 0), 0));
        // seq.Insert(0, transform.DOLocalRotate(new Vector3(0,10,-180), 0.5f).SetEase(Ease.Linear).SetLoops(90, LoopType.Incremental));
        // Sequence seq = DOTween.Sequence();
        // seq.Append(transform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(0,0,-180)), 2).SetLoops(-1, LoopType.Incremental));
        // seq.Append(transform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(0,0,-180)), 2).SetLoops(-1, LoopType.Incremental));

        // Ray ray = new Ray(new Vector3(-56.8f, 149.7f, 121.1f), Vector3.down * 100);
        // Physics.Raycast(ray, out RaycastHit hit);
        // Debug.Log(" hit : " + hit.collider.gameObject.name);
    }
    
    //Physics.gravity = new Vector3(0, 9.81F, 0);
    
    
    private void FixedUpdate()
    {

        transform.rotation = _startQuaternion * _quaternion1 * _quaternion2;


    }
    
    private Vector3 GetPointOnVectorByDistance(Vector3 startPoint, Vector3 vector, float distance)
    {
        var x = distance * vector.x / vector.magnitude + startPoint.x;
        var y = distance * vector.y / vector.magnitude + startPoint.y;
        var z = distance * vector.z / vector.magnitude + startPoint.z;
        return new Vector3(x, y, z);
    }

    private void OnTriggerEnter(Collider other)
    {
        throw new NotImplementedException();
    }

    private void FixedUpdate2()
    {
        // _rigidbody.AddRelativeTorque( _angleVector * Time.fixedDeltaTime, ForceMode.Impulse);
        // if (_isStoppingRotate)
        //     _angleVector /= 1.2f;
        //
        // Debug.Log("_angleVector : " + _angleVector);
        
        
        // Quaternion deltaRotation = Quaternion.Euler(_eulerAngleVelocity * Time.fixedDeltaTime);
        // _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);

    }

    private void Update()
    {
        // transform.rotation = Quaternion.Euler(0,0,-180);
        // transform.Rotate(_eulerAngleVelocity);
    }

    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log("Collision");
        // if (other.gameObject.TryGetComponent(out Ground ground))
        // {
        //     Debug.Log("Ground");
        //     // _angleVector = Vector3.zero;
        //     _isStoppingRotate = true;
        //     Debug.Log("_angleVector : " + _angleVector);
        // }
    }


    // public float koef = 10f;
    // private Vector3 oldPos;
    // private Vector3 newPos;
    // private bool speedOrder;
    // private float moveSpeed;
    //    
    // void FixedUpdate () {
    //     if (speedOrder) {
    //         newPos = transform.position;
    //         moveSpeed = Vector3.Distance(oldPos, newPos) * koef;
    //     } else
    //         oldPos = transform.position;
    //     speedOrder = !speedOrder;          
    // }

}
