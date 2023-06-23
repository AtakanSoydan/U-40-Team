using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerCharacter : ABaseCharacter//, IAreaAbility, IHealingAbility 
{
    public DenemeAbility bloodMagicData;
    public DenemeAbility2 staticAbility;

    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bloodMagicData.TriggerAbility();

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            staticAbility.TriggerAbility();

        }
        //MoveArea(_dummyArea);

        if (bloodMagicData.canMove)
        {
            bloodMagicData.MoveArea(bloodMagicData._dummyArea);
            Debug.Log("Ray çalýþýyor2");
        }
        
    }

}
