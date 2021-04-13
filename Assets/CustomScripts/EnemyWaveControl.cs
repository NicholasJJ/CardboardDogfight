using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWaveControl : MonoBehaviour
{
    [SerializeField] private GameObject[] waves;
    [SerializeField] private float restTime = 1f;
    public int waveNumber;
    GameObject currWave;
    GameObject player;
    Text display;
    bool startingNextWave;
    float nextWaveStartTime;

    public void Begin() {
        player = GameObject.Find("SPlayer");
        display = GameObject.Find("WaveN").GetComponent<Text>();
        waveNumber = 0;
        NextWaveCountdown();
    }

    // Update is called once per frame
    void Update()
    {
        if ((currWave != null && currWave.transform.childCount == 0) || Input.GetKeyDown(KeyCode.N)) {
            Destroy(currWave);
            NextWaveCountdown();
        }
        if (startingNextWave && Time.time >= nextWaveStartTime) {
            NextWave();
        }
    }

    void NextWaveCountdown() {
        startingNextWave = true;
        nextWaveStartTime = Time.time + restTime;
    }

    void NextWave() {
        int clampedWaveNumber = Mathf.Min(waveNumber, waves.Length - 1);
        currWave = Instantiate(waves[clampedWaveNumber], player.transform.position, player.transform.rotation);
        waveNumber++;
        display.text = waveNumber.ToString();
        startingNextWave = false;
    }
}
