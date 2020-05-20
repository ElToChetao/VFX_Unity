using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraMaterialChanger : MonoBehaviour
{

    public Color myColor; // color you want the camera to render it as
    public Material material; // material you want the camera to change
    public string colorPropertyName; // name of the color property in the material's shader

    void OnPreRender()
    {
        _default = material.GetColor(colorPropertyName);
        material.SetColor(colorPropertyName, myColor);
    }

    void OnPostRender()
    {
        material.SetColor(colorPropertyName, _default);
    }

    private Color _default;
}