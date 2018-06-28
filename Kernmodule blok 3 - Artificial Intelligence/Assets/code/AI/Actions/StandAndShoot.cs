using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandAndShoot : ActionBase {

    public GameObject pfx_Shot;
    public float damage;
    public float range;
    public float reloadTime;
    private bool canFire = true;
    public GameObject target;

    private void Awake()
    {
        if(target == null) ////Nullcheck om errors te verwijderen in AI vs AI
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        
    }

    public override void Action()
    {
        base.Action();
        Shoot();
        Debug.Log("Fired at player.");
    }

    private void Shoot()
    {
        if (canFire)
        {
            canFire = false;
            Invoke("SetReadyToFire", reloadTime);

            if(target.transform != null) ////Nullcheck om errors te verwijderen in AI vs AI
            {
                transform.LookAt(target.transform);
            }
            

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range))
            {
                if (hit.transform.tag == "Player" || hit.transform.tag == "Enemy")
                {
                    hit.transform.GetComponent<HealthSystem>().TakeDamage(damage);
                }
            }

            GameObject shotFX = Instantiate(pfx_Shot, transform.position, Quaternion.identity);
            shotFX.transform.LookAt(target.transform);


        }
    }

    private void SetReadyToFire()
    {
        canFire = true;
    }

}
