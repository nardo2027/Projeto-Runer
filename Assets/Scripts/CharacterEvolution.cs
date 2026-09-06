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

    private void Start()
    {
        RefreshAppearance();
    }

    private void Update()
    {
        RefreshAppearance();
    }

    public void RefreshAppearance()
    {
        if (RunGameManager.Instance == null || stages == null || stages.Length == 0)
            return;

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
}
