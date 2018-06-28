using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFar : ConditionBase {

    public float farDistance;
    public Transform playerTransform;

    private void Awake()
    {
        if(playerTransform == null) ////Nullcheck om errors te verwijderen in AI vs AI
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
    }

    public override bool Condition()
    {
        if(playerTransform != null) ////Nullcheck om errors te verwijderen in AI vs AI
        {
            if (Vector3.Distance(transform.position, playerTransform.position) < farDistance)
            {
                evaluation = true;
            }
            else evaluation = false;
        }
        
        return evaluation;
    }
}
