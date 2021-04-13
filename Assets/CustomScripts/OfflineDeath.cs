using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineDeath : MonoBehaviour
{
    public GameObject deathExplosion;
    public Vector3 RespawnPosition;
    public float maxHP;
    public float HP;
    public float damage;
    public float bumpDamage;
    [SerializeField] private float iTime;
    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        iTime = iTime - Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R) && gameObject.GetComponent<fly2>().dead == false)
        {
            HP = HP - damage;
        }
        if (HP <= 0)
        {
            Die();
        }
        HP = Mathf.Min(HP + (Time.deltaTime * .1f), maxHP);
        transform.Find("Main Camera").Find("Canvas").Find("Slider").GetComponent<Slider>().value = HP;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        if (gameObject.GetComponent<fly2>().dead == false)
        {
            if (other.tag == "Bullet")
            {
                HP = HP - damage;
            }
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        if (gameObject.GetComponent<fly2>().dead == false)
        {
            Vector3 impact = col.GetContact(0).point - transform.position;
            HP -= Mathf.Max(bumpDamage, 1 - (Vector3.Angle(transform.forward, impact) / 50));
        }
    }
    public void laserShot(float d = .34f)
    {
        HP -= d;
    }
    private void Die()
    {
        Debug.Log("u ded");
        Instantiate(deathExplosion, transform.position, Quaternion.identity);
        gameObject.GetComponent<fly2>().weDied();
        HP = 1;
    }
}
