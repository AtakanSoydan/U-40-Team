using UnityEngine;

public class ObjectTransparency : MonoBehaviour
{
    public Transform player; // Oyuncunun Transform bileþeni
    public float fadeSpeed;
    public float fadeAmount;

    private void Update()
    {
        // Kameranýn konumu ve oyuncunun konumu arasýnda bir vektör oluþtur
        Vector3 cameraToPlayer = player.position - Camera.main.transform.position;

        // Kameranýn baktýðý yöne doðru bir ray oluþtur
        Ray ray = new Ray(Camera.main.transform.position, cameraToPlayer);

        RaycastHit[] hits; // Raycast sonuçlarýný depolamak için bir dizi oluþtur
        hits = Physics.RaycastAll(ray, cameraToPlayer.magnitude); // Raycast'i kullanarak objeleri kontrol et

        // Tüm raycast sonuçlarýný dön
        foreach (RaycastHit hit in hits)
        {
            // Eðer raycast sonucunda bir obje bulunduysa ve obje bir Renderer bileþeni içeriyorsa
            if (hit.collider != null && hit.collider.GetComponent<Renderer>() != null)
            {
                Renderer renderer = hit.collider.GetComponent<Renderer>();

                // Materyalin renk deðerini al
                Color materialColor = renderer.material.color;

                // Materyalin alpha deðerini, fadeSpeed ve fadeAmount kullanarak zamanla deðiþtir
                float targetAlpha = Mathf.Lerp(materialColor.a, fadeAmount, fadeSpeed * Time.deltaTime);
                materialColor.a = targetAlpha;
                
                // Materyale güncellenmiþ alpha deðerini uygula
                renderer.material.color = materialColor;
            }
        }
    }
}
