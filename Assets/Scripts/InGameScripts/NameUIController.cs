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

    int carRego;
    void Start()
    {
        transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(),false);
        canvasGroup=GetComponent<CanvasGroup>();
        carRego=Leaderboard.RegisterCar(playerName.text);
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

        Leaderboard.SetPosition(carRego,cpManager.lap,cpManager.checkPoint,cpManager.timeEntered);
        string position=Leaderboard.GetPosition(carRego);

        lapDisplay.text = position + " " + cpManager.lap + " (" + cpManager.checkPoint + ")";
    }
}
