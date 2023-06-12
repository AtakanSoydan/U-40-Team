using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangeAbilityData
{
    public int RangeArea { get; set; }
    //public int MaxRangeArea { get; set; }
    public float RangedAttackSpeed { get; set; }
    //public float RangedAttackRate { get; set; }
    // public float RangedAttackDamege { get; set; }

    public void ShowRangeArea(int rangeArea);
}
