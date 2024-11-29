using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryController : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float speed;

    /******************************************/
    List<GameObject> child;

    void Start()
    {
        child = GetAllChildGameObjects(gameObject);

    }
    void Update()
    {

        transform.Rotate(movementVector * Time.time * speed);

        foreach (GameObject childObject in child)
        {
            childObject.transform.Rotate(movementVector * Time.time * speed);
        }

    }

    List<GameObject> GetAllChildGameObjects(GameObject parent){
        List<GameObject> childObjects = new List<GameObject>();
        Transform[] children = parent.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if(child != parent.transform){
                childObjects.Add(child.gameObject);
            }
        }
        return childObjects;
    }
}
