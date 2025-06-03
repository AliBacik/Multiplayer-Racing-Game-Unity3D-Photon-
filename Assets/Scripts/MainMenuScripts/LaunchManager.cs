using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LaunchManager : MonoBehaviour
{
    public TMP_InputField playerName;
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            playerName.text = PlayerPrefs.GetString("PlayerName");
        }
    }

    public void SetName(string name)
    {
        PlayerPrefs.SetString("PlayerName",name);
        PlayerPrefs.Save();
    }
    
    public void ConnectSinglePlayerMode()
    {
        SceneManager.LoadScene("GameScene");
    }
}
