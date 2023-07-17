using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveController_Deneme : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public VisualEffect vfx;

    public Material[] materials;
    public float[] tempStartDissolve;

    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (materials != null)
        {
            materials = meshRenderer.materials;
            tempStartDissolve = new float[materials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                tempStartDissolve[i] = materials[i].GetFloat("_DissolveAmount");
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Dissolve_Deneme()
    {

        //vfx?.Play();
        if (vfx != null)
        {
            vfx.Play();
            Debug.Log("çalýþýyor");
        }

        if (materials.Length > 0)
        {
            float counter = 0;
            

            while (materials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat("_DissolveAmount", counter);
                }

                if (counter>0.75f)
                {

                    gameObject.SetActive(false);
                    RestoreMaterialDissolve();
                    //materials = tempMaterials;
                    yield break;
                }
                yield return new WaitForSeconds(refreshRate);
            }
            //gameObject.SetActive(false);
        }
    }
    public void RestoreMaterialDissolve()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_DissolveAmount", tempStartDissolve[i]);
        }
    }
    /*
    public void DissolveCoroutine()
    {
        StartCoroutine(Dissolve_Deneme());
    }*/
}

