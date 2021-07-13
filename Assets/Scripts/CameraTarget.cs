using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform player;
    [SerializeField] float threshold;

    Vector2 centerPos;

    private void Start() 
    {
        centerPos = transform.position;
    }

    private void Update() 
    {
        Vector2 targetPos = ((Vector2)player.position + centerPos) / 2f;

        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + centerPos.x, threshold + centerPos.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + centerPos.y, threshold + centerPos.y);

        transform.position = targetPos;
    }
}
