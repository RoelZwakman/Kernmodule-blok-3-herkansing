using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CreatureType
{
    PLAYER = 0,
    ENEMY = 1
}

public class HealthSystem : MonoBehaviour {

    public CreatureType creatureType;
    public GameObject deathEffects;

    public float maxHealth;
    public float health;

    public UnityAction OnDeath;

    private void Awake()
    {
        HealthSetup();
    }

    private void HealthSetup()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            SpawnDeathEffects();
            if(GameManager.instance.generator != null) ////Nullcheck om errors te verwijderen in AI vs AI
            {
                OnDeath();
            }
            

            switch (creatureType) ////Does 
            {
                case CreatureType.PLAYER:
                    GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().enabled = false; /////Turns off the player controller on death.
                    break;

                case CreatureType.ENEMY:
                    Destroy(gameObject);
                    break;
            }
            
        }
    }

    public void AddHealth(float amount)
    {
        health += amount;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private void SpawnDeathEffects()
    {
        Instantiate(deathEffects, transform.position, Quaternion.identity);
    }



}
