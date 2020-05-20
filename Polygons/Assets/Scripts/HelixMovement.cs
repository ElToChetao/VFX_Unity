using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixMovement : MonoBehaviour
{
    private float newX;
    private float currentX;
    public Transform cam;
    public Transform fx;

    private float currentLerpTime;
    private const float lerpTime = 4;
    void Start()
    {
        newX = transform.position.x;
        currentX = newX;
        StartCoroutine(RandomPosition());
    }

    IEnumerator RandomPosition()
    {
        yield return new WaitForSeconds(lerpTime + Random.Range(0.0f, 1.0f));
        newX = Random.Range(-5.5f, 5.5f);
        currentLerpTime = 0;
        StartCoroutine(RandomPosition());
    }

    void Update()
    {
        float currentTime = Time.time;
        float newTimeScale = Mathf.PerlinNoise(currentTime * 0.3f, 0) + 0.7f;
        Time.timeScale = newTimeScale;

        float dt = Time.deltaTime;
        currentLerpTime += dt;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float t = currentLerpTime / lerpTime;
        t = t * t * (3f - 2f * t);

        currentX = Mathf.Lerp(currentX, newX, t);

        transform.Rotate(Vector3.up * dt * 100);

        float yOffset = transform.position.y + dt * 10;
        Vector3 offset = new Vector3(currentX, yOffset, 0);
        transform.position = offset;

        cam.position = new Vector3(cam.position.x, transform.position.y + Mathf.PerlinNoise(currentTime, 0) - 0.5f, cam.position.z);
        fx.position = new Vector3(fx.position.x, transform.position.y + 15, fx.position.z);
    }
    private float GetOffset(float dt, float input, float max, float currentSpeed, float accel)
    {
        float targetZSpeed = input * max;
        float velZOffset = targetZSpeed - currentSpeed;
        velZOffset = Mathf.Clamp(velZOffset, -accel * dt, accel * dt);
        return velZOffset;
    }
}
