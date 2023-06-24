using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAreaAbilityy
{
    public float AreaRadius { get; set; }
    public Collider[] SearchedArea { get; set; }
    public int LayerMasktoSearch { get; set; }


    public GameObject ShowArea(float areaRadius, GameObject dummyarea, float areaAlpha = 60.0f);

    public int SearchArea(float areaRadius, GameObject gameObject, Collider[] resultColliders, int layer);

    /*
    public bool CanMove { get; set; }
    public RaycastHit[] HitInfo { get; set; }
    public Ray Ray { get; set; }
    public Camera ActiveCamera { get; set; }
    public void MoveArea(GameObject denemeArea);
     */
}
