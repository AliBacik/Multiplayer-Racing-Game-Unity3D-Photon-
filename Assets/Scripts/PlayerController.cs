using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Drive _drive;
    float lastTimeMoving = 0;
    Vector3 lastPosition;
    Quaternion lastRotation;
    void Start()
    {
        _drive = GetComponent<Drive>();
        GetComponent<Ghost>().enabled = false;
    }

    void ResetLayer()
    {
        _drive.rb.gameObject.layer = 0;
        GetComponent<Ghost>().enabled = false;
    }
    void Update()
    {

        float a = Input.GetAxis("Vertical");
        float s = Input.GetAxis("Horizontal");
        float b = Input.GetAxis("Jump");

        if (_drive.rb.linearVelocity.magnitude > 1 || !RaceMonitor.racing)
        {
            lastTimeMoving = Time.time;
        }

        RaycastHit hit;
        if(Physics.Raycast(_drive.rb.gameObject.transform.position,-Vector3.up,out hit, 10))
        {
            if (hit.collider.gameObject.tag == "road")
            {
                lastPosition=_drive.rb.gameObject.transform.position;
                lastRotation=_drive.rb.gameObject.transform.rotation;
            }
        }

        if(Time.time > lastTimeMoving+4)
        {
            _drive.rb.gameObject.transform.position=lastPosition;
            _drive.rb.gameObject.transform.rotation=lastRotation;
            _drive.rb.gameObject.layer = 6;
            GetComponent<Ghost>().enabled=true;
            Invoke("ResetLayer", 3);
        }

        if (!RaceMonitor.racing) a=0;

        _drive.Go(a, s, b);

        _drive.CheckForSkid();

        _drive.CalculateEngineSound();

    }
}
