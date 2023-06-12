using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContinousAbilityData
{
    //public abstract float ContinuousAbilityDelay { get; set; }
    //public float ContinuousAttackSpeed { get; set;}
    public float ContinuousAbilityDuration { get; set; }
    public float ContinuousAbilityRepetitions { get; set;}
    public bool ContinuousAbilityEnable { get; set; }
    // public float ContinousDamage { get; set;}
}
