using UnityEngine;

public class FlipCar : MonoBehaviour
{
    Rigidbody rb;
    float lastTimeChecked;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
 
    void Update()
    {
        if(transform.up.y>0.5f || rb.linearVelocity.magnitude > 1)
        {
            lastTimeChecked = Time.time;
        }

        if (Time.time > lastTimeChecked) 
        {
            RightCar();
        }
    }

    void RightCar()
    {
        transform.position += Vector3.up;
        transform.rotation = Quaternion.LookRotation(transform.forward);
    }
}
