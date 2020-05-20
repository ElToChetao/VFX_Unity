using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    private Image image;
    private void Awake()
    {
        SceneManager.sceneLoaded += DoFade;
    }
    void Start()
    {
        image = GetComponent<Image>();
    }

    private void DoFade(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeDelay());
    }
    private IEnumerator FadeDelay()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(Fade());
    }
    IEnumerator Fade()
    {
        yield return null;
        Color c = image.color;
        c.a *= 0.96f;
        image.color = c;
        if(c.a > 0.01f)
        {
            StartCoroutine(Fade());
        }
    }
}
