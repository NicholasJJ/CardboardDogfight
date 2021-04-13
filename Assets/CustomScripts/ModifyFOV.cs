using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobfishCardboard;

public class ModifyFOV : MonoBehaviour
{
    [SerializeField] private float fovChange;
    private CardboardMainCamera cam;
    private Menu menu;
    private void Start()
    {
        cam = GameObject.Find("MenuCamera").GetComponent<CardboardMainCamera>();
        menu = GameObject.Find("Menu").GetComponent<Menu>();
    }
    private void Update()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space))
        {
            if (menu.isClicking(gameObject))
                gotClicked();
        }
    }
    private void gotClicked() {
        Debug.Log("clicked!");
        float fov, aspect, zNear, zFar;

        Matrix4x4 lmat;
        Matrix4x4 rmat;
        cam.getPov(out lmat, out rmat);
        //Left eye
        GetProjectionMatrixParameters(lmat, out fov, out aspect, out zNear, out zFar);
        //Debug.Log("fov: " + fov + " aspect: " + aspect + " near: " + zNear + " far: " + zFar);
        lmat = Matrix4x4.Perspective(fov + fovChange, aspect, zNear, zFar);

        //Right eye
        GetProjectionMatrixParameters(rmat, out fov, out aspect, out zNear, out zFar);
        Debug.Log("fov: " + fov + " aspect: " + aspect + " near: " + zNear + " far: " + zFar);
        rmat = Matrix4x4.Perspective(fov + fovChange, aspect, zNear, zFar);
        cam.ChangePoV(lmat, rmat);
    }

    //Function written by Bonfanti96
    public static void GetProjectionMatrixParameters(Matrix4x4 mat, out float fov, out float aspect, out float zNear, out float zFar)
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
}
