using UnityEngine;

public static class PrototypeBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void BuildPrototypeIfNeeded()
    {
        if (Object.FindObjectOfType<RunGameManager>() != null)
            return;

        GameObject systems = new GameObject("Runer Prototype Systems");
        systems.AddComponent<RunGameManager>();
        systems.AddComponent<RunHUD>();

        SetupCamera();
        SetupLighting();
        CreateGround();
        CreateRunner();

        GameObject spawner = new GameObject("Obstacle Spawner");
        spawner.AddComponent<ObstacleSpawner>();
    }

    private static void SetupCamera()
    {
        Camera camera = Camera.main;

        if (camera == null)
        {
            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            camera = cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();
        }

        camera.transform.position = new Vector3(2f, 4f, -14f);
        camera.transform.LookAt(new Vector3(2f, 1.2f, 0f));
        camera.fieldOfView = 55f;
        camera.backgroundColor = new Color(0.12f, 0.17f, 0.24f);
        camera.clearFlags = CameraClearFlags.SolidColor;
    }

    private static void SetupLighting()
    {
        if (Object.FindObjectOfType<Light>() != null)
            return;

        GameObject lightObject = new GameObject("Directional Light");
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        lightObject.transform.rotation = Quaternion.Euler(45f, -35f, 0f);

        RenderSettings.ambientLight = new Color(0.45f, 0.45f, 0.5f);
    }

    private static void CreateGround()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";
        ground.transform.position = new Vector3(2f, -0.5f, 0f);
        ground.transform.localScale = new Vector3(55f, 1f, 8f);

        Renderer renderer = ground.GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = new Color(0.16f, 0.18f, 0.20f);

        CreateLaneMarker(new Vector3(2f, 0.02f, 2.5f));
        CreateLaneMarker(new Vector3(2f, 0.02f, -2.5f));
    }

    private static void CreateLaneMarker(Vector3 position)
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        marker.name = "Lane Marker";
        marker.transform.position = position;
        marker.transform.localScale = new Vector3(55f, 0.03f, 0.08f);

        Collider collider = marker.GetComponent<Collider>();
        if (collider != null)
            Object.Destroy(collider);

        Renderer renderer = marker.GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = new Color(0.65f, 0.68f, 0.72f);
    }

    private static void CreateRunner()
    {
        GameObject runner = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        runner.name = "Runner";
        runner.transform.position = new Vector3(-5f, 1f, 0f);

        Rigidbody body = runner.AddComponent<Rigidbody>();
        body.mass = 1f;
        body.useGravity = true;
        body.interpolation = RigidbodyInterpolation.Interpolate;

        runner.AddComponent<RunnerController>();
        runner.AddComponent<RunnerCollision>();
        runner.AddComponent<CharacterEvolution>();
    }
}
