using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class PlayerHealthBar : ABaseHealthBar
{
    public Image DamagedHealth;
    public Color DamagedHealthColor;
    public Color tempDamagedHealthColor;
    public float damagedHealthShrinkTimer;
    public const float damageShrinkTimerMax = 1f;
    public Vector2 tempAnchor;


    public bool isActiveAutoHealing = false;
    public float autoHealTimer = 5f;
    public const float autoHealTimerConst = 5f;
    public float autoHealTimerMultiplier = 1f;
    public Slider autoHealTimerSlider;
    public float autoHealTimerDivede = 6f;
    public float autoHealSpeed = 0.62f;
    public TextMeshProUGUI autoHealTimerText;

    public void Start()
    {

        SetMaxHealth(characterMaxHealth.maxHealth, healthbar);
        SetHealth(characterInfo.currentHealth, healthbar);
        DamagedHealthColor = DamagedHealth.color;
        tempDamagedHealthColor = DamagedHealthColor;
        DamagedHealthColor.a = 0;
        tempAnchor = new Vector2();
        healthText.text = BarValueAsIntegerDisplay(healthbar);
        DamagedHealth.rectTransform.anchorMax = healthImage.rectTransform.anchorMax;
        autoHealTimerSlider.maxValue= autoHealTimer;
        autoHealTimerSlider.value = autoHealTimer;
        autoHealTimerText.text = BarValueAsIntegerDisplay(autoHealTimerSlider);

    }
    void Update()
    {
        SlowChanceBarEffect();
        HealPassive();
    }

    private void OnEnable()
    {
        OnDamaged += PlayerHealthBar_OnDamaged;
        //OnHealed += PlayerHealthBar_OnHealed;
    }
    /*
    private void PlayerHealthBar_OnHealed(object sender, EventArgs e)
    {
        
    }
    */
    private void PlayerHealthBar_OnDamaged(object sender, EventArgs e)
    {
        damagedHealthShrinkTimer = damageShrinkTimerMax;
        autoHealTimer = autoHealTimerConst;
        takedDamage = false;
        UpdateCurrentHealthAndBarText(healthText, healthbar, characterInfo);
        isActiveAutoHealing = false;
        /*

        healthText.text = BarValueAsIntegerDisplay(healthbar);
        */
        UpdateCurrentHealthWithBarText(healthText.text, characterInfo);
        //int.TryParse(healthText.text, out characterInfo.currentHealth);
        //DamageBarEffect();
        //Debug.Log("Çalýþýyor");


    }


    private void OnDisable()
    {
        OnDamaged -= PlayerHealthBar_OnDamaged;
    }

    public void HealPassive()
    {
        if (autoHealTimer > 0 && !takedDamage)
        {
            autoHealTimer -= Time.deltaTime * autoHealTimerMultiplier;
            autoHealTimerSlider.value = autoHealTimer;
            autoHealTimerText.text = BarValueAsIntegerDisplay(autoHealTimer % autoHealTimerDivede);
            //Debug.Log(autoHealTimer % autoHealTimerDivede);
        }
        else if (autoHealTimer <= 0 && !takedDamage)
        {
            isActiveAutoHealing= true;
            float tempvalue = healthbar.value + Time.deltaTime * autoHealSpeed;
            healthbar.value += Time.deltaTime * autoHealSpeed;
            //Debug.Log(healthbar.value);
            UpdateCurrentHealthAndBarText(healthText, healthbar, characterInfo);
            /*
            healthText.text = BarValueAsIntegerDisplay(healthbar);
            UpdateCurrentHealthWithBarText(healthText.text, characterInfo);
            */
            DamagedHealth.rectTransform.anchorMax = healthbar.fillRect.anchorMax;
        }

    }

    public void SlowChanceBarEffect()
    {
        if (damagedHealthShrinkTimer >= 0)
        {
            //Debug.Log("Çalýþýyor3");
            damagedHealthShrinkTimer -= Time.deltaTime;

        }
        else if (damagedHealthShrinkTimer < 0)
        {
            if (healthImage.rectTransform.anchorMax.x < DamagedHealth.rectTransform.anchorMax.x)
            {
                Debug.Log("Çalýþýyor2");
                float fadeamount = 0.12f;
                float tempx = DamagedHealth.rectTransform.anchorMax.x - fadeamount * Time.deltaTime;
                float tempy = DamagedHealth.rectTransform.anchorMax.y;
                tempAnchor.Set(tempx, tempy);
                DamagedHealth.rectTransform.anchorMax = tempAnchor;
            }
        }
    }
}
