using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class DamageDisappear : MonoBehaviour
{
    public Vector3 tempDamageTextPosition;
    private float randomX;
    private float randomY;
    private float randomZ;
    private Vector3 randomVector3;
    private Vector3 size;
    private TextMeshPro textMesh;
    private float duration = 0.0f;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        IGetBound characterColliderBounds = GetComponentInParent<IGetBound>();
        textMesh = GetComponent<TextMeshPro>();
        tempDamageTextPosition = textMesh.transform.localPosition;
        size = characterColliderBounds.GetBounds.size;
        randomVector3 = new Vector3();
        randomX = Random.Range(-size.x, size.x);
        randomY = Random.Range(-size.y, size.y);
        randomZ = Random.Range(-size.z, size.z);
        randomVector3.Set(randomX, randomY, randomZ);

    }

    // Update is called once per frame
    void Update()
    {
        Disappear();
    }

    public void Disappear()
    {
        if (duration <= 1)
        {
            textMesh.transform.localPosition = Vector3.Lerp(tempDamageTextPosition, tempDamageTextPosition + (randomVector3), duration);
            textMesh.alpha = 1 - duration;
            duration += Time.deltaTime;
            Debug.Log(size / 2);
            Debug.Log("randomVector: " + randomVector3);
            Debug.Log("Tesxtmesh position" + transform.localPosition);
            Debug.Log("Tesxtmesh position2" + transform.position + (size / 2));
        }
        else if (duration > 1)
        {
            Debug.Log("dissap çalýþtý");

            Destroy(gameObject);


        }
    }
}
