using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBase : MonoBehaviour {

    public GraphicsHelper helper;
    public Color associatedColor;
    public float actionPriority;

    private void Awake()
    {
        helper = GetComponent<GraphicsHelper>();
    }

    public virtual void Action()
    {
        helper.ChangeParticleColor(associatedColor);
    }

}
