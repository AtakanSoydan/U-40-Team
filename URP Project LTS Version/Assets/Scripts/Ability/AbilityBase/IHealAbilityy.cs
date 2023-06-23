using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealAbilityy
{
    public bool HealingEnabled { get; set; }
    public float HealRate { get; set; }
    public float Healing(float rawHealing, float healingRatio, float healigMultiplier = 0.0f);
}
