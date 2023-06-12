using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAreaAbilityData
{
    public float AreaRadius { get; set; }
    public Collider2D[] SearchedArea { get; set; }
    public int LayerMasktoSearch { get; set; }
    //public int MaxAreaRadius { get; set; }
    //public float RangedAttackSpeed { get; set; }
    /*
    public GameObject ShowArea(float areaRadius, GameObject gameObject);
    public Collider2D[] SearchArea(float areaRadius, GameObject gameObject, int layerMask);
    */
}
