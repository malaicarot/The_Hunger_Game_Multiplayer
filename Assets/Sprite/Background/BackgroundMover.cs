using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
   [SerializeField] Renderer bckRenderer;
   [SerializeField] float speed = 0.3f;

    void Update()
    {
        bckRenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0f);
    }
}
