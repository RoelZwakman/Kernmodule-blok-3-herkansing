using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavRoom : MonoBehaviour {

    [Header("Connected navigation objects")]
    public List<NavRoom> connectedRooms = new List<NavRoom>(0);
    public List<NavDoor> connectedDoors = new List<NavDoor>(0);
    
    [Header("Navigation variables")]
    public float roomResistance;
    public bool readyForUse = false;

    private void Awake()
    {
        StartCoroutine(SetupConnectedRooms());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "door" && !connectedDoors.Contains(other.GetComponent<NavDoor>()))
        {
            connectedDoors.Add(other.GetComponent<NavDoor>());
        }
    }

    private IEnumerator SetupConnectedRooms()
    {
        yield return new WaitForSeconds(1f);
        if(connectedDoors.Count > 0)
        {
            for(int i = 0; i < connectedDoors.Count; i++)
            {
                for(int j = 0; j < connectedDoors[i].connectedRooms.Count; j++) /////iterate through all the connected rooms of the connected door
                {
                    if (connectedDoors[i].connectedRooms[j] != this)
                    {
                        connectedRooms.Add(connectedDoors[i].connectedRooms[j]);
                    }
                }
            }
            readyForUse = true;
        }

    }

    public NavRoom GetHighestWeightRoom(Transform target, Queue<NavRoom> pathMemory)
    {
        
        NavRoom lowestWeightRoom = new NavRoom();
        float lastLowestWeight = Mathf.Infinity;
        
        for(int i = 0; i < connectedRooms.Count; i++)
        {
            if (connectedRooms[i].GetWeight(target) < lastLowestWeight && !pathMemory.Contains(connectedRooms[i]))
            {
                lastLowestWeight = connectedRooms[i].GetWeight(target);
                lowestWeightRoom = connectedRooms[i];
            }
           
        }

        return lowestWeightRoom;

    }

    public float GetWeight(Transform target)
    {
        float w;
        float distance = Vector3.Distance(transform.position, target.position);
        w = distance * roomResistance;

        return w;
    }

}
