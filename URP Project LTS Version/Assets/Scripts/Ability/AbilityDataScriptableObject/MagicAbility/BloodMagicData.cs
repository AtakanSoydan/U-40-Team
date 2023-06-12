using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
///     Yetenek için temel bilgileri saðlayan kodlanabilir nesne, bir veri deposu gibidir. Project kýsmýnda Sað týklama ile yeni nesne üretip farklý versiyonlar oluþturulabilir.
///     Doðrudn gameobject'e eklenmez bir script üzerinden gerekli bilgilerin saðlnmasýn yardýmcý olur.
///     Yeni nesnesini üretip ilk nesneden baðýmsýz olrak baþka yerlerde kullanýlabilir.
///     Üretilen nesne oyun oynarken deðiþirse bu deðiþiklik kalýcýdýr, kaydedilir.
///     Üretilen nesne deðiþtirilirse o nesneyi kullanan bütün gameobject'ler ve script'ler bundan etkilenir
/// </summary>
[CreateAssetMenu(menuName = "MagicSpeel/BloodMagic")]
[Serializable]
public class BloodMagicData : AMagicAbilityData, IAreaAbilityData, IContinousAbilityData, IHealingAbilityData
{
    //public float maxRate = 1.0f;
    public float healRate = 0.4f;
    public GameObject healingTarget;
    [SerializeField] public GameObject _gameObject;


    [SerializeField] private float areaRadius = 21.0f;
    [SerializeField] private Collider2D[] searchedArea;
    private int layerMasktoSearch;


    [SerializeField] private float continuousAbilityDuration = 1.0f;
    [SerializeField] private float continuousAbilityRepetitions = 10.0f;
    /// <summary>
    /// Bu yetenekte aslýnda kullanýlmýyor
    /// </summary>
    [SerializeField] private bool continuousAbilityEnable = true; 
    [SerializeField] private bool healingEnabled = true;

    public float HealRate { get => healRate; set => healRate = value; }

    public float AreaRadius { get => areaRadius; set => areaRadius=value; }
    public Collider2D[] SearchedArea 
    { 
        get => searchedArea;
        set
        {
            searchedArea = value;
        }
    }
    public int LayerMasktoSearch { get; set; }

    public float ContinuousAbilityDuration { get => continuousAbilityDuration; set => continuousAbilityDuration=value; }
    public float ContinuousAbilityRepetitions { get => continuousAbilityRepetitions; set => continuousAbilityRepetitions = value; }
    public bool HealingEnabled { get => healingEnabled; set => healingEnabled = value; }
    /// <summary>
    /// Bu yetenekte aslýnda kullanýlmýyor
    /// </summary>
    public bool ContinuousAbilityEnable { get => continuousAbilityEnable; set => continuousAbilityEnable=value; }


    public override void UpdateAbility(float firstAbilityDelay, float abilityCooldown, float abilityDamage)
    {
        throw new System.NotImplementedException();
    }
}
