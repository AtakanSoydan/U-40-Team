using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class BloodMagicAbility : AMagicAbility, IAreaAbility, IHealingAbility, IContinousAbility
{
    public BloodMagicData bloodMagicData;
    public Coroutine bloodMagicCoroutine;
    public GameObject _dummyArea;
    public GameObject _healingTarget;
    public HealthBar healthBar;
    public Image health;
    int targetLayerMask = 1 << 8;

    // Start is called before the first frame update
    void Start()
    {
        bloodMagicData.LayerMasktoSearch = targetLayerMask;
        bloodMagicData.healingTarget = _healingTarget;
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
    public GameObject ShowArea(float areaRadius, GameObject dummyarea, float areaAlpha)
    {

        dummyarea.transform.localScale.Set(areaRadius, areaRadius, dummyarea.transform.localScale.z);
        SpriteRenderer spriteRenderer = dummyarea.GetComponent<SpriteRenderer>();
        CircleCollider2D circleCollider2D = dummyarea.GetComponent<CircleCollider2D>();
        circleCollider2D.enabled = true;
        spriteRenderer.enabled = true;
        Color spriteColor = spriteRenderer.color;
        spriteColor.a = areaAlpha;
        return dummyarea;

    }

    public GameObject ShowGhostArea(float areaRadius, GameObject dummyarea, float areaAlpha)
    {

        dummyarea.transform.localScale.Set(areaRadius, areaRadius, dummyarea.transform.localScale.z);
        SpriteRenderer spriteRenderer = dummyarea.GetComponent<SpriteRenderer>();
        CircleCollider2D circleCollider2D = dummyarea.GetComponent<CircleCollider2D>();
        circleCollider2D.enabled = true;
        spriteRenderer.enabled = true;
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

        bloodMagicCoroutine = StartCoroutine(UseAbility(bloodMagicData.firstAbilityDelay, bloodMagicData.abilityCooldown, bloodMagicData.abilityDamage));


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
        ShowArea(bloodMagicData.AreaRadius, _dummyArea, 0.2f);
        yield return new WaitForSeconds(bloodMagicData.ContinuousAbilityDuration);
        SpriteRenderer spriteRenderer2 = _dummyArea.GetComponent<SpriteRenderer>();
        CircleCollider2D circleCollider2D = _dummyArea.GetComponent<CircleCollider2D>();
        circleCollider2D.enabled = false;
        spriteRenderer2.enabled = false;
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
        for (int i = 0; i < 100; i++)
        {
            health.fillAmount += (rawHealing * healingRatio) / 100;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log(collision.name);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    public override void UpdateAbility(float firstAbilityDelay, float abilityCooldown, float abilityDamage, int requiredExp, int abilityLevel)
    {
        throw new System.NotImplementedException();
    }

    public override void SaveAbilityExpToScirptable(int abilityExp)
    {
        throw new System.NotImplementedException();
    }



    public float CalculateDamage(float abilityDamage, float characterAttackDamage, float armor)
    {
        float calculatedDamage = characterAttackDamage * (1 + (abilityDamage / 100));

        return calculatedDamage;
    }
    public float DamageWithCritic(float criticRatio, float calculatedDamage)
    {
        if (UnityEngine.Random.Range(0.0f, 100.0f) < criticRatio)
        {
            return calculatedDamage *= (1 + (criticRatio / 100));
        }
        else
        {
            return calculatedDamage;
        }

    }
    public float def(int characterLevel, int enemyLevel, int enemyArmor)
    {
        float def = (100 + characterLevel) / ((100 + characterLevel) + (100 + enemyLevel) * (1 + enemyArmor / 100));
        return def;
    }


}
