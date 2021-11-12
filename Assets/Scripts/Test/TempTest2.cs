using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTest2 : MonoBehaviour
{
    private Vector3 _colliderExtens;
    void Start()
    {
        // // var bounds1 = GetComponent<MeshFilter>().sharedMesh.bounds;
        // var bounds2 = GetComponentInChildren<Collider>().bounds;
        // // _colliderExtens = GetComponentInChildren<Collider>().bounds.extents;
        //
        // Debug.Log("bounds collider: " + bounds2);
        // Debug.Log("GameObject : " + gameObject.name + "; z: " + bounds2.extents.z);

        GetFirstColliderPoint();
    }

    private void Update()
    {
        // GetFirstColliderPoint();
    }

    private void GetFirstColliderPoint()
    {
        var collider = GetComponent<Collider>();
        
        RaycastHit hitBoxcast = new RaycastHit();

        Physics.BoxCast(collider.bounds.center, transform.localScale, transform.forward, out hitBoxcast, transform.rotation, 5);

        if (hitBoxcast.collider != null)
        {
            Debug.Log("transform.localScale :" + transform.localScale);
            Debug.Log("collider.bounds.extents :" + collider.bounds.extents);
            Debug.Log("hitBoxcast.collider :" + hitBoxcast.collider);
            Debug.Log("hitBoxcast.collider :" + hitBoxcast.transform.position);
            // GameObject hitObject = new GameObject();
            // hitObject.transform.position = hitBoxcast.transform.position;
        }
        else
        {
            Debug.Log("NO HITS !!!");
        }

        // resultPoint = VectorUtils.GetPointOnVectorByDistance(startRay, endVector, hit2.distance - offset); 
        
        
        // Vector3 resultPoint = new Vector3();
        // Vector3 startRay = new Vector3(0,0,0);
        // Vector3 endRay = new Vector3(1,1,1);
        //
        // var endVector = new Vector3(endRay.x, 0, endRay.z);
        //
        // if (collider != null)
        // {
        //     _colliderExtens = collider.bounds.extents;
        //     float offset = _colliderExtens.y + _colliderExtens.z;
        //     
        //     RaycastHit hitBoxcast = new RaycastHit();
        //
        //     if (GetComponent<BoxCollider>() != null)
        //     {
        //         Physics.BoxCast(collider.bounds.center, transform.localScale, endVector, out hitBoxcast, transform.rotation, 100);
        //     
        //         if (hitBoxcast.collider != null)
        //             resultPoint = VectorUtils.GetPointOnVectorByDistance(startRay, endVector, hitBoxcast.distance - offset);
        //     } else if (GetComponent<SphereCollider>() != null)
        //     {
        //         Physics.SphereCast(collider.bounds.center, _colliderExtens.y, endVector, out hitBoxcast, 100);
        //     
        //         if (hitBoxcast.collider != null)
        //             resultPoint = VectorUtils.GetPointOnVectorByDistance(startRay, endVector, hitBoxcast.distance - offset);
        //     }
        //
        //     Debug.Log(" resultPoint : " + resultPoint);
        

        
        // else if (_throwItem.GetComponent<BoxCollider>() != null)
        // {
        //     var collider = GetComponent<BoxCollider>();
        //     RaycastHit hitBoxcast = new RaycastHit();
        //     Physics.BoxCast(collider.bounds.center, transform.localScale, transform.forward, out hitBoxcast, transform.rotation, 100);
        //     
        //     if (hitBoxcast.collider != null)
        //         resultPoint = VectorUtils.GetPointOnVectorByDistance(startRay, endVector, hit2.distance - offset);
        // }

        // Debug.Log("pointBeforeCollider : " + pointBeforeCollider);
    }

}
