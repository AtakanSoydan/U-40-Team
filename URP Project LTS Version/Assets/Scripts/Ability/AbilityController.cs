using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour, IExecuteAbility
{
    //public BloodMagicData bloodMagicData;
    public BloodMagic bloodMagic;
    public BloodMagicAbility ability;


    public void ExecuteAbility(ABaseAbilityData triggeredAbility)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        Player.BloodMagic += bloodMagic.TriggerAbility;
    }
    private void OnDisable() 
    {
        Player.BloodMagic -= bloodMagic.TriggerAbility;
    }
}

