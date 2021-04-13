using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public float lookSpeed;
    Quaternion targetYRotation, targetXRotation;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Mouse Y");
        var y = Input.GetAxis("Mouse X");


        /*  targetYRotation = transform.localRotation * Quaternion.AngleAxis(y * lookSpeed * Time.deltaTime, Vector3.up);
          targetXRotation = transform.localRotation * Quaternion.AngleAxis(-x * lookSpeed * Time.deltaTime, Vector3.right);

          transform.localRotation = Quaternion.Slerp(transform.localRotation, targetYRotation, Time.deltaTime * 15);
          transform.localRotation = Quaternion.Slerp(transform.localRotation, targetXRotation, Time.deltaTime * 15);
          */

        transform.Rotate(-x * lookSpeed * Time.deltaTime, y * lookSpeed * Time.deltaTime, -transform.localRotation.eulerAngles.z);
    }
}


