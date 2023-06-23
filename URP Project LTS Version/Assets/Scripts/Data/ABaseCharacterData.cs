using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ABaseCharacterData : ScriptableObject
{
    public string characterName = "Karakterin Ýsmi";
    public int characterIndex = 1;
    public int characterCount = 0;
    public int characterType = 0;
    public int maxHealth = 1000;
    public int maxLifeStealRatio = 0;
    public int maxArmor = 1000;
    public int maxMovementSpeed = 10;
    public int maxAttackPower = 100;
    public int maxAttackSpeed = 100;
    public int maxLevel = 100;
    public int maxExperiance = 1;
    public float maxAttackMultiplier = 0f;
    public float maxDefenseMultiplier = 0f;
}
