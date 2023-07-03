using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABaseAbilityClass : MonoBehaviour
{
    [Header("ABaseAbilityData")]
    public float firstAbilityDelay = 0.1f;
    public float abilityCooldown = 5.0f;
    public float abilityDamage = 50.0f;
    public float abilityDamageRatio = 20f;
    public string abilityName = "Yetenek Ýsmi";
    public string abilityDescription = "Yeteneðin açýklanmasý";
    //public Transform startPoint;
    public bool isAbilityActive = true;

    public int abilityLevel = 1;
    public int abilityExp = 0;
    public int requiredExp = 100;

    public int AbilityLevel { get => abilityLevel; set => abilityLevel = value; }
    public int AbilityExp { get => abilityExp; set => abilityExp = value; }
    public int RequiredExp { get => requiredExp; set => requiredExp = value; }

    [SerializeField] public GameObject _gameObject;
    public bool isActiveted = true;

    public void GetAbilityToTargetPositionChild(GameObject ability, GameObject parent)
    {

        ability.transform.SetParent(parent.transform,false);

    }
}
