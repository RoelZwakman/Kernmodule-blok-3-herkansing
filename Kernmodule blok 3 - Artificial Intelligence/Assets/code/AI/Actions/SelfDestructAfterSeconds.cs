using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructAfterSeconds : ActionBase
{
    public float damage;
    public float explosionRange;
    public float waitTime;

    public override void Action()
    {
        base.Action();
        Invoke("SelfDestruction", waitTime);
        Debug.Log("Initiating self destruction...");
    }

    private void SelfDestruction()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRange);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject != gameObject)
            {
                if (hitColliders[i].gameObject.tag == "Player" || hitColliders[i].gameObject.tag == "Enemy")
                {
                    hitColliders[i].gameObject.GetComponent<HealthSystem>().TakeDamage(damage);
                }
            }
        }
        Debug.Log("Destroyed self...");
        HealthSystem myHealth = GetComponent<HealthSystem>();
        myHealth.TakeDamage(myHealth.maxHealth);
        
    }
}
