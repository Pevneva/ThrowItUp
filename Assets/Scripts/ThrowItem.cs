using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]
public class ThrowItem : MonoBehaviour
{
    [SerializeField] private bool _isRotate;
    [SerializeField] private float _passOffsetY;
    [SerializeField] private float _failOffsetY;
    [SerializeField] private bool _isGorizontalMoving;

    public float PassOffsetY => _passOffsetY;
    public float FailOffsetY => _failOffsetY;
    public bool IsGorizontalMoving => _isGorizontalMoving;
    public bool IsRotate => _isRotate;
}
