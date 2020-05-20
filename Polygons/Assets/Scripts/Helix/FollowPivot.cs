using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPivot : MonoBehaviour
{
    public Transform target;

    public float springK = 10.0f;
    public float damping = 5.0f;
    private Vector3 idealPos;
    private Vector3 cameraVel = new Vector3(0.0f, 0.0f, 0.0f);
    private float distance;
    public float maxDistance;

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        transform.position = SmoothFollow(dt);
        transform.LookAt(target);
    }
    private Vector3 SmoothFollow(float dt)
    {
        Vector3 pos = transform.position;

        distance = maxDistance;

        idealPos = target.position;

        Vector3 offset = idealPos - pos;
        float damp = Mathf.Min(1.0f, damping * dt);
        cameraVel *= 1.0f - damp;
        Vector3 springAccel = offset * springK;
        cameraVel += springAccel * dt;

        Vector3 movement = cameraVel * dt;
        pos += movement;

        offset = pos - idealPos;
        float sqrOffset = offset.sqrMagnitude;
        if (sqrOffset > distance * distance)
        {
            float offsetDis = Mathf.Sqrt(sqrOffset);
            offset *= (distance / offsetDis);
            pos = idealPos + offset;
        }
        return pos;
    }
}
