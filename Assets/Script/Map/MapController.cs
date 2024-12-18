using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapController : MonoBehaviour
{
    List<PlatformEffector2D> floors;
    void Start()
    {
        floors = new List<PlatformEffector2D>();
        foreach (Transform child in transform)
        {
            if (child.name == "Ground")
            {
                PlatformEffector2D effector = child.GetComponent<PlatformEffector2D>();
                if (effector != null)
                {
                    floors.Add(effector);
                }
            }
        }
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
