using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DenemeAbility3 : AMagicAbility, IAreaAbilityy, IContinousAbilityy, IHealAbilityy
{

    public float continuousAbilityDuration = 1.0f;
    public float continuousAbilityRepetitions = 10.0f;
    public bool continuousAbilityEnable = true;

    public float areaRadius = 21.0f;
    public Collider[] searchedArea;
    public int layerMasktoSearch = 1 << 6;
    [SerializeField] private int maxSearchedColliderCount = 50;

    public bool healingEnabled = true;
    public float healRate;

    public GameObject _dummyArea;
    private Vector3 tempDummyArea;
    public ABaseCharacter player;


    public float ContinuousAbilityDuration { get => continuousAbilityDuration; set => continuousAbilityDuration = value; }
    public float ContinuousAbilityRepetitions { get => continuousAbilityRepetitions; set => continuousAbilityRepetitions = value; }
    public bool ContinuousAbilityEnable { get => continuousAbilityEnable; set => continuousAbilityEnable = value; }

    public float AreaRadius { get => areaRadius; set => areaRadius = value; }
    public Collider[] SearchedArea { get => searchedArea; set { searchedArea = value; } }
    public int LayerMasktoSearch { get => layerMasktoSearch; set => layerMasktoSearch = value; }

    public bool HealingEnabled { get => healingEnabled; set => healingEnabled = value; }
    public float HealRate { get => healRate; set => healRate = value; }

    public Coroutine denemeAbiliy3Cor;

    public int SearchArea(float areaRadius, GameObject gameObject, Collider[] resultColliders, int layer)
    {
        return Physics.OverlapSphereNonAlloc(gameObject.transform.position, areaRadius, resultColliders, layer);
    }

    public GameObject ShowArea(float areaRadius, GameObject dummyarea, float areaAlpha = 60)
    {

        tempDummyArea.Set(2 * areaRadius, 2 * areaRadius, 2 * areaRadius);
        dummyarea.transform.localScale = tempDummyArea;
        //dummyarea.transform.lossyScale = tempDummyArea;

        dummyarea.SetActive(true);
        return dummyarea;

    }

    // Start is called before the first frame update
    void Start()
    {
        tempDummyArea = Vector3.zero;
        isAbilityActive = true;
        searchedArea = new Collider[maxSearchedColliderCount];

    }


    // Update is called once per frame
    void LateUpdate()
    {
        CancelTime(_dummyArea, ref showDuration, denemeAbiliy3Cor);
        if (canceled)
        {
            GetAbilityToTargetPositionChild(_gameObject, this.gameObject);
        }
    }
    /*
    public void canceltime()
    {
        /*
        if (canCancelTime)
        {
            
            while (showDuration > 0)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    canCancelTime = false;
                    showDuration = 0;

                }
                showDuration -= Time.deltaTime*10;
            }
            canCancelTime = false;
        }
        /
        if (showDuration > 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                showDuration = 0;
                canCancelTime = false;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                _dummyArea.SetActive(false);
                GetAbilityToTargetPositionChild(_gameObject, this.gameObject);
                StopCoroutine(denemeAbiliy3Cor);
                isActiveted = true;
                isAbilityActive = true;
                showDuration = 0;
            }
            showDuration -= Time.deltaTime * 10;
            if (showDuration <= 0)
            {
                showDuration = 0;
                canCancelTime = false;
            }
        }

    }
    */

    public void TriggerAbility()
    {
        if (isAbilityActive)
        {
            denemeAbiliy3Cor = StartCoroutine(UseAbility(firstAbilityDelay, abilityCooldown, abilityDamage));
        }
    }

    public IEnumerator UseAbility(float firstAbilityDelay, float abilityCooldown, float abilityDamage)
    {
        canceled = false;
        isAbilityActive = false;
        showDuration = 50.0f;
        yield return new WaitForSeconds(firstAbilityDelay);
        canCancelTime = true;

        GetAbilityToTargetPositionChild(_gameObject, player.gameObject);
        ShowArea(AreaRadius, _gameObject);

        Debug.Log("canCancelTime devam ediyor");
        while (canCancelTime)
        {
            Debug.Log("canCancelTime devam ediyor222222");
            yield return null;
        }
        Debug.Log("canCancelTime bitti");
        float tempContinuousAbilityRepetitions = ContinuousAbilityRepetitions;
        int colliderCount;
        EnemyHealthBar enemyHealthBar;
        EnemyCharacter enemyCharacter;
        if (HealingEnabled)
        {
            Debug.Log("Yetenek devam ediyor");
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
                tempContinuousAbilityRepetitions--;
                yield return new WaitForSeconds(ContinuousAbilityDuration);

            }
            _dummyArea.SetActive(false);
        }
        else if(!HealingEnabled)
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

        GetAbilityToTargetPositionChild(_gameObject, this.gameObject);
        yield return new WaitForSeconds(abilityCooldown);

    isAbilityActive = true;
    }

    public float Healing(float rawHealing, float healingRatio, float healigMultiplier = 0)
    {
        throw new System.NotImplementedException();
    }
    /*
    public IEnumerator UseAbility(float firstAbilityDelay, float abilityCooldown, float abilityDamage)
    {
        while (isActiveted)
        {
            isActiveted = false;
            isAbilityActive = false;
            showDuration = 50.0f;
            yield return new WaitForSeconds(firstAbilityDelay);
            GetAbilityToTargetPositionChild(_gameObject, player.gameObject);
            ShowArea(AreaRadius, _gameObject);
            canCancelTime = true;
            Debug.Log("canCancelTime devam ediyor");
            while (canCancelTime)
            {
                Debug.Log("canCancelTime devam ediyor222222");
                yield return null;
            }
            Debug.Log("canCancelTime bitti");
            float tempContinuousAbilityRepetitions = ContinuousAbilityRepetitions;
            int colliderCount;
            EnemyHealthBar enemyHealthBar;
            EnemyCharacter enemyCharacter;
            if (HealingEnabled)
            {
                Debug.Log("Yetenek devam ediyor");
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
                        enemyHealthBar.DamageApply(a);
                        enemyHealthBar.healthText.text = enemyHealthBar.BarValueAsIntegerDisplay(enemyHealthBar.healthbar);
                    }
                    tempContinuousAbilityRepetitions--;
                    yield return new WaitForSeconds(ContinuousAbilityDuration);

                }
                _dummyArea.SetActive(false);
            }
            else if (!HealingEnabled)
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

            GetAbilityToTargetPositionChild(_gameObject, this.gameObject);
            yield return new WaitForSeconds(abilityCooldown);
        }
        isActiveted = true;
        isAbilityActive = true;
    }
    */
}
