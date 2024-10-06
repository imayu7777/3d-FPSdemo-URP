using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public Transform target;
    public float collidingDamage;   // Åö×²ÉËº¦
    public float chaseSpeed;
    public Vector3 chaseDir;

    private void FixedUpdate()
    {
        Chase();
    }
    void Chase()
    {
        if (target != null)
        {
            chaseDir = (target.position - transform.position).normalized;
            chaseDir.y = 0;
            transform.Translate(chaseSpeed * chaseDir * Time.fixedDeltaTime);
        }
    }
    private void OnColliderEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<Character>().TakeDamage(collidingDamage);
        }
    }
}
