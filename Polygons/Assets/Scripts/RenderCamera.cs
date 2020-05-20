using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCamera : MonoBehaviour
{
    public Shader replacementShader;
    public string replacementTag;

    void OnValidate()
    {
        GetComponent<Camera>().SetReplacementShader(replacementShader, replacementTag);
    }
}
