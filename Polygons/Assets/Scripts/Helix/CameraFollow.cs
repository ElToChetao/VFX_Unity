using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float springK = 10.0f;
    public float damping = 5.0f;
    private float vel = 0.0f;
    private float distance;
    public float maxDistance;
    public float heightOffset = 3.0f;
    void Update()
    {
        float dt = Time.deltaTime;
        Vector3 newPos;
        newPos.x = transform.position.x;
        newPos.y = SmoothFollow(dt);
        newPos.z = transform.position.z;

        transform.position = newPos;
    }
    private float SmoothFollow(float dt)
    {
        float currentY = transform.position.y;
        float currentTime = Time.time;
        distance = maxDistance;

        float idealY = target.position.y + heightOffset;

        float offset = idealY - currentY;

        float randDamping = Mathf.PerlinNoise(currentTime * 0.3f, 0) * 10f;
        float damp = Mathf.Min(1.0f, randDamping * dt);

        vel *= 1.0f - damp;

        float randSpringK = Mathf.PerlinNoise(currentTime * 0.5f, 0) * 30f;
        float springAccel = offset * randSpringK;
        vel += springAccel * dt;

        float movement = vel * dt;
        currentY += movement;

        offset = currentY - idealY;
        if (offset > distance)
        {
            offset *= (distance / offset);
            currentY = idealY + offset;
        }
        return currentY;
    }
}
