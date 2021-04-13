using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Google;
using MobfishCardboard;

public class nitro : MonoBehaviour
{
    [SerializeField] private float boostCountDownTime;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float fastSpeed;
    [SerializeField] CardboardMainCamera cam;
    [SerializeField] private bool boosting = false;
    [SerializeField] private float boostCountDown;
    [SerializeField] private float defaultFOV;
    [SerializeField] private float trackerFOV;
    // Start is called before the first frame update
    void Start()
    {
        boostCountDown = boostCountDownTime;

        Matrix4x4 mat;
        cam.getPov(out mat, out _);
        float fov;
        GetProjectionMatrixParameters(mat, out fov, out _, out _, out _);
        defaultFOV = fov;
        trackerFOV = defaultFOV;
    }

    public void Begin(float startFov) {
        defaultFOV = startFov;
        trackerFOV = startFov;
        StereoFovPercent(cam, startFov);
    }

    // Update is called once per frame
    void Update()
    {

        //GameObject.Find("nitroCountDown").GetComponent<Text>().text = cam.fieldOfView.ToString();
        if ((Input.touchCount > 0) || Input.GetKey(KeyCode.Space)) {
            boostCountDown -= Time.deltaTime;
        } else if ((Input.touchCount == 0) || Input.GetKeyUp(KeyCode.Space)) {
            boostCountDown = boostCountDownTime;
        }
        if (boostCountDown <= 0 && !boosting) {
            EnterBoost();
        } else if (boostCountDown >= 0 && boosting) {
            ExitBoost();
        }
        if (boosting && trackerFOV <= defaultFOV + 30) {
            //cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 90, .5f);
            trackerFOV = Mathf.Lerp(trackerFOV, defaultFOV + 30, .5f);
            StereoFovPercent(cam, trackerFOV);
        } else if (!boosting && trackerFOV >= defaultFOV) {
            //cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60, .5f);
            trackerFOV = Mathf.Lerp(trackerFOV, defaultFOV, .5f);
            StereoFovPercent(cam, trackerFOV);
        }
    }

    void EnterBoost() {
        boosting = true;
        gameObject.GetComponent<fly2>().speed = fastSpeed;
    }

    void ExitBoost() {
        boosting = false;
        gameObject.GetComponent<fly2>().speed = normalSpeed;
    }

    //following function written by Bonfanti96
    void GetProjectionMatrixParameters(Matrix4x4 mat, out float fov, out float aspect, out float zNear, out float zFar)
    {
        float a = mat[0];
        float b = mat[5];
        float c = mat[10];
        float d = mat[14];

        aspect = b / a;

        float k = (c - 1.0f) / (c + 1.0f);
        zNear = (d * (1.0f - k)) / (2.0f * k);
        zFar = k * zNear;

        fov = Mathf.Rad2Deg * (2.0f * (float)Mathf.Atan(1.0f / b));
    }
    void StereoFovPercent(CardboardMainCamera camm, float nfov)
    {
        float fov, aspect, zNear, zFar;
        Matrix4x4 l, r;
        cam.getPov(out l, out r);
        //Left eye
        GetProjectionMatrixParameters(l, out fov, out aspect, out zNear, out zFar);
        //Debug.Log("fov: " + fov + " aspect: " + aspect + " near: " + zNear + " far: " + zFar);
        l = Matrix4x4.Perspective(nfov, aspect, zNear, 1000);

        //Right eye
        GetProjectionMatrixParameters(r, out fov, out aspect, out zNear, out zFar);
        //Debug.Log("fov: " + fov + " aspect: " + aspect + " near: " + zNear + " far: " + zFar);
        r = Matrix4x4.Perspective(nfov, aspect, zNear, 1000);
        cam.ChangePoV(l, r);
    }

}
