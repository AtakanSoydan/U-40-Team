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
        //healthbar.maxValue = characterMaxHealth.maxHealth;
        //healthbar.value = characterHealth.currentHealth;
        SetMaxHealth(characterMaxHealth.maxHealth, healthbar);
        SetHealth(characterHealth.currentHealth, healthbar);
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
        isActiveAutoHealing = false;
        healthText.text = BarValueAsIntegerDisplay(healthbar);
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
            autoHealTimerText.text = Mathf.CeilToInt(autoHealTimer % autoHealTimerDivede).ToString();
            //Debug.Log(autoHealTimer % autoHealTimerDivede);
        }
        else if (autoHealTimer <= 0 && !takedDamage)
        {
            isActiveAutoHealing= true;
            float tempvalue = healthbar.value + Time.deltaTime * autoHealSpeed;
            healthbar.value += Time.deltaTime * autoHealSpeed;
            //Debug.Log(healthbar.value);
            healthText.text = BarValueAsIntegerDisplay(healthbar);
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

        /*
        damagedHealthShrinkTimer -= Time.deltaTime;
        if (damagedHealthShrinkTimer < 0)
        {
            if (healthImage.rectTransform.anchorMax.x < DamagedHealth.rectTransform.anchorMax.x)
            {
                Debug.Log("Çalýþýyor2");
                float fadeamount = 0.12f;
                //Vector2 tempAnchor = new Vector2(0,0);
                float tempx = DamagedHealth.rectTransform.anchorMax.x - fadeamount * Time.deltaTime;
                float tempy = DamagedHealth.rectTransform.anchorMax.y;
                tempAnchor.Set(tempx, tempy);
                DamagedHealth.rectTransform.anchorMax = tempAnchor;
            }
        }
        */

        /* ***Olmaadý***
        while (damagedHealthShrinkTimer>0)
        {
            damagedHealthShrinkTimer -= Time.deltaTime;
        }

        while (healthImage.rectTransform.anchorMax.x < DamagedHealth.rectTransform.anchorMax.x)
        {
            Debug.Log("Çalýþýyor2");
            float fadeamount = 0.12f;
            //Vector2 tempAnchor = new Vector2(0,0);
            float tempx = DamagedHealth.rectTransform.anchorMax.x - fadeamount * Time.deltaTime;
            float tempy = DamagedHealth.rectTransform.anchorMax.y;
            tempAnchor.Set(tempx, tempy);
            DamagedHealth.rectTransform.anchorMax = tempAnchor;

        }
        //float damagedHealthShrinkTimer, Image healthImage
        */

    }




    /*
    //public ABaseCharacter currentHealth;
    //public ABaseCharacterData characterMaxHealth;
    public float changeSpeed;
    public float lerpTime;

    public Image DamagedHealth;
    public Color DamagedHealthColor;
    public Color tempDamagedHealthColor;
    public Slider healthbar;
    public Image healthImage;
    public TextMeshProUGUI healthText;
    public float damage;
    public float health;
    public float maximumHealth = 1000;
    public float damagedHealthShrinkTimer;
    public const float damageShrinkTimerMax = 1f;
    Vector2 tempAnchor;


    public event EventHandler OnDamaged;
    //public event EventHandler OnHealed;
    // Start is called before the first frame update
    void Start()
    {
        DamagedHealthColor = DamagedHealth.color;
        tempDamagedHealthColor = DamagedHealthColor;
        DamagedHealthColor.a = 0;
        tempAnchor = new Vector2();
        healthText.text = healthbar.value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        DamageBarEffect();
        
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
    /*
    private void PlayerHealthBar_OnDamaged(object sender, EventArgs e)
    {
        damagedHealthShrinkTimer = damageShrinkTimerMax;
        healthText.text = healthbar.value.ToString();
        //DamageBarEffect();
        Debug.Log("Çalýþýyor");
        
    }

    private void OnDisable()
    {
        OnDamaged -= PlayerHealthBar_OnDamaged;
    }

    public float NormalizeData(float oldmin, float oldmax, float newmin, float newmax, float value)
    {
        return ((value - oldmin) / oldmax - oldmin) * (newmax - newmin) + newmin;
    }

    public float SetHealth(float value)
    {
        return healthbar.value = value;
    }
    public void UpdateMaxHealth(float newMaxHealth)
    {
        health = healthbar.value;
        health = NormalizeData(0, maximumHealth, 0, newMaxHealth, health);
        healthbar.maxValue = newMaxHealth;
        healthbar.value = health;
        healthText.text = health.ToString();
    }

    public void HealthSystem(float health)
    {
        this.health = health;
    }
    public void Damage(float amount)
    {
        healthbar.value -= amount;
        if (health<0)
        {
            health = 0;
        }
        if (OnDamaged !=null)
        {
            OnDamaged(this, EventArgs.Empty);
        }
    }
    public void Heal(float amount)
    {
        health += amount;
        if (health>maximumHealth)
        {
            health = maximumHealth;
        }
        //OnHealed?.Invoke(this, EventArgs.Empty);
    }
    public void DamageBarEffect() 
    {
        Debug.Log("Çalýþýyor3");
        damagedHealthShrinkTimer -= Time.deltaTime;
        if (damagedHealthShrinkTimer < 0)
        {
            if (healthImage.rectTransform.anchorMax.x < DamagedHealth.rectTransform.anchorMax.x)
            {
                Debug.Log("Çalýþýyor2");
                float fadeamount = 0.12f;
                //Vector2 tempAnchor = new Vector2(0,0);
                float tempx = DamagedHealth.rectTransform.anchorMax.x - fadeamount * Time.deltaTime;
                float tempy = DamagedHealth.rectTransform.anchorMax.y;
                tempAnchor.Set(tempx, tempy);
                DamagedHealth.rectTransform.anchorMax = tempAnchor;
            }
        }
        /*
        damagedHealthShrinkTimer -= Time.deltaTime;
        if (damagedHealthShrinkTimer < 0)
        {
            if (healthImage.rectTransform.anchorMax.x < DamagedHealth.rectTransform.anchorMax.x)
            {
                Debug.Log("Çalýþýyor2");
                float fadeamount = 0.12f;
                //Vector2 tempAnchor = new Vector2(0,0);
                float tempx = DamagedHealth.rectTransform.anchorMax.x - fadeamount * Time.deltaTime;
                float tempy = DamagedHealth.rectTransform.anchorMax.y;
                tempAnchor.Set(tempx, tempy);
                DamagedHealth.rectTransform.anchorMax = tempAnchor;
            }
        }
        */

    /* ***Olmaadý***
    while (damagedHealthShrinkTimer>0)
    {
        damagedHealthShrinkTimer -= Time.deltaTime;
    }

    while (healthImage.rectTransform.anchorMax.x < DamagedHealth.rectTransform.anchorMax.x)
    {
        Debug.Log("Çalýþýyor2");
        float fadeamount = 0.12f;
        //Vector2 tempAnchor = new Vector2(0,0);
        float tempx = DamagedHealth.rectTransform.anchorMax.x - fadeamount * Time.deltaTime;
        float tempy = DamagedHealth.rectTransform.anchorMax.y;
        tempAnchor.Set(tempx, tempy);
        DamagedHealth.rectTransform.anchorMax = tempAnchor;

    }

}*/

}
