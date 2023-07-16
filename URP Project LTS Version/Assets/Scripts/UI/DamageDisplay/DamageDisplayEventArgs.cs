using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDisplayEventArgs : EventArgs
{
    private float damageAmount;


    public DamageDisplayEventArgs(float damageAmount)
    {
        this.damageAmount = damageAmount;
    }

    public float DamageAmount { get => damageAmount; set => damageAmount = value; }
}
