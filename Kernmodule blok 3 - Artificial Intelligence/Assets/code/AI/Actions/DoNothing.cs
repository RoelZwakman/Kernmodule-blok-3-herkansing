using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNothing : ActionBase
{

    public override void Action()
    {
        base.Action();
        Debug.Log("Doing nothing...");
    }

}
