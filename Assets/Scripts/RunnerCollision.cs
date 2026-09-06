using UnityEngine;

public class RunnerCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
            RunGameManager.Instance?.EndRun();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
            RunGameManager.Instance?.EndRun();
    }
}
