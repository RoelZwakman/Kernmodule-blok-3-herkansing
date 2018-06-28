using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : ActionBase {

    public float speed;
    public GameObject target;
    public NavRoom currentRoom;
    public float yOffset;

    public int pathMemorySize;
    public float pathMemoryTime;
    public Queue<NavRoom> pathMemory;

    [Header("Debug properties")]
    public Color inMemoryColor;
    public Color notInMemoryColor;

    private void Awake()
    {
        pathMemory = new Queue<NavRoom>(pathMemorySize);

        if(target == null) ////Nullcheck om errors te verwijderen in AI vs AI
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        
        helper = GetComponent<GraphicsHelper>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "room")
        {
            currentRoom = other.gameObject.GetComponent<NavRoom>();
            StartCoroutine(AddToPathMemory());

        }
    }

    public override void Action()
    {
        base.Action();
        Vector3 tgt = GetNavTarget().position;
        tgt.y += yOffset;
        transform.position = Vector3.MoveTowards(transform.position, tgt, speed * Time.deltaTime);
    }

    private Transform GetNavTarget()
    {
        Transform navTarget;

        NavRoom bestRoom = currentRoom.GetHighestWeightRoom(target.transform, pathMemory);

        if(bestRoom != null)////Nullcheck om errors te verwijderen in AI vs AI
        {
            navTarget = bestRoom.transform;
        }
        else
        {
            navTarget = currentRoom.transform;
        }
        

        return navTarget;
    }

    private IEnumerator AddToPathMemory()
    {
        if(Vector3.Distance(transform.position, currentRoom.transform.position) < 0.15f && !pathMemory.Contains(currentRoom))
        {
            pathMemory.Enqueue(currentRoom);
            currentRoom.GetComponent<MeshRenderer>().material.color = inMemoryColor;
            yield return new WaitForSeconds(pathMemoryTime);
            pathMemory.Dequeue().GetComponent<MeshRenderer>().material.color = notInMemoryColor;
        }
    }


}
