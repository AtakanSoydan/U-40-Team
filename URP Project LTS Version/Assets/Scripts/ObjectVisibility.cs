using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectVisibility : MonoBehaviour
{
    public float fadeSpeed, fadeAmount;
    private float originalOpacitiy;
    Material[] _materials;
    public bool doFade = false;

    private void Awake()
    {
        _materials = GetComponent<Renderer>().materials;
        Debug.Log(_materials);

        foreach (Material material in _materials)
        {
            originalOpacitiy = material.color.a;
        }
    }


    private void Update()
    {
        if (doFade)
        {
            FadeNow();
        }
        else
        {
            ResetFade();
        }
    }

    private void FadeNow()
    {
        foreach (Material material in _materials)
        {
            Color currenColor = material.color;
            Color smoothColor = new Color(currenColor.r, currenColor.g, currenColor.b, Mathf.Lerp(currenColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
            material.color = smoothColor;
        }
    }

    private void ResetFade()
    {
        foreach (Material material in _materials)
        {
            Color currenColor = material.color;
            Color smoothColor = new Color(currenColor.r, currenColor.g, currenColor.b, Mathf.Lerp(currenColor.a, originalOpacitiy, fadeSpeed * Time.deltaTime));
            material.color = smoothColor;
        }
    }
}

