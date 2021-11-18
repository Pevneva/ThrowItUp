using UnityEngine;
using UnityEngine.Events;

public class ThrownItemInput : MonoBehaviour
{
    private Vector2 _startPos;
    private Vector2 _direction;
    private LineRenderer _line;

    public event UnityAction<Vector2> SwipeDone;

    private void Start()
    {
        _line = FindObjectOfType<LineRenderer>();
    }

    private void Update()
    {
        TryDoSwipe();
    }
    
    private void TryDoSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RemoveLine();
            _startPos = Input.mousePosition;
        } 
        else if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePosition = Input.mousePosition;
            if (Vector2.Distance(_startPos, currentMousePosition) > 25f)
            {
                Camera cam = Camera.main;

                var screenPos = new Vector3(currentMousePosition.x, currentMousePosition.y, cam.nearClipPlane + 1);
                var worldPos = cam.ScreenToWorldPoint(screenPos);
                
                AddPointToLine(worldPos);
            }

        } 
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePoint = Input.mousePosition;
            _direction = mousePoint - _startPos;

            if (Vector2.Distance(_startPos, mousePoint) > 25f)
                SwipeDone?.Invoke(_direction.normalized * 10);
            
            Invoke(nameof(RemoveLine), 0.5f);
        }        
    }
    
    private void RemoveLine()
    {
        _line.positionCount = 0;
    }

    private void AddPointToLine(Vector3 point)
    {
        _line.positionCount ++;
        _line.SetPosition(_line.positionCount - 1, point);          
    }
}
