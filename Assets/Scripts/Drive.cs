using UnityEngine;

public class Drive : MonoBehaviour
{
    public WheelCollider[] WheelColliders;
    public GameObject[] Wheel_Meshes;
    public AudioSource SkidSound;
    public AudioSource highAccel;
    public float Torque = 200;
    public float MaxSteerAngle = 30f;
    public float MaxBreakTorque = 500;

    public Transform SkidTrailPrefab;
    Transform[] skidTrails = new Transform[4];

    public ParticleSystem Smoke;
    ParticleSystem[] SkidSmoke = new ParticleSystem[4];

    public GameObject[] BrakeLight;

    public Rigidbody rb;
    public float gearLength = 3;
    public float currentSpeed { get { return rb.linearVelocity.magnitude * gearLength; } }
    public float lowPitch = 1f;
    public float highPitch = 6f;
    public int numGears = 5;
    float rpm;
    int currentGear = 1;
    float currentGearPerc;
    public float maxSpeed = 200f;
    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            SkidSmoke[i]=Instantiate(Smoke);
            SkidSmoke[i].Stop();
        }

        BrakeLight[0].SetActive(false);
        BrakeLight[1].SetActive(false);
    }

    public void CalculateEngineSound()
    {
        float gearPercentage = (1 /(float)numGears);
        float targetGearFactor = Mathf.InverseLerp(gearPercentage * currentGear, gearPercentage * (currentGear + 1), 
            Mathf.Abs(currentSpeed / maxSpeed));
        currentGearPerc=Mathf.Lerp(currentGearPerc,targetGearFactor,Time.deltaTime*5f);
        var gearNumFactor=currentGear/(float)numGears;

        rpm=Mathf.Lerp(gearNumFactor,1,currentGearPerc);

        float speedPercentage= Mathf.Abs(currentSpeed/maxSpeed);
        float upperGearMax = (1/(float)numGears)*(currentGear+1);
        float downGearMax = (1 / (float)numGears) * currentGear;

        if (currentGear > 0 && speedPercentage < downGearMax)
        {
            currentGear--;
        }

        if (speedPercentage > upperGearMax && (currentGear < (numGears - 1)))
        {
            currentGear++;
        }

        float pitch = Mathf.Lerp(lowPitch,highPitch,rpm);
        highAccel.pitch = Mathf.Min(highPitch, pitch) * 0.25f;


    }
    public void StartSkidTrail(int i)
    {
        if(skidTrails[i] == null)
        {
            skidTrails[i]= Instantiate(SkidTrailPrefab);
        }

        skidTrails[i].parent = WheelColliders[i].transform;
        skidTrails[i].localRotation=Quaternion.Euler(90,0,0);
        skidTrails[i].localPosition = -Vector3.up * WheelColliders[i].radius;
    }

    public void EndSkidTrail(int i)
    {
        if (skidTrails[i] == null) return;
        Transform holder = skidTrails[i];
        skidTrails[i] = null;
        holder.parent = null;
        holder.rotation = Quaternion.Euler(90, 0, 0);
        Destroy(holder.gameObject,30);
    }

    public void Go(float Acceleration, float Steer,float brake)
    {

        Acceleration = Mathf.Clamp(Acceleration, -1, 1);
        Steer = Mathf.Clamp(Steer, -1, 1) * MaxSteerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * MaxBreakTorque;

        if (brake != 0)
        {
            BrakeLight[0].SetActive(true);
            BrakeLight[1].SetActive(true);
        }
        else
        {
            BrakeLight[0].SetActive(false);
            BrakeLight[1].SetActive(false);
        }

        float thrustTorque = 0;

        if (currentSpeed<maxSpeed)
        {
            thrustTorque = Acceleration * Torque;
        }

        for (int i = 0; i < 4; i++)
        {
            WheelColliders[i].motorTorque = thrustTorque;

            if (i < 2) 
            { 
                WheelColliders[i].steerAngle = Steer; 
            }
            else
            {
                WheelColliders[i].brakeTorque = brake;
            }
                
            Quaternion quat;
            Vector3 pos;
            WheelColliders[i].GetWorldPose(out pos, out quat);

            Wheel_Meshes[i].transform.position = pos;
            Wheel_Meshes[i].transform.rotation = quat;
        }
        
    }

    public void CheckForSkid()
    {
        int numSkidding = 0;

        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelHit;
            WheelColliders[i].GetGroundHit(out wheelHit);

            if (Mathf.Abs(wheelHit.forwardSlip) >= 0.4f || Mathf.Abs(wheelHit.sidewaysSlip) >= 0.4f)
            {
                numSkidding++;
                if (!SkidSound.isPlaying)
                {
                    SkidSound.Play();
                }

                //StartSkidTrail(i);
                SkidSmoke[i].transform.position = WheelColliders[i].transform.position - WheelColliders[i].transform.up * WheelColliders[i].radius;
                SkidSmoke[i].Emit(1);
            }
            else
            {
               //EndSkidTrail(i);
            }
        }

        if( numSkidding == 0 && SkidSound.isPlaying)
        {
              SkidSound.Stop();
        }
    }

   
}