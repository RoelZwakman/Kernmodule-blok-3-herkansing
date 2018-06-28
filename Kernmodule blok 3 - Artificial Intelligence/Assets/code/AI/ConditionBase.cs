using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionBase : MonoBehaviour {

    public ActionBase associatedAction;
    public bool evaluation;

    public virtual bool Condition()
    {
        return evaluation;
    }

}
