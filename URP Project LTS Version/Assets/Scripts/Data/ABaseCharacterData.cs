using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABaseCharacterData : ScriptableObject
{
    public string characterName = "Karakterin Ýsmi";
    public int characterIndex = 1;
    public int characterCount = 0;
    public int characterType = 0;
    public int Health = 100;
    public int LifeStealRatio = 0;
    public int Armor = 10;
    public int MovementSpeed = 10;
    public int AttackPower = 100;
    public int AttackSpeed = 100;
    public int Level = 1;
    public int Experiance = 1;
}
