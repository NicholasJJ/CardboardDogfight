using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Gvr;
using MobfishCardboard;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject waveControl;
    [SerializeField] CardboardMainCamera menuCam;
    private bool startingGame;
    private GameObject startButton;
    // Start is called before the first frame update
    void Start()
    {
        startingGame = false;
        startButton = GameObject.Find("StartButton");
    }
    // Update is called once per frame
    void Update()
    {
        if (startingGame) {
            transform.position = Vector3.Lerp(transform.position, startButton.transform.position, .2f);
            if (Vector3.Distance(transform.position, startButton.transform.position) <= .01f) {
                player.SetActive(true);
                Matrix4x4 l;
                menuCam.getPov(out l, out _);
                float fov;
                ModifyFOV.GetProjectionMatrixParameters(l, out fov, out _, out _, out _);
                player.GetComponent<nitro>().Begin(fov);
                waveControl.SetActive(true);
                waveControl.GetComponent<EnemyWaveControl>().Begin();
                transform.parent.gameObject.SetActive(false);
            }
        } else {
            if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space))
            {
                if (!Physics.Raycast(transform.position, GameObject.Find("MenuCamera").transform.forward, 50f))
                {
                    CardboardManager.RecenterCamera();
                }
                    
            }
        }
    }
    public bool isClicking(GameObject obj)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, GameObject.Find("MenuCamera").transform.forward, out hit)){
            if (hit.transform.gameObject.Equals(obj))
                return true;
        }
        return false;
    }
    public void OnShipAlign() {
        startingGame = true;
    }
}
