﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClose : ConditionBase {

    public float closeDistance;
    public Transform playerTransform;

    private void Awake()
    {
        if (playerTransform == null) ////Nullcheck om errors te verwijderen in AI vs AI
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
            
    }

    public override bool Condition()
    {
        if (playerTransform != null) ////Nullcheck om errors te verwijderen in AI vs AI
        {
            if (Vector3.Distance(transform.position, playerTransform.position) < closeDistance)
            {
                evaluation = true;
            }
            else evaluation = false;
        }
        return evaluation;
    }
}