using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is an example of how an action script could look like. In this case, it just writes "Doing nothing..." into the log.
*/

public class ExampleAction : ActionBase {

    public override void Action() /////write the desired action to take place here.
    {
        Debug.Log("Doing nothing...");
    }

}
