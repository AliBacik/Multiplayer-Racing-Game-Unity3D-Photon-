using UnityEngine;

public class Circuit : MonoBehaviour
{
    public GameObject[] wayPoints;

    private void OnDrawGizmos()
    {
        DrawGizmos(false);
    }

    private void OnDrawGizmosSelected()
    {
        DrawGizmos(true);
    }
    void DrawGizmos(bool selected)
    {
        if (selected == false) return;

        if (wayPoints.Length > 0)
        {
            Vector3 prev = wayPoints[0].transform.position;
            for (int i = 0; i < wayPoints.Length; i++)
            {
                Vector3 next = wayPoints[i].transform.position;
                Gizmos.DrawLine(prev, next);
                prev = next;
            }

            Gizmos.DrawLine(prev, wayPoints[0].transform.position);
        }
    }
}
