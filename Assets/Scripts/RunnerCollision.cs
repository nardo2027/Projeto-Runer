using UnityEngine;

public class RunnerCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ObstacleMarker>() != null)
            RunGameManager.Instance?.EndRun();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ObstacleMarker>() != null)
            RunGameManager.Instance?.EndRun();
    }
}
