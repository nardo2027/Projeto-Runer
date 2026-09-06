using UnityEngine;

public class CharacterEvolution : MonoBehaviour
{
    [System.Serializable]
    public class EvolutionStage
    {
        public string name;
        public int requiredSpeedLevel;
        public GameObject visualRoot;
    }

    [SerializeField] private EvolutionStage[] stages;

    private int activeStage = -1;
    private int lastPrototypeLevel = -1;
    private Renderer bodyRenderer;
    private GameObject runnerPack;
    private GameObject techShoes;
    private GameObject shoulderGear;
    private GameObject thruster;

    private void Start()
    {
        bodyRenderer = GetComponent<Renderer>();

        if (stages == null || stages.Length == 0)
            BuildPrototypeEvolutionParts();

        RefreshAppearance();
    }

    private void Update()
    {
        RefreshAppearance();
    }

    public void RefreshAppearance()
    {
        if (RunGameManager.Instance == null)
            return;

        if (stages != null && stages.Length > 0)
        {
            RefreshConfiguredStages();
            return;
        }

        RefreshPrototypeAppearance();
    }

    private void RefreshConfiguredStages()
    {
        int speedLevel = RunGameManager.Instance.SpeedLevel;
        int unlockedStage = 0;

        for (int i = 0; i < stages.Length; i++)
        {
            if (speedLevel >= stages[i].requiredSpeedLevel)
                unlockedStage = i;
        }

        if (unlockedStage == activeStage)
            return;

        activeStage = unlockedStage;

        for (int i = 0; i < stages.Length; i++)
        {
            if (stages[i].visualRoot != null)
                stages[i].visualRoot.SetActive(i == activeStage);
        }
    }

    private void RefreshPrototypeAppearance()
    {
        int level = RunGameManager.Instance.SpeedLevel;
        if (level == lastPrototypeLevel)
            return;

        lastPrototypeLevel = level;

        SetActive(runnerPack, level >= 1);
        SetActive(techShoes, level >= 3);
        SetActive(shoulderGear, level >= 5);
        SetActive(thruster, level >= 8);

        if (bodyRenderer == null)
            return;

        if (level >= 8)
            bodyRenderer.material.color = new Color(0.65f, 0.85f, 1f);
        else if (level >= 5)
            bodyRenderer.material.color = new Color(0.32f, 0.38f, 0.48f);
        else if (level >= 3)
            bodyRenderer.material.color = new Color(0.15f, 0.55f, 0.95f);
        else if (level >= 1)
            bodyRenderer.material.color = new Color(0.15f, 0.75f, 0.35f);
        else
            bodyRenderer.material.color = new Color(0.88f, 0.72f, 0.55f);
    }

    private void BuildPrototypeEvolutionParts()
    {
        runnerPack = CreatePart("Runner Pack", PrimitiveType.Cube, new Vector3(-0.42f, 0.25f, 0f), new Vector3(0.22f, 0.6f, 0.5f));
        techShoes = CreatePart("Tech Shoes", PrimitiveType.Cube, new Vector3(0.12f, -0.88f, 0f), new Vector3(0.65f, 0.18f, 0.65f));
        shoulderGear = CreatePart("Shoulder Gear", PrimitiveType.Cube, new Vector3(0f, 0.58f, 0f), new Vector3(0.75f, 0.18f, 0.75f));
        thruster = CreatePart("Thruster", PrimitiveType.Cylinder, new Vector3(-0.55f, 0.18f, 0f), new Vector3(0.18f, 0.35f, 0.18f));

        SetActive(runnerPack, false);
        SetActive(techShoes, false);
        SetActive(shoulderGear, false);
        SetActive(thruster, false);
    }

    private GameObject CreatePart(string partName, PrimitiveType primitive, Vector3 localPosition, Vector3 localScale)
    {
        GameObject part = GameObject.CreatePrimitive(primitive);
        part.name = partName;
        part.transform.SetParent(transform, false);
        part.transform.localPosition = localPosition;
        part.transform.localScale = localScale;

        Collider partCollider = part.GetComponent<Collider>();
        if (partCollider != null)
            Destroy(partCollider);

        Renderer partRenderer = part.GetComponent<Renderer>();
        if (partRenderer != null)
            partRenderer.material.color = new Color(0.1f, 0.85f, 1f);

        return part;
    }

    private static void SetActive(GameObject target, bool active)
    {
        if (target != null)
            target.SetActive(active);
    }
}
