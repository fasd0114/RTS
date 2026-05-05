using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FactionComponent))]
public class Health : MonoBehaviour
{
    [Header("체력 설정")]
    public int maxHealth = 100;
    int currentHealth;

    bool isDead = false;

    [Header("피격")]
    public UnityEvent onDamaged;   // 피해 시
    public UnityEvent onDeath;     // 사망 시

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead || amount <= 0) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        onDamaged?.Invoke();

        if (currentHealth == 0)
            Die();
    }
    void Die()
    {
        if (isDead) return;
        isDead = true;

        onDeath?.Invoke();
        Destroy(gameObject);
    }
    public int GetCurrentHealth() => currentHealth;
}
