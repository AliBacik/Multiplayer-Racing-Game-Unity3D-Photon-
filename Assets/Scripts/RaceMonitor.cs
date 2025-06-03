using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceMonitor : MonoBehaviour
{
    public GameObject[] countDownItems;
    CheckpointManager[] carsCPM;

    public GameObject[] carPrefabs;
    public Transform[] spawnPos;

    public static bool racing = false;
    public static int totalLaps = 1;
    public GameObject GameOverPanel;
    public GameObject HUD;
    
    void Start()
    {
        foreach(GameObject g in countDownItems)
        {
            g.SetActive(false);
        }
        StartCoroutine(PlayCountDown());

        foreach(Transform t in spawnPos)
        {
            GameObject car = Instantiate(carPrefabs[Random.Range(0,carPrefabs.Length)]);    
            car.transform.position = t.position;
            car.transform.rotation = t.rotation;
        }

        GameObject[] cars = GameObject.FindGameObjectsWithTag("car");
        carsCPM = new CheckpointManager[cars.Length];
        for(int i = 0; i < cars.Length; i++)
        {
            carsCPM[i]=cars[i].GetComponent<CheckpointManager>();
        }
    }

    public void RestartLevel()
    {
        racing = false;
        SceneManager.LoadScene("GameScene");
    }

    private void LateUpdate()
    {
        int finishedCount = 0;
        foreach(CheckpointManager cpm in carsCPM)
        {
            if (cpm.lap == totalLaps)
            {
                finishedCount++;
            }
        }

        if(finishedCount == carsCPM.Length)
        {
            HUD.SetActive(false);
            gameObject.SetActive(true);
        }
    }

    IEnumerator PlayCountDown()
    {
        yield return new WaitForSeconds(2);
        foreach(GameObject g in countDownItems)
        {
            g.SetActive(true);
            yield return new WaitForSeconds (1);
            g.SetActive(false);
        }

        racing = true;
    }

    
}
