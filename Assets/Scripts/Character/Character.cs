using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("����")]
    public float maxHealth;
    public float currentHealth;

    [Header("�޵�֡")]
    public float invulnerableDuration;
    public bool invulnerable;

    [Header("�¼�")]
    public UnityEvent OnHurt;
    public UnityEvent OnDeath;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (invulnerable)
        {
            return;
        }
        if (currentHealth < damage)
        {
            Die();
        }
        else
        {
            currentHealth -= damage;
            StartCoroutine(nameof(InvulnerableCoroutine));
            OnHurt?.Invoke();
        }
    }
    // �޵�֡
    private IEnumerator InvulnerableCoroutine()
    {
        invulnerable = true;
        yield return new WaitForSeconds(invulnerableDuration);
        invulnerable = false;
    }
    public void Die()
    {
        currentHealth = 0;
        OnDeath?.Invoke();
    }

}
