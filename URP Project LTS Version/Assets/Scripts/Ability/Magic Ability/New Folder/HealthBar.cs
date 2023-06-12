using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;
    [SerializeField] private bool startDisable;

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        if (maxHealth == currentHealth && startDisable)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }

        //_healthbarSprite.DOFillAmount(currentHealth / maxHealth, 1);
    }


}
