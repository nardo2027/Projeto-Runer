using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private float spawnX = 16f;
    [SerializeField] private float minGapMeters = 8f;
    [SerializeField] private float maxGapMeters = 14f;

    private double nextSpawnDistance;

    private void Start()
    {
        ScheduleNextSpawn(6f);
    }

    private void Update()
    {
        RunGameManager game = RunGameManager.Instance;
        if (game == null || !game.IsRunning)
            return;

        if (game.DistanceMeters >= nextSpawnDistance)
        {
            SpawnObstacle();
            ScheduleNextSpawn(Random.Range(minGapMeters, maxGapMeters));
        }
    }

    private void ScheduleNextSpawn(float gap)
    {
        double currentDistance = RunGameManager.Instance != null
            ? RunGameManager.Instance.DistanceMeters
            : 0d;

        nextSpawnDistance = currentDistance + gap;
    }

    private void SpawnObstacle()
    {
        float width = Random.Range(0.7f, 1.4f);
        float height = Random.Range(0.8f, 1.8f);

        GameObject obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obstacle.name = "Obstacle";
        obstacle.transform.position = new Vector3(spawnX, height * 0.5f, 0f);
        obstacle.transform.localScale = new Vector3(width, height, 1.5f);
        obstacle.AddComponent<ObstacleMarker>();
        obstacle.AddComponent<WorldScroller>();

        Renderer renderer = obstacle.GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = new Color(0.72f, 0.18f, 0.12f);
    }
}
