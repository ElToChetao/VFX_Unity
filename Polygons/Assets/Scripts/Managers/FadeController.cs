using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    private Image image;
    private float currentLerpTime;
    private const float lerpTime = 2;
    private bool sceneLoaded;

    private void Awake()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }
    void Start()
    {
        image = GetComponent<Image>();
        currentLerpTime = 0;
    }
    private void Update()
    {
        if (sceneLoaded)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
                Destroy(gameObject);
            }

            float t = currentLerpTime / lerpTime;
            t *= t;

            Color col = image.color;
            col.a = Mathf.Lerp(col.a, 0, t);
            image.color = col;
        }
    }
    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        sceneLoaded = true;
    }
}
