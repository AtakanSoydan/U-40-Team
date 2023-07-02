using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ABaseAbilityClass : MonoBehaviour
{
    [Header("ABaseAbilityData")]
    public float firstAbilityDelay = 0.1f;
    public float abilityCooldown = 5.0f;
    public float abilityDamage = 50.0f;
    public float abilityDamageRatio = 20f;
    public string abilityName = "Yetenek �smi";
    public string abilityDescription = "Yetene�in a��klanmas�";
    //public Transform startPoint;
    public bool isAbilityActive = true;

    public int abilityLevel = 1;
    public int abilityExp = 0;
    public int requiredExp = 100;


    public int AbilityLevel { get => abilityLevel; set => abilityLevel = value; }
    public int AbilityExp { get => abilityExp; set => abilityExp = value; }
    public int RequiredExp { get => requiredExp; set => requiredExp = value; }

    [SerializeField] public GameObject _gameObject;
    public bool isActiveted = true;


    public bool canCancelTime = false;
    public bool canceled = false;
    public float showDuration;

    public void GetAbilityToTargetPositionChild(GameObject ability, GameObject parent)
    {

        ability.transform.SetParent(parent.transform,false);

    }


    /// <summary>
    /// Yetene�i iptal eder yetene�in uygulanaca�� alan� g�steren nesnesiyi eski yerine g�t�r�r. Karakterle birlikte hareket eden yetenekler i�in
    /// </summary>
    /// <param name="_dummyArea"></param>
    /// <param name="showDuration"></param>
    /// <param name="denemeAbiliyCor"></param>
    public void CancelTime(GameObject _dummyArea, ref float showDuration, Coroutine denemeAbiliyCor)
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
        */
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
                isActiveted = true;
                isAbilityActive = true;
                canceled = true;
                showDuration = 0;
                StopCoroutine(denemeAbiliyCor);
            }
            showDuration -= Time.deltaTime * 10;
            Debug.Log("Canceltime devammmm");
            if (showDuration <= 0)
            {
                showDuration = 0;
                canCancelTime = false;
            }
        }

    }

    /*

    public IEnumerator ShowDuration(short showDuration = 50, bool canMove=false, float repTime = 0.02f)
    {
       
        short tempMoveDuration = showDuration;
        while (canMove || showDuration > 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                canMove = false;
                yield break;
            }

            yield return new WaitForSeconds(repTime);
            showDuration--;
        }
        canMove = false;
        showDuration = tempMoveDuration;
        yield break;
    }
    */
}
