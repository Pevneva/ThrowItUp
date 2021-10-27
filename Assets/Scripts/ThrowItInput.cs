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
    
    public Vector2 SwipeDirection { get; private set; }
    public Vector2 SwipeStartPoint { get; private set; }
    public Vector2 SwipeEndPoint { get; private set; }
    public event UnityAction<Vector2> SwipeDone;
    
    private void Update()
    {
        // Track a single touch as a direction control.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on touch phase.
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    Debug.Log("INPUT Began : ");
                    _startPos = touch.position;
                    _isDirectionChosen = false;
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    // _direction = touch.position - _startPos;
                    Debug.Log("INPUT _direction : " + _direction);
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    Debug.Log("INPUT Ended : ");
                    _isDirectionChosen = true;
                    break;
            }
        }
        if (_isDirectionChosen)
        {
            // Something that uses the chosen direction...
            
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePoint = Input.mousePosition;

            _startPos = mousePoint;
            // SwipeStartPoint = mousePoint;

        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePoint = Input.mousePosition;
            _direction = mousePoint - _startPos;
            
            SwipeDone?.Invoke(_direction.normalized * 10);
        }
    }
}
