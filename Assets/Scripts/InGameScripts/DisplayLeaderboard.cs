using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayLeaderboard : MonoBehaviour
{

    public TextMeshProUGUI First;
    public TextMeshProUGUI Second;
    public TextMeshProUGUI Third;
    public TextMeshProUGUI Fourth;


    private void Start()
    {
        Leaderboard.Reset();
    }

    private void LateUpdate()
    {
        List<string> places = Leaderboard.GetPlaces();

        if (places.Count > 0) 
        {
            First.text = places[0];
        }
        if (places.Count > 1)
        {
            Second.text = places[1];
        }
        if (places.Count > 2)
        {
            Third.text = places[2];
        }
        if (places.Count > 3)
        {
            Fourth.text = places[3];
        }
        foreach (string s in places) 
        {
            Debug.Log("places: " + s);
        
        }  
    }
}
