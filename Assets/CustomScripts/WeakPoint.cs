using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    [SerializeField] int MaxHp = 3;
    [SerializeField] float deathDelay;
    [SerializeField] GameObject explosion;
    int Hp;
    float deathTime;
    // Start is called before the first frame update
    void Start()
    {
        Hp = MaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0) {
            transform.localScale = new Vector3(transform.localScale.x * .9f,
                transform.localScale.y * .9f, transform.localScale.z * .9f);
            if (Time.time - deathTime >= deathDelay) {
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(transform.parent.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with a " + other.gameObject.name);
        if (Hp > 0)
        {
            if (other.tag == "Bullet") {
                if (other.GetComponent<autoMove>().isPlayerBullet)
                    Hp--;
            } else if (other.tag != "Radar"){
                Hp = 0;
            }
            if (Hp <= 0) {
                deathTime = Time.time;
            }
        }
        
    }
    public void ManualDeath() {
		Instantiate(explosion, transform.position, Quaternion.identity);
		Destroy(transform.parent.gameObject);
	}
}
