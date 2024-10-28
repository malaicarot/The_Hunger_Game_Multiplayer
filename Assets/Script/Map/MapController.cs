using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapController : MonoBehaviour
{
    // List<PlatformEffector2D> floors;
    PlatformEffector2D[] floors;
    void Start()
    {
        List<PlatformEffector2D> foundFloor = new List<PlatformEffector2D>();
        foreach (Transform child in transform)
        {
            if (child.name == "Floor")
            {
                Debug.Log(child);
                PlatformEffector2D effector = child.GetComponent<PlatformEffector2D>();
                if (effector != null)
                {
                    Debug.Log(effector);
                    foundFloor.Add(effector);
                }
            }
        }
        floors = foundFloor.ToArray();
    }

    IEnumerator ResetSurfaceArc()
    {
        yield return new WaitForSeconds(0.2f);
        foreach (var floor in floors)
        {
            floor.surfaceArc = 180;
        }

    }

    public void ActiveResetSurfaceArc()
    {
        StartCoroutine(ResetSurfaceArc());
    }

    public void SettingSurfaceArc()
    {
        foreach (var floor in floors)
        {
            floor.surfaceArc *= -1;
        }

    }


}
