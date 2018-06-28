using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedDoor : MonoBehaviour {

    public enum DoorState
    {
        CLOSED = 0,
        OPEN = 1
    }

    public DoorState doorState;
    public AnimationCurve closedToOpen;
    public AnimationCurve openToClosed;
    public float scaleMultiplier;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Enemy")
        {
            StartCoroutine(IChangeDoorState(DoorState.OPEN));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            StartCoroutine(IChangeDoorState(DoorState.CLOSED));
        }
    }

    private void Start()
    {
        StartCoroutine(IChangeDoorState(doorState));
    }

    private IEnumerator IChangeDoorState(DoorState state)
    {
        float curveTime = 0;
        Vector3 tempPos = transform.position;
        while(transform.position.y != closedToOpen.keys[1].value * scaleMultiplier || transform.position.y != openToClosed.keys[1].value * scaleMultiplier)
        {
            switch (state)
            {
                case DoorState.CLOSED:
                    tempPos.y = openToClosed.Evaluate(curveTime) * scaleMultiplier;
                    transform.position = tempPos;
                    break;
                case DoorState.OPEN:
                    tempPos.y = closedToOpen.Evaluate(curveTime) * scaleMultiplier;
                    transform.position = tempPos;
                    break;
            }
            curveTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

}
