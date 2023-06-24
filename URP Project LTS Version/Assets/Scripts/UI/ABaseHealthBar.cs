using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ABaseHealthBar : MonoBehaviour, IDamage
{

    public ABaseCharacter characterHealth;
    public ABaseCharacterData characterMaxHealth;
    //public float changeSpeed;
    //public float lerpTime;

    public Slider healthbar;
    public Image healthImage;
    public TextMeshProUGUI healthText;
    public float damage;
    public float health;
    //public float maximumHealth = 1000;

    public event EventHandler OnDamaged;

    public bool takedDamage = false;


    /// <summary>
    /// Verilerin belli aralýktan baþka aralýðý taþýnmasýnda kullanýlan metottur
    /// </summary>
    /// <param name="oldmin">Eski aralýðýn minimum deðeri</param>
    /// <param name="oldmax">Eski aralýðýn maksimum deðeri</param>
    /// <param name="newmin">Yeni aralýðýn minimum deðeri</param>
    /// <param name="newmax">Yeni aralýðýn maksimum deðeri</param>
    /// <param name="value">Yeni aralýða taþýnacak deðer</param>
    /// <returns></returns>
    public float NormalizeData(float oldmin, float oldmax, float newmin, float newmax, float value)
    {
        return ((value - oldmin) / oldmax - oldmin) * (newmax - newmin) + newmin;
    }

    public void SetHealth(float value,Slider healthbarValue)
    {
        healthbarValue.value = value;
    }
    public void SetMaxHealth(float value, Slider healthbarMaxValue)
    {
        healthbarMaxValue.maxValue = value;
    }
    public void UpdateMaxHealth(float oldmaximumHealth, float newMaxHealth, float newMinHealth=0.0f)
    {
        health = healthbar.value;
        health = NormalizeData(0, oldmaximumHealth, newMinHealth, newMaxHealth, health);
        healthbar.maxValue = newMaxHealth;
        healthbar.value = health;
        healthText.text = health.ToString();

    }


    /// <summary>
    /// Hasarýn uygulandýðý sýnýf
    /// </summary>
    /// <param name="amount">Hesaplanmýþ hasar miktarý. Bu hasar miktarý kadar can azalacak</param>
    public void Damage(float amount)
    {
        //Debug.Log("damage");
        healthbar.value -= amount;
        takedDamage = true;
        if (health < 0)
        {
            health = 0;
        }

        if (OnDamaged != null)
        {
            OnDamaged(this, EventArgs.Empty);

        }
        takedDamage = false;
    }
    /*
    public void Heal(float amount)
    {
        health += amount;
        if (health > maximumHealth)
        {
            health = maximumHealth;
        }
        //OnHealed?.Invoke(this, EventArgs.Empty);
    }
    */

    /// <summary>
    /// Canýn integer'a dönüþtürülü string olarak geri döndürür
    /// </summary>
    /// <param name="healthbar"></param>
    /// <returns></returns>
    public string BarValueAsIntegerDisplay(Slider healthbar)
    {
        return Mathf.CeilToInt(healthbar.value).ToString();
    }

    public string BarValueAsIntegerDisplay(float value)
    {
        return Mathf.CeilToInt(value).ToString();
    }

    //TODO bunu kaldýrabilirim  Abasecharacter'da bir tane daha var

    /// <summary>
    ///     Ýyileþme miktarýný hesaplamak için kullanýlan metottur.
    /// </summary>
    /// <param name="rawHealing">Ham iyileþme miktarý bunun üzerinden hesplama yapýlmaktadýr. Örneðin düþmana verilen hasar ham iyileþme miktarýdýr.
    /// Bunun üzerinden iþlem yapýlmalý, mesela istenirse %50'si alýnabilir.
    /// </param>
    /// <param name="targetGameObject">Ýyileþmenin uygulanacaðý hedef</param>
    /// <param name="healingRatio">Ýyileþme oranýdýr. Ham iyileþme miktarýnýn ne kadarýnýn iyileþme olacaðýný belirler</param>
    /// <param name="healigMultiplier">Ýyileþmeyi oransal olarak azaltýr veya arttýrabilir, -100 ile 100 arasýnda bir sayý olmalýdýr. Varsayýlan 0'dýr.</param>
    public float Healing(float rawHealing, GameObject targetGameObject, float healingRatio, float healigMultiplier=0.0f)
    {
        Slider targetHealImage = targetGameObject.GetComponent<Slider>();
        float healthfill = targetHealImage.value;
        return healthfill + (rawHealing * (1 + healingRatio / 100) * (1 - healigMultiplier / 100));
    }
}