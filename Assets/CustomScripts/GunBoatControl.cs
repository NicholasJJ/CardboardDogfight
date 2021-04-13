using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBoatControl : MonoBehaviour
{
    [SerializeField] private float waitTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxMoveDist;
    private float nextMoveTime;
    private Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        nextMoveTime = Time.time + waitTime;
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextMoveTime) {
            Vector3 targetDelta = Random.onUnitSphere * Random.Range(maxMoveDist / 2, maxMoveDist);
            targetPosition = transform.position + targetDelta;
            nextMoveTime = Time.time + waitTime;
        }
        if (Vector3.Distance(transform.position, targetPosition) > 0.1f) {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed);
        }
    }
}
