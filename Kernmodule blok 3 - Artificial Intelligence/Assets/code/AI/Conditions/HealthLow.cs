using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLow : ConditionBase {

    public HealthSystem healthSystem;
    public float lowHealth;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    public override bool Condition()
    {
        if(healthSystem.health <= lowHealth && healthSystem.health > 0)
        {
            evaluation = true;
        }
        else evaluation = false;
        return evaluation;
    }


}
