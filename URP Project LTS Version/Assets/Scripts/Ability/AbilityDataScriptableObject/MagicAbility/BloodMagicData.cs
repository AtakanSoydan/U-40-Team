using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
///     Yetenek i�in temel bilgileri sa�layan kodlanabilir nesne, bir veri deposu gibidir. Project k�sm�nda Sa� t�klama ile yeni nesne �retip farkl� versiyonlar olu�turulabilir.
///     Do�rudn gameobject'e eklenmez bir script �zerinden gerekli bilgilerin sa�lnmas�n yard�mc� olur.
///     Yeni nesnesini �retip ilk nesneden ba��ms�z olrak ba�ka yerlerde kullan�labilir.
///     �retilen nesne oyun oynarken de�i�irse bu de�i�iklik kal�c�d�r, kaydedilir.
///     �retilen nesne de�i�tirilirse o nesneyi kullanan b�t�n gameobject'ler ve script'ler bundan etkilenir
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
    /// Bu yetenekte asl�nda kullan�lm�yor
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
    /// Bu yetenekte asl�nda kullan�lm�yor
    /// </summary>
    public bool ContinuousAbilityEnable { get => continuousAbilityEnable; set => continuousAbilityEnable=value; }


    public override void UpdateAbility(float firstAbilityDelay, float abilityCooldown, float abilityDamage)
    {
        throw new System.NotImplementedException();
    }
}
