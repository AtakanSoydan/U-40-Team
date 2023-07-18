using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.VFX;

public class ArrowRain : AMagicAbility, IAreaAbilityy, IContinousAbilityy, IHealAbilityy, IMoveAbleArea
{
    [Header("BloodMagicData")]
    //public float maxRate = 1.0f;
    public float healRate = 0.4f;
    public GameObject healingTarget;
    public VisualEffect vfx;
    public GameObject vfxParentObject;


    [SerializeField] private float areaRadius = 21.0f;
    [SerializeField] private Collider[] searchedArea;
    [SerializeField] private int maxSearchedColliderCount = 50;
    [SerializeField] private int layerMasktoSearch = 1 << 6;


    [SerializeField] private float continuousAbilityDuration = 1.0f;
    [SerializeField] private float continuousAbilityRepetitions = 10.0f;
    /// <summary>
    /// Bu yetenekte aslýnda kullanýlmýyor
    /// </summary>
    [SerializeField] private bool continuousAbilityEnable = true;
    [SerializeField] private bool healingEnabled = true;

    public float HealRate { get => healRate; set => healRate = value; }

    public float AreaRadius { get => areaRadius; set => areaRadius = value; }
    public bool CanMove { get => canMove; set => canMove = value; }
    public RaycastHit[] HitInfo { get => hitInfo; set { hitInfo = value; } }
    public Ray Ray { get => ray; set => ray = value; }
    public Camera ActiveCamera { get => activeCamera; set => activeCamera = value; }
    public Collider[] SearchedArea
    {
        get => searchedArea;
        set
        {
            searchedArea = value;
        }
    }
    public int LayerMasktoSearch
    {
        get => layerMasktoSearch;
        set { layerMasktoSearch = value; }
    }

    public float ContinuousAbilityDuration { get => continuousAbilityDuration; set => continuousAbilityDuration = value; }
    public float ContinuousAbilityRepetitions { get => continuousAbilityRepetitions; set => continuousAbilityRepetitions = value; }
    public bool HealingEnabled { get => healingEnabled; set => healingEnabled = value; }
    /// <summary>
    /// Bu yetenekte aslýnda kullanýlmýyor
    /// </summary>
    public bool ContinuousAbilityEnable { get => continuousAbilityEnable; set => continuousAbilityEnable = value; }

    public ABaseCharacter player;
    public GameObject _dummyArea;
    public Vector3 tempDummyArea;
    public int colliderCount;

    public ABaseCharacter enemyCharacter;
    public ABaseHealthBar enemyHealthBar;
    public Collider[] colliders;
    [SerializeField] Coroutine denemeAbilityCor;

    //public GameObject denemeArea;
    //public Transform denemeAreaParentTransform;
    public bool canMove = false;
    public short moveDuration = 250;
    [SerializeField] public RaycastHit[] hitInfo;
    [SerializeField] public Ray ray;
    public Camera activeCamera;
    public int groundLayer = 1 << 7;
    public Vector3 areaPos;
    public Vector3 denPos;

    private void Start()
    {
        isAbilityActive = true;
        searchedArea = new Collider[maxSearchedColliderCount];
        vfx = GetComponentInChildren<VisualEffect>();
        vfxParentObject = vfx.transform.parent.gameObject;
        vfxParentObject.SetActive(false);

        colliders = new Collider[10];
        hitInfo = new RaycastHit[10];
        tempDummyArea = Vector3.zero;


    }

    void Update()
    {
        CancelTime(_dummyArea, ref showDuration, denemeAbilityCor);
        /*
        if (!canceled && canMove)
        {
            MoveArea(_dummyArea);
        }*/
        MoveArea(_dummyArea);

        //Debug.Log("radius: " + vfx.GetFloat("Radius"));
    }
    /*
    private void LateUpdate()
    {
        MoveArea(_dummyArea);
    }
    */

    /// <summary>
    ///     Göstermelik alaný aktive eden metot
    /// </summary>
    /// <param name="areaRadius"> Alanýn yarýçapý </param>
    /// <param name="dummyarea"> Gösterilecek sahte alan </param>
    /// <returns></returns>
    public GameObject ShowArea(float areaRadius, GameObject dummyarea, float areaAlpha = 60.0f)
    {
        tempDummyArea.Set(2 * areaRadius, 2 * areaRadius, 2 * areaRadius);
        dummyarea.transform.localScale = tempDummyArea;
        //dummyarea.transform.lossyScale = tempDummyArea;
        /*
        Material dummyareaMaterial = dummyarea.GetComponent<Renderer>().material;
        dummyareaMaterial.SetFloat("_Mode", 3);
        dummyareaMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        dummyareaMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        dummyareaMaterial.SetInt("_ZWrite", 0);
        Color tempColor = dummyareaMaterial.color;
        tempColor.a = 0.2f;
        dummyareaMaterial.color = tempColor;
        Debug.Log(dummyareaMaterial.color);
        */
        dummyarea.SetActive(true);
        return dummyarea;

    }

    public void MoveArea(GameObject denemeArea)
    {
        if (canMove && !canceled)
        {
            Debug.Log("Ray çalýþýyor");
            ray = activeCamera.ScreenPointToRay(Input.mousePosition);

            Debug.Log($"ray: {ray}");
            int results = Physics.RaycastNonAlloc(ray, hitInfo, 300f, groundLayer);
            float smallestDistance = 300;
            int tempIndex = 0;
            for (int i = 0; i < results; i++)
            {

                if (smallestDistance > hitInfo[i].distance)
                {
                    smallestDistance = hitInfo[i].distance;
                    tempIndex = i;
                }
            }
            Debug.Log($"hitpoint: {hitInfo[tempIndex].point}");
            //Debug.Log($"deneme: {denemeArea.position}");
            if (results > 0)
            {
                denPos = hitInfo[tempIndex].point;
                //denemeArea.transform.position = Vector3.Lerp(denemeArea.transform.position, denPos, Time.deltaTime * 100);
                denemeArea.transform.position = denPos;
            }

        }


    }


    /*
    public void MoveToOriginalPosition(GameObject abilityController, GameObject ability)
    {
        ability.transform.SetParent(abilityController.transform);
        ability.transform.localPosition = Vector3.zero;
    }
    */

    /// <summary>
    ///     BloodMagic yeteneði için bir yarýçapta çemberin içinde kalan belli bir Layer'ý(6.Layer) tarýyor.
    /// </summary>
    /// <param name="areaRadius"> Taranacak yarýçap </param>
    /// <param name="gameObject"> Taramanýn baþlatýlacaðý nokta </param>
    /// <returns></returns>
    public int SearchArea(float areaRadius, GameObject gameObject, Collider[] resultColliders, int layer)
    {

        return Physics.OverlapSphereNonAlloc(gameObject.transform.position, areaRadius, resultColliders, layer);
    }

    /// <summary>
    ///     Ýyileþme miktarýný hesaplamak için kullanýlan metottur.
    /// </summary>
    /// <param name="rawHealing">Ham iyileþme miktarý bunun üzerinden hesplama yapýlmaktadýr. Örneðin düþmana verilen hasar ham iyileþme miktarýdýr.
    /// Bunun üzerinden iþlem yapýlmalý, mesela istenirse %50'si alýnabilir.
    /// </param>
    /// <param name="healingRatio">Ýyileþme oranýdýr. Ham iyileþme miktarýnýn ne kadarýnýn iyileþme olacaðýný belirler</param>
    /// <param name="healigMultiplier">Ýyileþmeyi oransal olarak azaltýr veya arttýrabilir, -100 ile 100 arasýnda bir sayý olmalýdýr. Varsayýlan 0'dýr.</param>
    public float Healing(float rawHealing, float healingRatio, float healigMultiplier = 0.0f)
    {
        return rawHealing * (1 + healingRatio / 100) * (1 - healigMultiplier / 100);

    }

    public void TriggerAbility()
    {
        if (isAbilityActive)
        {
            denemeAbilityCor = StartCoroutine(UseAbility(firstAbilityDelay, abilityCooldown, abilityDamage));
        }
    }

    /// <summary>
    ///     Yeteneðin uygulandýðý bölümdür. Ýçerisinde kullanýlan bütün bilgiler DenemeAbility'den saðlanmaktadýr.
    ///     Ýlk uygulanma gecikmesi bulunmaktadýr. Ýlk baþta bir göstermelik alan ortaya çýkýyor. 
    ///     Ardýndan deðiþtirilebilir(10 saniye) bir süre boyunca saniyede 1 kez belli bir yarýçptaki(10.0f) 6.Layer'a ait nesneleri tarýyor ve 
    ///     bulunan her nesneye(temel olarak düþmana) hasar veriyor, verilen hasarýn bir maktarý iyileþme olarak geri dönüyor.
    /// </summary>
    /// <returns></returns>
    public IEnumerator UseAbility(float firstAbilityDelay, float abilityCooldown, float abilityDamage)
    {
        while (isActiveted)
        {
            isActiveted = false;
            isAbilityActive = false;
            canMove = true;
            canceled = false;
            showDuration = 50.0f;
            yield return new WaitForSeconds(firstAbilityDelay);
            canCancelTime = true;
            ShowArea(AreaRadius, _dummyArea);
            /*
            short tempMoveDuration = moveDuration;
            while (canMove && moveDuration > 0)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    canMove = false;

                }

                yield return new WaitForSeconds(0.02f);
                moveDuration--;
            }
            moveDuration = tempMoveDuration;
            */
            while (canCancelTime)
            {
                Debug.Log("canCancelTime devam ediyor2");
                yield return null;
            }
            canMove = false;
            float tempContinuousAbilityRepetitions = ContinuousAbilityRepetitions;
            vfxParentObject.transform.SetPositionAndRotation(_gameObject.transform.position, Quaternion.identity);
            vfx.SetFloat("Radius", AreaRadius);
            vfx.SetFloat("LifeTime", tempContinuousAbilityRepetitions * ContinuousAbilityDuration);
            Debug.Log(vfx.GetFloat("LifeTime"));
            vfxParentObject.SetActive(true);
            if (HealingEnabled)
            {
                while (tempContinuousAbilityRepetitions > 0)
                {

                    colliderCount = SearchArea(AreaRadius, _gameObject, SearchedArea, LayerMasktoSearch);
                    //colliderCount = Physics.OverlapSphereNonAlloc(bloodMagicData._gameObject.transform.position, bloodMagicData.AreaRadius, colliders, bloodMagicData.LayerMasktoSearch);
                    for (int i = 0; i < colliderCount; i++)
                    {
                        enemyHealthBar = SearchedArea[i].GetComponentInChildren<EnemyHealthBar>();
                        enemyCharacter = SearchedArea[i].GetComponent<EnemyCharacter>();
                        float a = player.CalculateDamage(characterAttackDamage: player.AttackPower,
                                                    baseAbilityDamage: abilityDamage,
                                                    abilityDamageRatio,
                                                    enemyCharacter.Armor,
                                                    player.criticRatio,
                                                    player.Level,
                                                    enemyCharacter.Level,
                                                    maxArmorHalf: enemyHealthBar.characterMaxHealth.maxArmor);
                        Debug.Log(a);
                        Debug.Log(enemyCharacter.Level);
                        Debug.Log((1 + (player.Level / 200) - (enemyCharacter.Level / 200)));
                        enemyHealthBar.DamageApply(a, player.isCritic);
                        enemyHealthBar.healthText.text = enemyHealthBar.BarValueAsIntegerDisplay(enemyHealthBar.healthbar);
                    }
                    /*
                    SearchArea(bloodMagicData.AreaRadius, bloodMagicData._gameObject, bloodMagicData.SearchedArea, bloodMagicData.LayerMasktoSearch);
                    foreach (Collider enemy in bloodMagicData.SearchedArea)
                    {
                        Debug.Log("çalýþýyor: " + enemy.name);
                        enemyCharacter = enemy.GetComponent<ABaseCharacter>();
                        enemyHealthBar = enemy.GetComponentInChildren<EnemyHealthBar>();
                        float a = CalculateDamage(AttackPower, abilityDamage, bloodMagicData.abilityDamageRatio, enemyCharacter.Armor, criticRatio, Level, enemyCharacter.Level);
                        Debug.Log(a);
                        enemyHealthBar.Damage(a);
                        enemyHealthBar.healthText.text = enemyHealthBar.BarValueAsIntegerDisplay(enemyHealthBar.healthbar);
                        //enemy.gameObject.GetComponentInChildren<EnemyHealth>()?.TakeDamage(abilityDamage);
                        //yield return StartCoroutine(Healingg(abilityDamage, healthBar.gameObject, bloodMagicData.healRate));

                    }*/
                    tempContinuousAbilityRepetitions--;
                    yield return new WaitForSeconds(ContinuousAbilityDuration);

                }
                _dummyArea.SetActive(false);
            }
            else
            {
                while (tempContinuousAbilityRepetitions > 0)
                {
                    SearchArea(AreaRadius, _gameObject, SearchedArea, LayerMasktoSearch);
                    //bloodMagicData.SearchedArea = SearchArea(bloodMagicData.AreaRadius, bloodMagicData._gameObject);
                    foreach (Collider enemy in SearchedArea)
                    {
                        Debug.Log("çalýþýyor");
                        //enemy.gameObject.GetComponentInChildren<EnemyHealth>()?.TakeDamage(abilityDamage);

                    }
                    tempContinuousAbilityRepetitions--;
                    yield return new WaitForSeconds(ContinuousAbilityDuration);

                }
                _dummyArea.SetActive(false);
            }
            tempContinuousAbilityRepetitions = ContinuousAbilityRepetitions;

            yield return new WaitForSeconds(abilityCooldown);
            vfxParentObject.SetActive(false);
        }
        isActiveted = true;
        isAbilityActive = true;
    }


}

