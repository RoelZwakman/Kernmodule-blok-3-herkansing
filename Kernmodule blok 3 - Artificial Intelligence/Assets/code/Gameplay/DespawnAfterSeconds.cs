using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnAfterSeconds : MonoBehaviour {

    public float lifetime;

    private void Awake()
    {
        StartCoroutine(IDespawnAfterSeconds());
    }

    private IEnumerator IDespawnAfterSeconds()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

}
