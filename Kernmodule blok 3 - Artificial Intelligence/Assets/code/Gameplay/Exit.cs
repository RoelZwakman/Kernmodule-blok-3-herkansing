using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Exit : MonoBehaviour {

    public UnityAction OnExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OnExit();
        }
    }

}
