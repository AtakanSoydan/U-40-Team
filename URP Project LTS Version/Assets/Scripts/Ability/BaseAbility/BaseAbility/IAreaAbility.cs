using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Alan etkili yetenekler i�in temel metotlar i�eren interface
/// </summary>
public interface IAreaAbility
{
    public GameObject ShowArea(float areaRadius, GameObject gameObject, float areaAlpha);
    public Collider2D[] SearchArea(float areaRadius, GameObject gameObject);
}
