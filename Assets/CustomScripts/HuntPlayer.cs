using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntPlayer : MonoBehaviour
{
    public float rotateSpeed;
    public float speed;
    public float fireDistance;
    public float fireRoation;
    [SerializeField] GameObject target;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject firePos;
    [SerializeField] private float reloadTime;
    [SerializeField] private float evadeDuration;
    [SerializeField] private float minPlayerDist;
    [SerializeField] private float nextFireTime;
    [SerializeField] public GameObject evadeTarg;
    [SerializeField] private float endOfEvadeTime;
    // Start is called before the first frame update
    void Start()
    {
        evadeTarg = null;
        target = GameObject.FindGameObjectWithTag("Player");
        nextFireTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (evadeTarg == null) {
            Quaternion lookDir = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, rotateSpeed);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            float angleToTarg = Quaternion.Angle(lookDir, transform.rotation);
            float distanceToTarg = Vector3.Distance(transform.position, target.transform.position);
            if (angleToTarg <= fireRoation && distanceToTarg <= fireDistance) {
                Shoot();
            }
            if (distanceToTarg <= minPlayerDist) {
                BeginEvade(target);
            }
        } else {
            Quaternion lookDir = Quaternion.Inverse(Quaternion.LookRotation(evadeTarg.transform.position - transform.position));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, rotateSpeed);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (Time.time > endOfEvadeTime) {
                evadeTarg = null;
            }
        }
        
    }
    void Shoot() {
        if (Time.time > nextFireTime) {
            Instantiate(bullet, firePos.transform.position, transform.rotation);
            nextFireTime = Time.time + reloadTime;
        }
    }
    private void OnTriggerEnter(Collider other){
        HuntPlayer potentialHuntComp = other.gameObject.GetComponent<HuntPlayer>();
        if (other.tag != "Bullet" && !(potentialHuntComp != null && potentialHuntComp.evadeTarg != null
            && potentialHuntComp.evadeTarg.Equals(gameObject))) {
            BeginEvade(other.gameObject);
        }
    }
    void BeginEvade(GameObject evt) {
        evadeTarg = evt.gameObject;
        endOfEvadeTime = Time.time + evadeDuration;
    }
}
