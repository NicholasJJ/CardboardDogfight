using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyAfterTime : MonoBehaviour
{

    public float time;
    private float counter;

    // Start is called before the first frame update
    void Start()
    {
        counter = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup >= counter + time)
        {
            Destroy(gameObject);
        }
    }
}
