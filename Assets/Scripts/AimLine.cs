using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    LineRenderer lineRenderer;

    private void Awake() 
    {
        lineRenderer = GetComponent<LineRenderer>();    
    }

    private void Start() 
    {
        lineRenderer.positionCount = 2;    
    }

    public void DrawAimLine(Vector2 origin, Vector2 target)
    {
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, target);
    }
}
