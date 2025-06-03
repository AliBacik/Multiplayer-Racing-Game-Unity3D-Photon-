using Unity.VisualScripting;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Circuit circuit;
    public float brakingSensitivity = 3f;
    Drive ds;
    public float steeringSensitivity = 0.01f;
    public float accelSensitivity = 0.3f;
    Vector3 target;
    Vector3 nextTarget;
    int currentWP = 0;
    float totalDistanceToTarget;

    GameObject tracker;
    int currentTrackerWP = 0;
    public float lookAhead = 10;

    float lastTimeMoving = 0;
    public Ghost _Ghost;

    CheckpointManager cpm;
    float finishSteer;

    void Start()
    {

        if(circuit == null)
        {
            circuit=GameObject.FindGameObjectWithTag("circuit").GetComponent<Circuit>();
        }

        ds = GetComponent<Drive>();
        target=circuit.wayPoints[currentWP].transform.position;
        nextTarget = circuit.wayPoints[currentWP+1].transform.position;
        totalDistanceToTarget =Vector3.Distance(target, ds.rb.gameObject.transform.position);

        tracker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.GetComponent<MeshRenderer>().enabled = false;
        tracker.transform.position = ds.rb.gameObject.transform.position;
        tracker.transform.rotation = ds.rb.gameObject.transform.rotation;

        _Ghost.enabled = false;
        finishSteer = Random.Range(-1.0f, 1.0f);
    }

    void ProgressTracker()
    {
        Debug.DrawLine(ds.rb.gameObject.transform.position, tracker.transform.position);

        if (Vector3.Distance(ds.rb.gameObject.transform.position, tracker.transform.position) > lookAhead) return;

        tracker.transform.LookAt(circuit.wayPoints[currentTrackerWP].transform.position);
        tracker.transform.Translate(0, 0, 1.0f);  //speed of tracker

        if (Vector3.Distance(tracker.transform.position, circuit.wayPoints[currentTrackerWP].transform.position) < 1)
        {
            currentTrackerWP++;
            if (currentTrackerWP >= circuit.wayPoints.Length)
                currentTrackerWP = 0;
        }

    }

    void ResetLayer()
    {
        ds.rb.gameObject.layer = 0;
        _Ghost.enabled = false;
    }
    void Update()
    {
        if (!RaceMonitor.racing) { lastTimeMoving = Time.time; return; }
        if (cpm == null)
        {
            cpm = ds.rb.GetComponent<CheckpointManager>();
        }
        if (cpm.lap == RaceMonitor.totalLaps + 1)
        {
            ds.highAccel.Stop();
            ds.Go(0, finishSteer, 0);
            return;
        }

        ProgressTracker();
        Vector3 localTarget;
        float targetAngle;

        if (ds.rb.linearVelocity.magnitude > 1)
        {
            lastTimeMoving = Time.time;
        }

        if(Time.time > lastTimeMoving+4 || ds.rb.gameObject.transform.position.y <-5)
        {
            ds.rb.gameObject.transform.position=cpm.lastCP.transform.position+Vector3.up*2;
            ds.rb.gameObject.transform.rotation=cpm.lastCP.transform.rotation;

            tracker.transform.position = cpm.lastCP.transform.position;
            ds.rb.gameObject.layer = 7;
            _Ghost.enabled = true;
            Invoke("ResetLayer", 3);
        }

        if (Time.time < ds.rb.GetComponent<AvoidDetector>().avoidTime)
        {
            localTarget = tracker.transform.right * ds.rb.GetComponent<AvoidDetector>().avoidPath;
        }
        else
        {
            localTarget = ds.rb.gameObject.transform.InverseTransformPoint(tracker.transform.position);
        }
        targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(ds.currentSpeed);
        float speedFactor = ds.currentSpeed / ds.maxSpeed;
        float corner = Mathf.Clamp(Mathf.Abs(targetAngle), 0, 90);
        float cornerFactor = corner / 90.0f;

        float brake = 0;

        if (corner> 10 && speedFactor > 0.1f)
        {
            brake = Mathf.Lerp(0, 1 + speedFactor * brakingSensitivity, cornerFactor);
        }

        
        float accel = 1f;

        if (corner > 20 && speedFactor > 0.2f)
        {
            accel = Mathf.Lerp(0, 1 * accelSensitivity, 1 - cornerFactor);
        }

        float prevTorque=ds.Torque;
        if (speedFactor < 0.3f && ds.rb.gameObject.transform.forward.y > 0.1f)
        {
            ds.Torque *= 2f;
            accel = 1f;
            brake = 0;
        }

        ds.Go(accel, steer, brake);

        ds.CheckForSkid();
        ds.CalculateEngineSound();

        ds.Torque = prevTorque;
    }
}
