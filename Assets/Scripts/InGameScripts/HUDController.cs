using UnityEngine;

public class HUDController : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    float HUDSetting = 0;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        if (PlayerPrefs.HasKey("HUD"))
        {
            HUDSetting = PlayerPrefs.GetFloat("HUD");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (RaceMonitor.racing)
        {
            canvasGroup.alpha = HUDSetting;
        }

        if (Input.GetKeyUp(KeyCode.H)) 
        {
            canvasGroup.alpha = canvasGroup.alpha == 1 ? 0 : 1;
            HUDSetting=canvasGroup.alpha;
            PlayerPrefs.SetFloat("HUD",canvasGroup.alpha);
        
        }

    }
}
