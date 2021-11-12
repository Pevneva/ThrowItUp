using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HitCastUtils
{
    public static Vector3 GetHitPoint(CastType castType, Collider collider, Vector3 startPoint, Vector3 direction, float offset)
    {
        var resultPoint = Vector3.zero;
        var boundsExtents = collider.bounds.extents;
        var halfExtents = new Vector3(boundsExtents.x, boundsExtents.y, boundsExtents.z);

        RaycastHit hitCast = new RaycastHit();

        if (castType == CastType.BOX)
            Physics.BoxCast(startPoint, halfExtents, direction, out hitCast, Quaternion.identity, 50);
        else if (castType == CastType.SPHERE)
            Physics.SphereCast(startPoint, halfExtents.y, direction, out hitCast, 50);

        if (hitCast.collider != null)
            resultPoint = VectorUtils.GetPointOnVectorByDistance(startPoint, direction, hitCast.distance - offset);

        return resultPoint;
    }
}

public enum CastType
{
    NONE = 0,
    BOX = 1,
    SPHERE = 2,
}