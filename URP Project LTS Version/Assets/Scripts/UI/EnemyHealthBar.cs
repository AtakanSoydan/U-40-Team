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
    public Quaternion textCameraLookQuaternion;
    Vector3 barLookDirection;
    [SerializeField] private DissolveController_Deneme dissolve;

    // Start is called before the first frame update
    void Start()
    {
        SetMaxHealth(characterMaxHealth.maxHealth, healthbar);
        SetHealth(characterInfo.currentHealth, healthbar);
        healthText.text = BarValueAsIntegerDisplay(healthbar);
        levelText.text = $"Level {characterInfo.Level}";
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate()
    {
        barLookDirection = gameObject.transform.position - cameraTransform.position;
        textCameraLookQuaternion = Quaternion.LookRotation(barLookDirection);
        gameObject.transform.parent.rotation = textCameraLookQuaternion;

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
        UpdateCurrentHealthAndBarText(healthText, healthbar, characterInfo);
        if (characterInfo.currentHealth <=0 )
        {
            dissolve = characterInfo.gameObject.GetComponent<DissolveController_Deneme>();
            StartCoroutine(dissolve.Dissolve_Deneme());
            //dissolve.gameObject.SetActive(false);
        }
    }
}
