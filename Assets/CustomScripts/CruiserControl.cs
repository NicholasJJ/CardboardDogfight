using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CruiserControl : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] float minTurnDistance;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("SPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) >= minTurnDistance) {
            Quaternion lookDir = Quaternion.LookRotation(player.transform.position - transform.position);
            lookDir = Quaternion.Euler(lookDir.eulerAngles + (Vector3.up * 45));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, rotateSpeed);
        }
        if (transform.childCount == 3)
            transform.Find("Core").GetComponent<WeakPoint>().ManualDeath();
    }
}
