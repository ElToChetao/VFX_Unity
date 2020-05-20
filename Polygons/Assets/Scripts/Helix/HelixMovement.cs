using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixMovement : MonoBehaviour
{
    public Transform cam;
    public Transform fx;

    void Update()
    {
        float currentTime = Time.time;
        float dt = Time.deltaTime;

        float radius = Mathf.PerlinNoise(currentTime * 0.3f, 0) * 6f + 0.5f;
        Vector3 newPos;
        newPos.x = radius * Mathf.Cos(currentTime);
        newPos.y = transform.position.y + dt * 10f;
        newPos.z = radius * Mathf.Sin(currentTime);

        transform.Rotate(Vector3.up * dt * 100);
        transform.position = newPos;

        fx.position = new Vector3(fx.position.x, transform.position.y + 15, fx.position.z);
    }
}
