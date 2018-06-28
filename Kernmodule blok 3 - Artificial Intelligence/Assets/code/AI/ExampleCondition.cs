using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is an example of how a condition script could look like. In this case, it checks if the distance between this gameobject and a transform 
 * called player is smaller than maxDistance. This could be used to perform an action if the player is within an agents range. 
*/

public class ExampleCondition : ConditionBase {

    public Transform player;
    public float maxDistance;

    public override bool Condition() /////do a condition check here
    {
        evaluation = false;

        if(Vector3.Distance(transform.position, player.position) < maxDistance)
        {
            evaluation = true;
        }

        return evaluation;
    }
}
