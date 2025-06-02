using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public int lap = 0;
    public int checkPoint = -1;
    int checkPointCount;
    int nextCheckPoint;

    void Start()
    {
        checkPointCount = GameObject.FindGameObjectsWithTag("checkpoint").Length;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "checkpoint")
        {
            int thisCPNumber = int.Parse(other.gameObject.name);

            if(thisCPNumber == nextCheckPoint)
            {
                checkPoint = thisCPNumber;

                if (checkPoint == 0) lap++;

                nextCheckPoint++;

                if (nextCheckPoint >= checkPointCount)
                {
                    nextCheckPoint = 0;
                }
            }
        }
    }
}
