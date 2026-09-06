using UnityEngine;
using UnityEngine.SceneManagement;

public class RunGameManager : MonoBehaviour
{
    public static RunGameManager Instance { get; private set; }

    [Header("Run")]
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float speedPerLevel = 0.5f;

    public double DistanceMeters { get; private set; }
    public long TotalPoints { get; private set; }
    public int SpeedLevel { get; private set; }
    public bool IsRunning { get; private set; } = true;

    public float CurrentSpeed => baseSpeed + (SpeedLevel * speedPerLevel);
    public long CurrentSpeedUpgradeCost => 10L * (SpeedLevel + 1) * (SpeedLevel + 1);

    private const string PointsKey = "total_points";
    private const string SpeedLevelKey = "speed_level";
    private const string BestDistanceKey = "best_distance";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        LoadProgress();
    }

    private void Update()
    {
        if (!IsRunning)
            return;

        DistanceMeters += CurrentSpeed * Time.deltaTime;
    }

    public void EndRun()
    {
        if (!IsRunning)
            return;

        IsRunning = false;

        long earnedPoints = Mathf.FloorToInt((float)(DistanceMeters / 10d));
        TotalPoints += earnedPoints;

        double bestDistance = PlayerPrefs.GetFloat(BestDistanceKey, 0f);
        if (DistanceMeters > bestDistance)
            PlayerPrefs.SetFloat(BestDistanceKey, (float)DistanceMeters);

        SaveProgress();
        Debug.Log($"Run ended: {DistanceMeters:F1} m | +{earnedPoints} points");
    }

    public bool BuySpeedUpgrade()
    {
        long cost = CurrentSpeedUpgradeCost;
        if (TotalPoints < cost)
            return false;

        TotalPoints -= cost;
        SpeedLevel++;
        SaveProgress();
        return true;
    }

    public void RestartRun()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadProgress()
    {
        TotalPoints = long.Parse(PlayerPrefs.GetString(PointsKey, "0"));
        SpeedLevel = PlayerPrefs.GetInt(SpeedLevelKey, 0);
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetString(PointsKey, TotalPoints.ToString());
        PlayerPrefs.SetInt(SpeedLevelKey, SpeedLevel);
        PlayerPrefs.Save();
    }
}
