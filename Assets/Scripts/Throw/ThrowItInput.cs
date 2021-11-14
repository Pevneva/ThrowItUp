using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ThrowItInput : MonoBehaviour
{
    private Vector2 _startPos;
    private Vector2 _direction;
    private bool _isDirectionChosen;
    private LineRenderer _line;
    
    public Vector2 SwipeDirection { get; private set; }
    public Vector2 SwipeStartPoint { get; private set; }
    public Vector2 SwipeEndPoint { get; private set; }
    public event UnityAction<Vector2> SwipeDone;

    private void Start()
    {
        _line = FindObjectOfType<LineRenderer>();
    }

    private void Update()
    {
        CheckClicking();
    }
    
    private void CheckClicking()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RemoveLine();
            Vector2 mousePoint = Input.mousePosition;
            _startPos = mousePoint;
            Debug.Log("ZZZAAA _startPos : " + _startPos);
        } 
        else if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Input.mousePosition;
            // Debug.Log("ZZZ Distance(_startPos, mousePos) :" + Vector2.Distance(_startPos, mousePos));
            // Debug.Log("ZZZ _startPos :" + _startPos);
            // Debug.Log("ZZZ mousePos :" + mousePos);
            if (Vector2.Distance(_startPos, mousePos) > 25f)
            {
                Camera cam = Camera.main;
                
                mousePos.x = Input.mousePosition.x;
                mousePos.y = Input.mousePosition.y;
                
                var screenPos = new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane + 1);
                var worldPos = cam.ScreenToWorldPoint(screenPos);
                
                _line.positionCount ++;
                _line.SetPosition(_line.positionCount-1, worldPos);                
            }

        } 
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePoint = Input.mousePosition;
            _direction = mousePoint - _startPos;
            
            Debug.Log("ZZZ Distance(_startPos, mousePos) :" + Vector2.Distance(_startPos, mousePoint));
            // Debug.Log("AAA _endPos : " + mousePoint);
            // Debug.Log("AAA _direction : " + _direction);
            //
            if (Vector2.Distance(_startPos, mousePoint)>25f)
                SwipeDone?.Invoke(_direction.normalized * 10);
            
            Invoke(nameof(RemoveLine), 0.5f);
        }        
    }
    
    private void RemoveLine()
    {
        _line.positionCount = 0;
    }
}
