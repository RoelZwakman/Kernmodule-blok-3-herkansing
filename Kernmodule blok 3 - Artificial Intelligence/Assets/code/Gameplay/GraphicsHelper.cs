using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsHelper : MonoBehaviour {

    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void ChangeParticleColor(Color color)
    {
        ps.startColor = color;
    }

}
