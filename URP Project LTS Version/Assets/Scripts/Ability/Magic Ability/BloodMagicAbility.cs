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
    ///     Göstermelik alaný aktive eden metot
    /// </summary>
    /// <param name="areaRadius"> Alanýn yarýçapý </param>
    /// <param name="dummyarea"> Gösterilecek sahte alan </param>
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

        bloodMagicCoroutine = StartCoroutine(UseAbility(bloodMagicData.firstAbilityDelay, bloodMagicData.abilityCooldown, bloodMagicData.abilityDamage));


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
        ShowArea(bloodMagicData.AreaRadius, _dummyArea, 0.2f);
        yield return new WaitForSeconds(bloodMagicData.ContinuousAbilityDuration);
        SpriteRenderer spriteRenderer2 = _dummyArea.GetComponent<SpriteRenderer>();
        CircleCollider2D circleCollider2D = _dummyArea.GetComponent<CircleCollider2D>();
        circleCollider2D.enabled = false;
        spriteRenderer2.enabled = false;
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
