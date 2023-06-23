using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : ABaseHealthBar
{
    public TextMeshProUGUI levelText;
    public Transform cameraTransform;
    public Quaternion quaternion;
    Vector3 point;
    // Start is called before the first frame update
    void Start()
    {
        SetMaxHealth(characterMaxHealth.maxHealth, healthbar);
        SetHealth(characterHealth.currentHealth, healthbar);
        healthText.text = BarValueAsIntegerDisplay(healthbar);
        levelText.text = $"Level {characterHealth.Level}";
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        point = gameObject.transform.position - cameraTransform.position;
        quaternion = Quaternion.LookRotation(point);
        gameObject.transform.parent.rotation = quaternion;

    }
}
