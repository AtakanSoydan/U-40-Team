using UnityEngine;

public class EnemyHealth : HealthBase, ITakeDamage
{
    [SerializeField] private HealthBar _healthBar;

    //[SerializeField] private ShowDamage _showDamage;

    private void Start()
    {
        _healthBar.UpdateHealthBar(MaxHealth, CurrentHealth);
    }

    public void TakeDamage(float damage)
    {
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            CurrentHealth -= damage;
            _healthBar.UpdateHealthBar(MaxHealth, CurrentHealth);
            /*
            if (_showDamage)
            {
                _showDamage.Execute(damage);
            }
            */
        }
    }
}
