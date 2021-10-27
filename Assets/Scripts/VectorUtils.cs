using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtils
{
    public static Vector3 GetPointOnVectorByDistance(Vector3 startPoint, Vector3 vector, float distance)
    {
        var x = distance * vector.x / vector.magnitude + startPoint.x;
        var y = distance * vector.y / vector.magnitude + startPoint.y;
        var z = distance * vector.z / vector.magnitude + startPoint.z;
        return new Vector3(x, y, z);
    }
    
    public static Vector2 GetMiddleVectorPoint(Vector2 startPoint, Vector2 endPoint)
    {
        return new Vector2((endPoint.x  + startPoint.x)/2, (endPoint.y + startPoint.y)/2);
    }
    
    public static Vector2 GetRotatedVector(Vector2 startPoint, Vector2 endPoint, float angle, bool isClockWise)
    {
        var startVector = endPoint - startPoint;
        var angleRad = angle * Mathf.Deg2Rad;
        
        Debug.Log("   ==== startVector : " + startVector);
        float targetX;
        float targetY;
        Vector2 targetVector;

        if (isClockWise == false)
        {
            targetX = startVector.x * Mathf.Cos(angleRad) - startVector.y * Mathf.Sin(angleRad);
            targetY = + startVector.x * Mathf.Sin(angleRad) + startVector.y * Mathf.Cos(angleRad);  
            targetVector = new Vector2(targetX + startPoint.x, targetY + startPoint.y);
            Debug.Log("   ==== targetVector 1 : " + targetVector);
        }
        else
        {
            targetX = startVector.x * Mathf.Cos(angleRad) + startVector.y * Mathf.Sin(angleRad);
            targetY = - startVector.x * Mathf.Sin(angleRad) + startVector.y * Mathf.Cos(angleRad);
            targetVector = new Vector2(targetX + startPoint.x, targetY + startPoint.y);
            Debug.Log("   ==== targetVector 2 : " + targetVector);           
        }

        return targetVector;
    }
}
