using UnityEngine;
using UnityEngine.Serialization;

public class ThrownItem : MonoBehaviour
{
    [SerializeField] private bool _isVerticalRotate;
    [SerializeField] private bool _isGorizontalRotate;
    [SerializeField] private float _passOffsetY;
    [SerializeField] private float _failOffset;
    [SerializeField] private bool _isGorizontalMoving;
    [SerializeField] private float _heightThrownIfVertical = 100;
    [SerializeField] private bool _isNeedRotateToPass = false;

    public float PassOffsetY => _passOffsetY;
    public float FailOffset => _failOffset;
    public bool IsGorizontalMoving => _isGorizontalMoving;
    public bool IsVerticalRotate => _isVerticalRotate;
    public bool IsGorizontalRotate => _isGorizontalRotate;
    public bool IsNeedRotateToPass => _isNeedRotateToPass;
    public float HeightThrownIfVertical => _heightThrownIfVertical;
}
