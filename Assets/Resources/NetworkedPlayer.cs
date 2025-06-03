using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using Photon.Pun.UtilityScripts;

public class NetworkedPlayer : MonoBehaviourPunCallbacks
{
    public static GameObject LocalPlayerInstance;
    public GameObject playerNamePrefab;
    public Rigidbody rb;
    public Renderer jeepMesh;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
        }
        else
        {
            GameObject playerName = Instantiate(playerNamePrefab);
            playerName.GetComponent<NameUIController>().target=rb.gameObject.transform;

            string sentName = null;
            if (photonView.InstantiationData != null)
            {
                sentName = (string)photonView.InstantiationData[0];
            }

            if (sentName != null) 
            {
                playerName.GetComponent<TextMeshProUGUI>().text = sentName;
            }
            else
            {
                playerName.GetComponent<TextMeshProUGUI>().text = photonView.Owner.NickName;
            }
                
            playerName.GetComponent<NameUIController>().carRend = jeepMesh;
        }
    }
}
