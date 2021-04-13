using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fly : MonoBehaviour
{
    public float speed;
    public float strafeSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * (GameObject.FindWithTag("target").transform.position - transform.position));
        //transform.Translate(Vector3.forward);
        //transform.rotation = transform.Find("Main Camera").rotation;
    }
}
