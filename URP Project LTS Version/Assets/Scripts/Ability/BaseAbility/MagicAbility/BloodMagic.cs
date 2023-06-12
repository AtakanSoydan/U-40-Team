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
    ///     G�stermelik alan� aktive eden metot
    /// </summary>
    /// <param name="areaRadius"> Alan�n yar��ap� </param>
    /// <param name="dummyarea"> G�sterilecek sahte alan </param>
    /// <returns></returns>
    public GameObject ShowArea(float areaRadius, GameObject dummyarea, float areaAlpha = 60.0f)
    {

        dummyarea.transform.localScale.Set(areaRadius, areaRadius, dummyarea.transform.localScale.z);
        dummyarea.SetActive(true);
        return dummyarea;

    }
    /// <summary>
    ///     BloodMagic yetene�i i�in bir yar��apta �emberin i�inde kalan belli bir Layer'�(8.Layer) tar�yor.
    /// </summary>
    /// <param name="areaRadius"> Taranacak yar��ap </param>
    /// <param name="gameObject"> Taraman�n ba�lat�laca�� nokta </param>
    /// <returns></returns>
    public Collider2D[] SearchArea(float areaRadius, GameObject gameObject)
    {

        return Physics2D.OverlapCircleAll(gameObject.transform.position, areaRadius, bloodMagicData.LayerMasktoSearch);
    }
    /// <summary>
    /// AbilityController'da yetene�in bir aktive edilmesinde kullan�l�yor.
    /// </summary>
    public override void TriggerAbility()
    {
        if (bloodMagicData.isAbilityActive)
        {
            bloodMagicCoroutine = StartCoroutine(UseAbility(bloodMagicData.firstAbilityDelay, bloodMagicData.abilityCooldown, bloodMagicData.abilityDamage));
        }
    }

    /// <summary>
    ///     Yetene�in uygulnd��� b�l�md�r. ��erisinde kullan�lan b�t�n bilgiler Blood Magic'in Scriptable object'inden sa�lanmaktad�r.
    ///     �lk uygulanma gecikmesi bulunmaktad�r. �lk ba�ta bir g�stermelik alan ortaya ��k�yor. 
    ///     Ard�ndan de�i�tirilebilir(10 saniye) bir s�re boyunca saniyede 1 kez belli bir yar��ptaki(10.0f) 8.Layer'a ait nesneleri tar�yor ve 
    ///     bulunan her nesneye(temel olarak d��mana) hasar veriyor, verilen hasar�n bir maktar� iyile�me olarak geri d�n�yor.
    ///     ContinuousAbilityEnable isminde bool de�i�ken ile istenirse s�rekli veya bir kerelik etki sa�lanabilir.
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
                        Debug.Log("�al���yor: " + enemy.name);
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
                        Debug.Log("�al���yor");
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
    ///     �yile�me yetene�i i�in kullan�lan metottur. �imdilik bir can bar kontrol� i�in script olmad��� i�in �zel olarak sahnedeki helath bar�n
    ///     ilk child nesnesinin alt�ndaki child'da karakterin can bar�n� temsil eden Image var. Bu Image componentinin fill de�erini de�erini de�i�rtiryor
    /// </summary>
    /// <param name="rawHealing">Ham iyile�me miktar� bunun �zerinden hesplama yap�lmaktad�r. �rne�in d��mana verilen hasar ham iyile�me miktar�d�r.
    /// Bunun �zerinden i�lem yap�lmal�, mesela istenirse %50'si al�nabilir.
    /// </param>
    /// <param name="healingRatio">�yile�me oran�d�r. Varsay�lan de�eri 1'dir.  Ham iyile�me miktar�n�n ne kadar�n�n iyile�me olaca��n� belirler</param>
    /// <param name="targetGameObject">�imdilik Healthbar�n kendisini �zel olarak tutan bir parametredir. Ama esas amac� iyile�tirmenin hedefini belirlemk</param>
    
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
