using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Yetenekler i�in temel s�n�f
/// </summary>
public abstract class ABaseAbility : MonoBehaviour
{
    public int abilityLevel = 1;
    public int abilityExp = 0;
    public int requiredExp = 100;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Yetene�in �al��t�r�ld��� s�n�f
    /// </summary>
    public abstract void TriggerAbility();

    /// <summary>
    /// Yetene�in i�eri�i
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator UseAbility(float firstAbilityDelay, float abilityCooldown, float abilityDamage);

    public abstract void UpdateAbility(float firstAbilityDelay, float abilityCooldown, float abilityDamage, int requiredExp, int abilityLevel);

    public abstract void SaveAbilityExpToScirptable(int abilityExp);
}