using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DamageDisplay : MonoBehaviour, IGetBound
{
    public GameObject damageTextGameObject;
    public TextMeshProUGUI damageText;
    public Canvas canvas;
    public ABaseHealthBar enemyBaseHealthBar;

    public float damageTextDurationTime;

    public TextMeshPro meshPro;
    public TMP_Text meshProText;

    [SerializeField] public Transform cameraTransform;
    public Quaternion textCameraLookQuaternion;
    Vector3 barLookDirection;

    public GameObject deneme;
    Vector3 size;
    public RectTransform rectTransform;
    TextMeshPro instantiatedText;
    public Queue<TextMeshPro> textProQueue;
    public TextMeshPro textMesh;
    public float duration;
    int textProQueueCount=0;
    public bool canDisappear = false;
    public bool canGetDamageText=false;
    public Vector3 tempDamageTextPosition;
    private float randomX;
    private float randomY;
    private float randomZ;
    private float delay=0.5f;
    private Vector3 randomVector3;
    public GameObject[] destroyDaamageTextArray;
    private Bounds playerColliderBounds;

    public Bounds GetBounds { get => playerColliderBounds; }

    //public GameObject den2;
    // Start is called before the first frame update
    void Start()
    {


        Collider denemeCollider = deneme.GetComponent<Collider>();
        playerColliderBounds = denemeCollider.bounds;
        size = denemeCollider.bounds.size;
        Debug.Log($"size: {size}, size.x: {size.x}");
        /*
        rectTransform = GetComponent<RectTransform>();
        Debug.Log($"sizedelta : {rectTransform.sizeDelta}");
        ChangeWidthAndHeight(size, ref rectTransform);
        rectTransform.position = deneme.transform.position;
        */


        meshPro.rectTransform.position = deneme.transform.position;
        meshProText = meshPro;
        rectTransform = meshProText.rectTransform;
        ChangeWidthAndHeight(size, ref rectTransform);
        //meshPro.transform.position += Vector3.one;

        //Stack<TextMeshPro> textProStack2;

        textProQueue = new Queue<TextMeshPro>(50);
        randomVector3 = new Vector3();

        /*
        //rectTransform.position = denemeCollider.bounds.min+denemeCollider.bounds.extents;
        //rectTransform.position = denemeCollider.bounds.min;
        Vector3 tempMin = new Vector3(rectTransform.localPosition.x, denemeCollider.bounds.min.y, rectTransform.localPosition.z);
        Vector3 tempMin2 = new Vector3(rectTransform.position.x, denemeCollider.bounds.min.y, rectTransform.position.z);
        //rectTransform.localPosition = tempMin;
        //rectTransform.position = tempMin;
        //ChangeWidthAndHeight(size, ref rectTransform);
        Vector3[] arr = new Vector3[4];
        // Vector2 center = rectTransform.rect.center;

        Vector2 tempp = new Vector2(rectTransform.rect.center.x , rectTransform.rect.center.y + rectTransform.rect.height);
        Vector2 tempp2 = new Vector2(rectTransform.rect.center.x, denemeCollider.bounds.min.y);

        //rectTransform.rect.center = tempp;
        rectTransform.GetWorldCorners(arr);
        //arr[0].Set()

        */
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {


            canGetDamageText = true;
                /*
                for (int i = 0; i < textProQueue.Count; i++)
                {
                    TextMeshPro textMesh = textProQueue.Dequeue();
                    while (duration<1)
                    {
                        Vector3 temp = Vector3.Lerp(textMesh.transform.position, textMesh.transform.position + size / 2, duration);
                        textMesh.transform.position = temp;
                        textMesh.alpha = 1-duration;
                        duration += Time.deltaTime;
                        
                    }
                    Destroy(textMesh);
                }
                */
                //textProQueue.Dequeue().transform.position += size / 2;
           
            
        }


        if (canGetDamageText && textProQueue.Count > 0)
        {
            randomX = Random.Range(-size.x, size.x);
            randomY = Random.Range(-size.y, size.y);
            randomZ = Random.Range(-size.z, size.z);
            randomVector3.Set(randomX, randomY, randomZ);
            GetFirstDamageText();
        }


    }
    private void LateUpdate()
    {
        barLookDirection = gameObject.transform.position - cameraTransform.position;
        textCameraLookQuaternion = Quaternion.LookRotation(barLookDirection);
        //gameObject.transform.parent.rotation = textCameraLookQuaternion;
        gameObject.transform.rotation = textCameraLookQuaternion;

        //damageTextGameObject.transform.position = deneme.transform.position;
    }
    private void OnEnable()
    {
        enemyBaseHealthBar.OnDamaged += EnemyBaseHealthBar_OnDamaged;
    }



    private void OnDisable()
    {
        enemyBaseHealthBar.OnDamaged -= EnemyBaseHealthBar_OnDamaged;
    }

    private void EnemyBaseHealthBar_OnDamaged(object sender, System.EventArgs e)
    {
        /*
        damageText.gameObject.SetActive(true);
        //enemyBaseHealthBar.da
        */
        //string amont = e.DamageAmount.ToString();
        //enemyBaseHealthBar.damage.ToString();
        SpawnDamageText(enemyBaseHealthBar.BarValueAsIntegerDisplay(enemyBaseHealthBar.damage), enemyBaseHealthBar.wasCritic);
        Debug.Log(enemyBaseHealthBar.wasCritic);
        if (enemyBaseHealthBar.wasCritic)
        {
            Debug.Log("criticc");
        }


    }

    public TextMeshPro InstantiateText()
    {
        float randomX = Random.Range(-size.x / 2, size.x / 2);
        float randomY = Random.Range(-size.y / 2, size.y / 2);
        //TextMeshPro instantiatedText = Instantiate(meshPro, deneme.transform);
        Vector3 instantiatePosition = new Vector3(deneme.transform.position.x + randomX, deneme.transform.position.y + randomY, deneme.transform.position.z);

        return Instantiate(meshPro, instantiatePosition, this.transform.rotation, this.transform);
    }

    public void SpawnDamageText(string damageAmount, bool isCritic=false)
    {
        instantiatedText = InstantiateText();
        instantiatedText.text = damageAmount;
        if (isCritic)
        {
            instantiatedText.color = Color.yellow;
            instantiatedText.fontSize += instantiatedText.fontSize / 2;
            //instantiatedText.fontStyle = FontStyles.Bold;
            instantiatedText.fontWeight = FontWeight.Black;
        }
        else { instantiatedText.color = Color.white; }
        textProQueue.Enqueue(instantiatedText);
        Debug.Log($"textProQueue: {textProQueue.Count}, {textProQueue}");

        //Destroy(instantiatedText, 1f);
    }
    public void GetFirstDamageText()
    {
        textMesh = textProQueue.Dequeue();
        tempDamageTextPosition = textMesh.transform.localPosition;
        canGetDamageText = false;
        canDisappear = true;
    }

    public void Disappear()
    {
        /*
        Debug.Log(textProQueue.Count);
        if (delay>0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            if (duration < 1)
            {
                textMesh.transform.localPosition = Vector3.Lerp(tempDamageTextPosition, tempDamageTextPosition + (randomVector3), duration);
                textMesh.alpha = 1 - duration;
                duration += Time.deltaTime;
                Debug.Log(size / 2);
                Debug.Log("Tesxtmesh position" + textMesh.transform.position);
                Debug.Log("Tesxtmesh position2" + textMesh.transform.position + (size / 2));
            }
            else
            {
                Debug.Log("dissap çalýþtý");
                Destroy(textMesh.gameObject);

                duration = 0;
                canGetDamageText = true;
                canDisappear = false;
                delay = 1.0f;
            }
        }
        */
        if (duration <= 1)
        {
            textMesh.transform.localPosition = Vector3.Lerp(tempDamageTextPosition, tempDamageTextPosition + (randomVector3), duration);
            textMesh.alpha = 1 - duration;
            duration += Time.deltaTime;
            Debug.Log(size / 2);
            Debug.Log("Tesxtmesh position" + textMesh.transform.position);
            Debug.Log("Tesxtmesh position2" + textMesh.transform.position + (size / 2));
        }
        else if(duration > 1)
        {
            Debug.Log("dissap çalýþtý");

            Destroy(textMesh.gameObject);

            duration = 0.0f;
            canGetDamageText = true;
            canDisappear = false;

        }
    }

    public void ChangeWidthAndHeight(Vector3 size, ref RectTransform rectTransform)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x * 2);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y * 2);
        //rectTransform.position = deneme.transform.position;
    }
}
