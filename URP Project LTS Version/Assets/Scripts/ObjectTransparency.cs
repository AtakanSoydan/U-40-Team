using UnityEngine;

public class ObjectTransparency : MonoBehaviour
{
    public Transform player; // Oyuncunun Transform bile�eni
    public float fadeSpeed;
    public float fadeAmount;

    private void Update()
    {
        // Kameran�n konumu ve oyuncunun konumu aras�nda bir vekt�r olu�tur
        Vector3 cameraToPlayer = player.position - Camera.main.transform.position;

        // Kameran�n bakt��� y�ne do�ru bir ray olu�tur
        Ray ray = new Ray(Camera.main.transform.position, cameraToPlayer);

        RaycastHit[] hits; // Raycast sonu�lar�n� depolamak i�in bir dizi olu�tur
        hits = Physics.RaycastAll(ray, cameraToPlayer.magnitude); // Raycast'i kullanarak objeleri kontrol et

        // T�m raycast sonu�lar�n� d�n
        foreach (RaycastHit hit in hits)
        {
            // E�er raycast sonucunda bir obje bulunduysa ve obje bir Renderer bile�eni i�eriyorsa
            if (hit.collider != null && hit.collider.GetComponent<Renderer>() != null)
            {
                Renderer renderer = hit.collider.GetComponent<Renderer>();

                // Materyalin renk de�erini al
                Color materialColor = renderer.material.color;

                // Materyalin alpha de�erini, fadeSpeed ve fadeAmount kullanarak zamanla de�i�tir
                float targetAlpha = Mathf.Lerp(materialColor.a, fadeAmount, fadeSpeed * Time.deltaTime);
                materialColor.a = targetAlpha;
                
                // Materyale g�ncellenmi� alpha de�erini uygula
                renderer.material.color = materialColor;
            }
        }
    }
}
