using System.Collections.Generic;
using UnityEngine;

public class ParkEnvironment : MonoBehaviour
{
    private readonly List<Transform> tiles = new List<Transform>();
    private readonly List<Material> materials = new List<Material>();
    private Material grass, path, wood, leaf, pale, metal, building, glass;
    private Material Mat(float r, float g, float b)
    {
        Material m = new Material(Shader.Find("Standard")) { color = new Color(r, g, b) };
        materials.Add(m); return m;
    }
    private void Shape(string name, Transform parent, Vector3 position, Vector3 scale, Material mat, PrimitiveType primitive = PrimitiveType.Cube)
    {
        GameObject go = GameObject.CreatePrimitive(primitive);
        go.name = name; go.transform.SetParent(parent, false);
        go.transform.localPosition = position; go.transform.localScale = scale;
        Collider c = go.GetComponent<Collider>(); c.enabled = false; Destroy(c);
        go.GetComponent<Renderer>().sharedMaterial = mat;
    }
    private void Awake()
    {
        grass = Mat(.26f, .42f, .25f); path = Mat(.68f, .39f, .27f);
        wood = Mat(.3f, .18f, .12f); leaf = Mat(.21f, .42f, .3f);
        pale = Mat(.91f, .83f, .63f); metal = Mat(.12f, .18f, .2f);
        building = Mat(.36f, .46f, .51f); glass = Mat(.68f, .72f, .64f);
        for (int i = 0; i < 8; i++)
        {
            Transform tile = new GameObject("Park segment " + i).transform;
            tile.SetParent(transform, false); tile.localPosition = new Vector3(-36 + i * 12, 0, 0);
            tiles.Add(tile); BuildSegment(tile, i);
        }
        Camera cam = Camera.main;
        cam.transform.position = new Vector3(0, 4.2f, -13);
        cam.transform.LookAt(new Vector3(0, 1.6f, 0)); cam.fieldOfView = 50;
        cam.backgroundColor = new Color(.83f, .65f, .5f);
        Light sun = Object.FindFirstObjectByType<Light>();
        if (sun != null)
        {
            sun.color = new Color(1, .81f, .59f); sun.intensity = 1.25f;
            sun.transform.rotation = Quaternion.Euler(32, -35, 0); sun.shadows = LightShadows.Soft;
        }
        RenderSettings.ambientLight = new Color(.54f, .58f, .64f);
        RenderSettings.fog = true; RenderSettings.fogColor = cam.backgroundColor;
        RenderSettings.fogMode = FogMode.Linear; RenderSettings.fogStartDistance = 35; RenderSettings.fogEndDistance = 95;
        Shape("Sun", transform, new Vector3(24, 22, 65), Vector3.one * 8, pale, PrimitiveType.Sphere);
    }
    private void BuildSegment(Transform tile, int index)
    {
        Shape("Lawn", tile, new Vector3(0, -.18f, 0), new Vector3(12, .3f, 65), grass);
        Shape("Running track", tile, new Vector3(0, -.025f, 0), new Vector3(12, .08f, 5.5f), path);
        foreach (float z in new[] { -2.55f, 2.55f })
            Shape("Track border", tile, new Vector3(0, .015f, z), new Vector3(12, .03f, .1f), pale);
        for (int j = 0; j < 3; j++)
            Shape("Track dash", tile, new Vector3(-4 + j * 4, .01f, 1.25f), new Vector3(1.7f, .03f, .07f), pale);
        Shape("Walkway", tile, new Vector3(0, -.03f, 4.3f), new Vector3(12, .08f, 2), pale);
        for (int j = 0; j < 2; j++)
        {
            float x = -4 + j * 7; float z = 7 + (index % 3);
            Shape("Tree trunk", tile, new Vector3(x, 1.3f, z), new Vector3(.3f, 2.6f, .3f), wood);
            Shape("Tree crown", tile, new Vector3(x, 3.1f, z), new Vector3(2.8f, 3.4f, 2.8f), leaf, PrimitiveType.Sphere);
            Shape("Tree crown highlight", tile, new Vector3(x + .6f, 3.8f, z), new Vector3(2, 2.1f, 2.2f), grass, PrimitiveType.Sphere);
        }
        Shape("Bench seat", tile, new Vector3(1, .65f, 5.5f), new Vector3(2, .13f, .65f), wood);
        Shape("Bench back", tile, new Vector3(1, 1.05f, 5.8f), new Vector3(2, .65f, .1f), wood);
        foreach (float x in new[] { .2f, 1.8f })
            Shape("Bench leg", tile, new Vector3(x, .3f, 5.5f), new Vector3(.12f, .6f, .5f), metal);
        Shape("Lamp post", tile, new Vector3(-4, 1.8f, 4), new Vector3(.1f, 3.6f, .1f), metal);
        Shape("Lamp", tile, new Vector3(-4, 3.65f, 4), new Vector3(.6f, .24f, .6f), pale);
        for (int j = 0; j < 3; j++)
        {
            float h = 5 + ((index * 7 + j * 3) % 9);
            float x = -4 + j * 4;
            Shape("City building", tile, new Vector3(x, h / 2, 25), new Vector3(3.4f, h, 4), building);
            for (float y = 1; y < h - .5f; y += 1.4f)
                Shape("Window band", tile, new Vector3(x, y, 22.98f), new Vector3(2.7f, .55f, .035f), glass);
        }
    }
    private void Update()
    {
        RunGameManager game = RunGameManager.Instance;
        if (game == null || !game.IsRunning) return;
        foreach (Transform tile in tiles)
        {
            tile.localPosition += Vector3.left * game.CurrentSpeed * Time.deltaTime;
            if (tile.localPosition.x < -42) tile.localPosition += Vector3.right * 96;
        }
    }
    private void OnDestroy() { foreach (Material material in materials) Destroy(material); }
}
