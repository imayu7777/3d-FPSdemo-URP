using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("属性")]
    public float maxHealth;
    public float currentHealth;

    [Header("无敌帧")]
    public float invulnerableDuration;
    public bool invulnerable;

    [Header("事件")]
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
    // 无敌帧
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
