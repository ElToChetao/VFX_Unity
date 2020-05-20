using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGPostpo : MonoBehaviour
{
    public Shader shader;
    public Material mat;
    public float stripes;
    public Color stripesCol;
    public float stripeOffset;
    public float stripeSpeed;

    void Start()
    {
        mat = new Material(shader);        
    }
    private void Update()
    {
        stripeOffset += stripeSpeed * Time.deltaTime;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        mat.SetFloat("_Stripe", stripes);
        mat.SetColor("_StripeCol", stripesCol);
        mat.SetFloat("_StripeOffset", stripeOffset);
        Graphics.Blit(source, destination, mat);
    }
}
