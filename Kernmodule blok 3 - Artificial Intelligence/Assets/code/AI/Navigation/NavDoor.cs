using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavDoor : MonoBehaviour {

    public List<NavRoom> connectedRooms = new List<NavRoom>(0);

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "room" && !connectedRooms.Contains(other.GetComponent<NavRoom>()))
        {
            connectedRooms.Add(other.GetComponent<NavRoom>());
        }
    }

}
