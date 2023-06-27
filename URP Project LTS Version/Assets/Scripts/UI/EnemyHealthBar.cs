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
    Vector3 barLookDirection;

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
        barLookDirection = gameObject.transform.position - cameraTransform.position;
        quaternion = Quaternion.LookRotation(barLookDirection);
        gameObject.transform.parent.rotation = quaternion;

    }
    private void OnEnable()
    {
        OnDamaged += EnemyHealthBar_OnDamaged;
    }


    private void OnDisable()
    {
        OnDamaged -= EnemyHealthBar_OnDamaged;
    }

    private void EnemyHealthBar_OnDamaged(object sender, System.EventArgs e)
    {
        Debug.Log("enemy deneme2");
    }
}
