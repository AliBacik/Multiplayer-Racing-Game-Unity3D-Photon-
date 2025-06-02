using System.Collections;
using UnityEngine;

public class RaceMonitor : MonoBehaviour
{
    public GameObject[] countDownItems;
    public static bool racing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(GameObject g in countDownItems)
        {
            g.SetActive(false);
        }
        StartCoroutine(PlayCountDown());
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
