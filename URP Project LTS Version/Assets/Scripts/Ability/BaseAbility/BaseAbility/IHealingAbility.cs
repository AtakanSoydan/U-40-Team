using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     �yile�me yetenekleri i�in temel metotlar i�eren interface
/// </summary>
// TODO bunu ISupportAbility yapmak daha iyi olabilir, �zerine biraz d���nmek iyi olabilir
public interface IHealingAbility
{
    public void Healing(float rawHealing, GameObject targetGameObject, float healingRatio);
}
