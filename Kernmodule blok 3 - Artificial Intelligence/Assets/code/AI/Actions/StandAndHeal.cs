using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandAndHeal : ActionBase {

    public float waitTime;
    public float healAmount;
    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    public override void Action()
    {
        base.Action();
        StartCoroutine(IHealAfterSeconds());   
    }

    private IEnumerator IHealAfterSeconds()
    {
        yield return new WaitForSeconds(waitTime);
        healthSystem.AddHealth(healAmount);
    }

}
