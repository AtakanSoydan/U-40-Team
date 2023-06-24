using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class DenemeAbility2 : AMagicAbility, IAreaAbilityy, IContinousAbilityy, IHealAbilityy
{

    public float continuousAbilityDuration=1.0f;
    public float continuousAbilityRepetitions=10.0f;
    public bool continuousAbilityEnable=true;

    public float areaRadius=21.0f;
    public Collider[] searchedArea;
    public int layerMasktoSearch= 1 << 6;
    [SerializeField] private int maxSearchedColliderCount = 50;

    public bool healingEnabled=true;
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

    public bool HealingEnabled { get => healingEnabled; set => healingEnabled =value; }
    public float HealRate { get => healRate; set => healRate=value; }

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
    void Update()
    {

    }


    public void TriggerAbility()
    {
        if (isAbilityActive)
        {
            StartCoroutine(UseAbility(firstAbilityDelay, abilityCooldown, abilityDamage));
        }
    }

    public IEnumerator UseAbility(float firstAbilityDelay, float abilityCooldown, float abilityDamage)
    {
        yield return new WaitForSeconds(firstAbilityDelay);
        GetAbilityToTargetPositionChild(_gameObject, player.gameObject);
        ShowArea(AreaRadius, _gameObject);
        int showDuration = 50;
        while (showDuration > 0)
        {
            yield return new WaitForSeconds(0.02f);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                showDuration = 0;

            }
            showDuration--;
        }
        float tempContinuousAbilityRepetitions = ContinuousAbilityRepetitions;
        int colliderCount;
        EnemyHealthBar enemyHealthBar;
        EnemyCharacter enemyCharacter;
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
                    enemyHealthBar.DamageApply(a);
                    enemyHealthBar.healthText.text = enemyHealthBar.BarValueAsIntegerDisplay(enemyHealthBar.healthbar);
                }
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

        GetAbilityToTargetPositionChild(_gameObject, this.gameObject);
        yield return new WaitForSeconds(abilityCooldown);

    }

    public float Healing(float rawHealing, float healingRatio, float healigMultiplier = 0)
    {
        throw new System.NotImplementedException();
    }
}
