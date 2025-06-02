using TMPro;
using UnityEngine;

public class NameUIController : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI lapDisplay;
    public Transform target;
    CanvasGroup canvasGroup;
    public Renderer carRend;
    CheckpointManager cpManager;
    void Start()
    {
        transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(),false);
        canvasGroup=GetComponent<CanvasGroup>();
    }

    void LateUpdate()
    {
        if (!RaceMonitor.racing) { canvasGroup.alpha = 0; return; }
        if (carRend == null) return;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        bool carInView = GeometryUtility.TestPlanesAABB(planes, carRend.bounds);
        canvasGroup.alpha = carInView ? 1 : 0;
        transform.position=Camera.main.WorldToScreenPoint(target.position+Vector3.up * 1.2f);

        if (cpManager == null) 
        { 
           cpManager=target.GetComponent<CheckpointManager>();
        }

        lapDisplay.text = "Lap:" + cpManager.lap + " (CP: " + cpManager.checkPoint + ")";
    }
}
