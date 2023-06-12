using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Ýyileþme yetenekleri için temel metotlar içeren interface
/// </summary>
// TODO bunu ISupportAbility yapmak daha iyi olabilir, üzerine biraz düþünmek iyi olabilir
public interface IHealingAbility
{
    public void Healing(float rawHealing, GameObject targetGameObject, float healingRatio);
}
