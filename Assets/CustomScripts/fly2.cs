using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fly2 : MonoBehaviour
{
    public float speed;
    //public float strafeSpeed;
    public float rotationSpeed;
    public GameObject force;
    public GameObject Camera;
    private Rigidbody rbody;
    float xRot = 0;
    float yRot = 0;
    float zRot = 0;
    public float dodgeTime;
    public float dodgeStall;
    public float dodgeSpeed;
    float dodgeTimer = 0;
    bool dodgeLeft;
    bool dodgeRight;
    public bool dead;
    float deathTime;
    public float maxDeathTime;

    public bool onPC;
    public bool online = true;

    GameObject[] spawnSpots;
    // Start is called before the first frame update
    void Start()
    {
        if (onPC)
        {
            zRot = 0;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        spawnSpots = GameObject.FindGameObjectsWithTag("SpawnSpot");
        dead = false;
        rbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            deathTime += Time.deltaTime;
            transform.Translate(new Vector3(0, 0, (-speed/3) * Time.deltaTime));
            if (deathTime > maxDeathTime)
            {
                if (online) {
                    dead = false;
                    Transform respawn = spawnSpots[Random.Range(0, spawnSpots.Length)].transform;
                    transform.position = respawn.position;
                    transform.rotation = respawn.rotation;
                    transform.Find("spaceship").GetComponent<MeshRenderer>().enabled = true;
                } else {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
        else
        {
            //find rotation
            xRot = -force.transform.localPosition.y;
            yRot = force.transform.localPosition.x;

            if (onPC == false)
            {
                zRot = (Camera.transform.localRotation.eulerAngles.z);
                if (zRot >= 90)
                {
                    zRot = zRot - 360;
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E))
                {
                    zRot = 0;
                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    zRot = 45;
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    zRot = -45;
                }
                else
                {
                    zRot = 0;
                }
            }

            //rotate and move

            transform.Rotate(rotationSpeed * Time.deltaTime * new Vector3(xRot, yRot, zRot / 40));
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));

            //Keep velocity at 0
            rbody.velocity = Vector3.zero;
            rbody.angularVelocity = Vector3.zero;
        }

    }

    public void weDied()
    {
        dead = true;
        deathTime = 0;
        transform.Find("spaceship").GetComponent<MeshRenderer>().enabled = false;
    }
}

