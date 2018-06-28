using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndShoot : ActionBase {

    [Header("Moving")]
    public float speed;
    public GameObject target;
    public NavRoom currentRoom;
    public float yOffset;

    public int pathMemorySize;
    public float pathMemoryTime;
    public Queue<NavRoom> pathMemory;

    [Header("Shooting")]
    public GameObject pfx_Shot;
    public float damage;
    public float range;
    public float reloadTime;
    private bool canFire = true;
    public GameObject shootTarget;

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
        
        shootTarget = target;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "room")
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
        Shoot();
    }

    private Transform GetNavTarget()
    {
        Transform navTarget;

        NavRoom bestRoom = currentRoom.GetHighestWeightRoom(target.transform, pathMemory);
        if(bestRoom != null)
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
        if (Vector3.Distance(transform.position, currentRoom.transform.position) < 0.15f && !pathMemory.Contains(currentRoom))
        {
            pathMemory.Enqueue(currentRoom);
            currentRoom.GetComponent<MeshRenderer>().material.color = inMemoryColor;
            yield return new WaitForSeconds(pathMemoryTime);
            pathMemory.Dequeue().GetComponent<MeshRenderer>().material.color = notInMemoryColor;
        }
    }

    private void Shoot()
    {
        if (canFire)
        {
            canFire = false;
            Invoke("SetReadyToFire", reloadTime);

            transform.LookAt(shootTarget.transform);

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range))
            {
                if (hit.transform.tag == "Player" || hit.transform.tag == "Enemy")
                {
                    hit.transform.GetComponent<HealthSystem>().TakeDamage(damage);
                }
            }

            GameObject shotFX = Instantiate(pfx_Shot, transform.position, Quaternion.identity);
            shotFX.transform.LookAt(shootTarget.transform);

        }
    }

    private void SetReadyToFire()
    {
        canFire = true;
    }
}
