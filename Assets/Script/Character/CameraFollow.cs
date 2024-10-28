using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    void LateUpdate()
    {
        if(target != null){
            transform.position = target.position + offset;
            transform.rotation = Quaternion.Euler(0, 0, target.eulerAngles.z);;
        }
        
    }
}
