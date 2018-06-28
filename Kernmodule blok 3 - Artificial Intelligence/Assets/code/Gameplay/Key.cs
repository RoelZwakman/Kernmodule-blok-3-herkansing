using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Key : MonoBehaviour {

    public UnityAction OnKeyTaken;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            OnKeyTaken();
        }
    }
}
