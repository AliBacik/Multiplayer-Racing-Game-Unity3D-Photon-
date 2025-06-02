using UnityEngine;

public class AvoidDetector : MonoBehaviour
{
    public float avoidPath = 0;
    public float avoidTime = 0;
    public float wanderDistance = 4;
    public float avoidLength = 1;

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag !="car") return;

        Rigidbody otherCar = collision.rigidbody;
        avoidTime = Time.time + avoidLength;

        Vector3 otherCarLocalTarget = transform.InverseTransformPoint(otherCar.gameObject.transform.position);
        float otherCarAngle = Mathf.Atan2(otherCarLocalTarget.x, otherCarLocalTarget.z);
        avoidPath = wanderDistance*-Mathf.Sign(otherCarAngle);

    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag !="car") return ;

        avoidTime = 0;
    }
}
