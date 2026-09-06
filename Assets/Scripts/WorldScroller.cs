using UnityEngine;

public class WorldScroller : MonoBehaviour
{
    [SerializeField] private Vector3 movementDirection = Vector3.left;
    [SerializeField] private float destroyAtX = -30f;

    private void Update()
    {
        if (RunGameManager.Instance == null || !RunGameManager.Instance.IsRunning)
            return;

        transform.position += movementDirection.normalized * RunGameManager.Instance.CurrentSpeed * Time.deltaTime;

        if (transform.position.x <= destroyAtX)
            Destroy(gameObject);
    }
}
