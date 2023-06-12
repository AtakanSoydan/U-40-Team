using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class BloodMagic : AMagicAbility, IAreaAbility, IHealingAbility, IContinousAbility
{
    public BloodMagicData bloodMagicData;
    public Coroutine bloodMagicCoroutine;
    public GameObject _dummyArea;
    public GameObject _healingTarget;
    public GameObject healthBar;
    public Image health;
    public int targetLayerMask = 1 << 8;
    public GameObject _gameObject;
    private float a = 0.2f;
    private float den = 0;
    private float b = 0.7f;
    [Header("Deneme")]
    [SerializeField]
    float healthfill;
    [SerializeField] float normalizehealth;
    [SerializeField] float healHealth;
    [SerializeField] float playerHealth;
    [SerializeField] float tempHealth;
    public bool isActiveted = true;

    // Start is called before the first frame update
    void Start()
    {
        bloodMagicData.LayerMasktoSearch = targetLayerMask;
        bloodMagicData.healingTarget = _healingTarget;
        bloodMagicData._gameObject = _gameObject;
        bloodMagicData.isAbilityActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    ///     Göstermelik alaný aktive eden metot
    /// </summary>
    /// <param name="areaRadius"> Alanýn yarýçapý </param>
    /// <param name="dummyarea"> Gösterilecek sahte alan </param>
    /// <returns></returns>
    public GameObject ShowArea(float areaRadius, GameObject dummyarea, float areaAlpha = 60.0f)
    {

        dummyarea.transform.localScale.Set(areaRadius, areaRadius, dummyarea.transform.localScale.z);
        dummyarea.SetActive(true);
        return dummyarea;

    }
    /// <summary>
    ///     BloodMagic yeteneði için bir yarýçapta çemberin içinde kalan belli bir Layer'ý(8.Layer) tarýyor.
    /// </summary>
    /// <param name="areaRadius"> Taranacak yarýçap </param>
    /// <param name="gameObject"> Taramanýn baþlatýlacaðý nokta </param>
    /// <returns></returns>
    public Collider2D[] SearchArea(float areaRadius, GameObject gameObject)
    {

        return Physics2D.OverlapCircleAll(gameObject.transform.position, areaRadius, bloodMagicData.LayerMasktoSearch);
    }
    /// <summary>
    /// AbilityController'da yeteneðin bir aktive edilmesinde kullanýlýyor.
    /// </summary>
    public override void TriggerAbility()
    {
        if (bloodMagicData.isAbilityActive)
        {
            bloodMagicCoroutine = StartCoroutine(UseAbility(bloodMagicData.firstAbilityDelay, bloodMagicData.abilityCooldown, bloodMagicData.abilityDamage));
        }
    }

    /// <summary>
    ///     Yeteneðin uygulndýðý bölümdür. Ýçerisinde kullanýlan bütün bilgiler Blood Magic'in Scriptable object'inden saðlanmaktadýr.
    ///     Ýlk uygulanma gecikmesi bulunmaktadýr. Ýlk baþta bir göstermelik alan ortaya çýkýyor. 
    ///     Ardýndan deðiþtirilebilir(10 saniye) bir süre boyunca saniyede 1 kez belli bir yarýçptaki(10.0f) 8.Layer'a ait nesneleri tarýyor ve 
    ///     bulunan her nesneye(temel olarak düþmana) hasar veriyor, verilen hasarýn bir maktarý iyileþme olarak geri dönüyor.
    ///     ContinuousAbilityEnable isminde bool deðiþken ile istenirse sürekli veya bir kerelik etki saðlanabilir.
    /// </summary>
    /// <returns></returns>
    public override IEnumerator UseAbility(float firstAbilityDelay, float abilityCooldown, float abilityDamage)
    {
        while (isActiveted)
        {
            isActiveted = false;
            bloodMagicData.isAbilityActive = false;
            yield return new WaitForSeconds(firstAbilityDelay);
            ShowArea(bloodMagicData.AreaRadius, _dummyArea);

            float tempContinuousAbilityRepetitions = bloodMagicData.ContinuousAbilityRepetitions;
            if (bloodMagicData.HealingEnabled)
            {
                while (tempContinuousAbilityRepetitions > 0)
                {

                    bloodMagicData.SearchedArea = SearchArea(bloodMagicData.AreaRadius, bloodMagicData._gameObject);
                    foreach (Collider2D enemy in bloodMagicData.SearchedArea)
                    {
                        Debug.Log("çalýþýyor: " + enemy.name);
                        //enemy.gameObject.GetComponentInChildren<EnemyHealth>()?.TakeDamage(abilityDamage);
                        yield return StartCoroutine(Healingg(abilityDamage, healthBar.gameObject, bloodMagicData.healRate));

                    }
                    tempContinuousAbilityRepetitions--;
                    yield return new WaitForSeconds(bloodMagicData.ContinuousAbilityDuration);

                }
                _dummyArea.SetActive(false);
            }
            else
            {
                while (tempContinuousAbilityRepetitions > 0)
                {

                    bloodMagicData.SearchedArea = SearchArea(bloodMagicData.AreaRadius, bloodMagicData._gameObject);
                    foreach (Collider2D enemy in bloodMagicData.SearchedArea)
                    {
                        Debug.Log("çalýþýyor");
                        //enemy.gameObject.GetComponentInChildren<EnemyHealth>()?.TakeDamage(abilityDamage);

                    }
                    tempContinuousAbilityRepetitions--;
                    yield return new WaitForSeconds(bloodMagicData.ContinuousAbilityDuration);

                }
                _dummyArea.SetActive(false);
            }
            tempContinuousAbilityRepetitions = bloodMagicData.ContinuousAbilityRepetitions;


            yield return new WaitForSeconds(abilityCooldown);

        }
        isActiveted = true;
        bloodMagicData.isAbilityActive = true;
    }

    /// <summary>
    ///     Ýyileþme yeteneði için kullanýlan metottur. Þimdilik bir can bar kontrolü için script olmadýðý için özel olarak sahnedeki helath barýn
    ///     ilk child nesnesinin altýndaki child'da karakterin can barýný temsil eden Image var. Bu Image componentinin fill deðerini deðerini deðiþrtiryor
    /// </summary>
    /// <param name="rawHealing">Ham iyileþme miktarý bunun üzerinden hesplama yapýlmaktadýr. Örneðin düþmana verilen hasar ham iyileþme miktarýdýr.
    /// Bunun üzerinden iþlem yapýlmalý, mesela istenirse %50'si alýnabilir.
    /// </param>
    /// <param name="healingRatio">Ýyileþme oranýdýr. Varsayýlan deðeri 1'dir.  Ham iyileþme miktarýnýn ne kadarýnýn iyileþme olacaðýný belirler</param>
    /// <param name="targetGameObject">Þimdilik Healthbarýn kendisini özel olarak tutan bir parametredir. Ama esas amacý iyileþtirmenin hedefini belirlemk</param>
    
    public void Healing(float rawHealing, GameObject targetGameObject, float healingRatio)
    {

        health = targetGameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponentInChildren<Image>();
        healthfill = health.fillAmount;
        normalizehealth = NormalizeData(0, 1, 0, 100, healthfill);
        healHealth = normalizehealth + (rawHealing * healingRatio);
        playerHealth = NormalizeData(0, 100, 0, 1, healHealth);
        /*;
    float timer = 0;
    while (timer<1)
    {
        health.fillAmount = Mathf.Lerp(health.fillAmount, playerHealth, timer);
        timer += 0.005f;
    }
    */
        //tempHealth = Mathf.SmoothDamp(health.fillAmount, playerHealth, ref a, b);
        //health.fillAmount = tempHealth
        float temp2 = health.fillAmount;
        while (den < 1.0f)
        {
            tempHealth = Mathf.Lerp(temp2, playerHealth, den);
            health.fillAmount = tempHealth;
            den = den + 0.01f;
        }
        den=0;
    }

    public IEnumerator Healingg(float rawHealing, GameObject targetGameObject, float healingRatio)
    {
        while (bloodMagicData.HealingEnabled)
        {
            bloodMagicData.HealingEnabled = false;
            health = targetGameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponentInChildren<Image>();
            healthfill = health.fillAmount;
            normalizehealth = NormalizeData(0, 1, 0, 100, healthfill);
            healHealth = normalizehealth + (rawHealing * healingRatio);
            playerHealth = NormalizeData(0, 100, 0, 1, healHealth);
            /*;
        float timer = 0;
        while (timer<1)
        {
            health.fillAmount = Mathf.Lerp(health.fillAmount, playerHealth, timer);
            timer += 0.005f;
        }
        */
            //tempHealth = Mathf.SmoothDamp(health.fillAmount, playerHealth, ref a, b);
            //health.fillAmount = tempHealth
            float temp2 = health.fillAmount;
            while (den < 1.0f)
            {
                tempHealth = Mathf.Lerp(temp2, playerHealth, den);
                health.fillAmount = tempHealth;
                den = den + 0.05f;
                yield return new WaitForSeconds(0.001f);
            }
            den = 0;
            yield return null;
        }
        bloodMagicData.HealingEnabled = true;
    }

    public override void UpdateAbility(float firstAbilityDelay, float abilityCooldown, float abilityDamage, int requiredExp, int abilityLevel)
    {
        bloodMagicData.RequiredExp = requiredExp;
        bloodMagicData.AbilityLevel = abilityLevel;
        bloodMagicData.firstAbilityDelay= firstAbilityDelay;
        bloodMagicData.abilityCooldown =abilityCooldown;
        bloodMagicData.abilityDamage=abilityDamage;
    }

    public override void SaveAbilityExpToScirptable(int abilityExp)
    {
        bloodMagicData.AbilityExp = abilityExp;
    }
    public float NormalizeData(float oldmin, float oldmax, float newmin, float newmax, float value)
    {
        return ((value - oldmin) / oldmax - oldmin) * (newmax - newmin) + newmin;
    }

}
