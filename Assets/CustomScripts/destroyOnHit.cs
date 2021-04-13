using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyOnHit : MonoBehaviour
{
    [SerializeField] float lifeTime = 10;
    private float startTime;
    private void Start()
    {
        startTime = Time.time;
    }
    private void Update()
    {
        if (Time.time >= startTime + lifeTime) {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Radar")
            Destroy(gameObject);
    }
}
