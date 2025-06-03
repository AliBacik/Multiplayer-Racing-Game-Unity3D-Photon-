using UnityEngine;

public class Spin : MonoBehaviour
{
    void LateUpdate()
    {
        transform.Rotate(0, 0.1f, 0);
    }
}
