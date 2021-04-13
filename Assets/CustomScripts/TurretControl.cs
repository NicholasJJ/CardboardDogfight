using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControl : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] private float lead;
    [SerializeField] private float reloadTime;
    [SerializeField] private float reloadTimeRange;
    enum shotType { bullet, laser};
    [SerializeField] private shotType myShotType;
    private float timeOfLastShot;
    [SerializeField] private Transform stand;
    public GameObject bullet;
    private float finalReloadTime;
    private GameObject gunBarrel;
	[SerializeField] private GameObject laser;
    [SerializeField] private Gradient[] laserGrad = new Gradient[2];
    [SerializeField] private float laserChargeTime;
    [SerializeField] private float laserShotHoldTime;
    private bool laserFiring;
    private LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        laserFiring = false;
        target = null;
        timeOfLastShot = Time.time;
        finalReloadTime = reloadTime + Random.Range(-reloadTimeRange, reloadTimeRange);
        gunBarrel = transform.Find("gunBarrel").gameObject;
        if (myShotType == shotType.laser)
            line = laser.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) {
            Debug.Log("looking for target");
            target = GameObject.FindGameObjectWithTag("Player");
        } else {
            Vector3 targetPoint = GetTargetPoint(target);
            if (myShotType != shotType.laser || laserFiring == false)
                turnToPoint(targetPoint, currUp());
            if (Vector3.Angle(transform.forward,-currUp()) < 85)
            {
                timeOfLastShot = Time.time;
                transform.localEulerAngles = new Vector3(5, transform.eulerAngles.y, 0);
                if (myShotType == shotType.laser)
                    laser.SetActive(false);
            }
            else
            {
                switch (myShotType)
                {
                    case shotType.bullet:
                        bulletShoot();
                        break;
                    case shotType.laser:
                        laserShoot(targetPoint);
                        break;
                }

                
            }

        }
    }

    Vector3 GetTargetPoint(GameObject t) {
        //TODO: maybe make the shots lead the player?
        return t.transform.position + (t.transform.forward * lead);
    }
    void bulletShoot()
    {
        if (Time.time - timeOfLastShot > finalReloadTime)
        {
            Instantiate(bullet, gunBarrel.transform.position, transform.rotation);
            timeOfLastShot = Time.time;
            finalReloadTime = reloadTime + Random.Range(-reloadTimeRange, reloadTimeRange);
        }
    }
    void laserShoot(Vector3 targ)
    {
        float timeSinceChange = Time.time - timeOfLastShot;
        laser.SetActive(true);
        if ((laserFiring && timeSinceChange >= laserShotHoldTime) ||
            (!laserFiring && timeSinceChange >= laserChargeTime))
        {
            laserFiring = !laserFiring;
            timeOfLastShot = Time.time;
            if (laserFiring)
                line.colorGradient = laserGrad[1];
            else
                line.colorGradient = laserGrad[0];
        }
        Vector3 sPos = transform.position;
        if (laserFiring)
        {
            line.widthMultiplier = Mathf.Lerp(line.widthMultiplier, 10, .02f);
            //Reset laser position in case the turret is being moved
            line.SetPosition(1, sPos + (line.GetPosition(1) - line.GetPosition(0)));
            line.SetPosition(0, sPos);
            Vector3 t = target.transform.position - line.GetPosition(0);
            Vector3 c = Vector3.Project(t, line.GetPosition(1) - line.GetPosition(0));
            if (Vector3.SqrMagnitude(t - c) < 9) //9=3^2, use sqrmag to reduce cpu pressure
            {
                target.GetComponent<OfflineDeath>().laserShot(Time.deltaTime);
            } 
            
        } else
        {
            float p = timeSinceChange / laserChargeTime;
            line.widthMultiplier = Mathf.Lerp(0, .4f, 1 - Mathf.Pow(p,10));
            line.SetPosition(0, sPos);
            line.SetPosition(1, 100000 * (transform.forward) + sPos);
        }
        
    }

    void turnToPoint(Vector3 p, Vector3 u)
    {
        //Debug.Log("turning");
        if (myShotType == shotType.laser)
        {
            Quaternion q = Quaternion.LookRotation(p - transform.position, u);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 90 * Time.deltaTime);
        } else
        {
            transform.LookAt(p, u);
        }
        
        //transform.rotation = q;
    }
    Vector3 currUp()
    {
        return stand.up;
    }
}
