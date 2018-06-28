using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    public GameObject pfx_Shot;
    public float damage;
    public float range;
    public float reloadTime;
    public Transform origin;

    public bool canFire;


    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (canFire)
        {
            canFire = false;
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * range, Color.yellow);

            if (Physics.Raycast(origin.position, origin.TransformDirection(Vector3.forward), out hit, range))
            {
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.GetComponent<HealthSystem>().TakeDamage(damage);
                }
            }

            Invoke("SetReadyToFire", reloadTime);

            GameObject shotFX = Instantiate(pfx_Shot, transform.position, Quaternion.identity);
            shotFX.transform.rotation = Quaternion.LookRotation(origin.TransformDirection(Vector3.forward));
        }
    }

    private void SetReadyToFire()
    {
        canFire = true;
    }

}
