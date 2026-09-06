using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunGameManager : MonoBehaviour
{
    public static RunGameManager Instance { get; private set; }

    [Header("Run")]
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float speedPerLevel = 0.5f;

    public double DistanceMeters { get; private set; }
    public double BestDistanceMeters { get; private set; }
    public long TotalPoints { get; private set; }
    public long LastRunPoints { get; private set; }
    public int SpeedLevel { get; private set; }
    public bool IsRunning { get; private set; } = true;

    public float CurrentSpeed => baseSpeed + (SpeedLevel * speedPerLevel);
    public long CurrentSpeedUpgradeCost => 10L * (SpeedLevel + 1) * (SpeedLevel + 1);

    private const string PointsKey = "total_points";
    private const string SpeedLevelKey = "speed_level";
    private const string BestDistanceKey = "best_distance_meters";

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
        LastRunPoints = Math.Max(1L, (long)Math.Floor(DistanceMeters / 10d));
        TotalPoints += LastRunPoints;

        if (DistanceMeters > BestDistanceMeters)
            BestDistanceMeters = DistanceMeters;

        SaveProgress();
        Debug.Log($"Run ended: {DistanceMeters:F1} m | +{LastRunPoints} points");
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
        Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene.buildIndex >= 0)
            SceneManager.LoadScene(activeScene.buildIndex);
        else if (!string.IsNullOrWhiteSpace(activeScene.name))
            SceneManager.LoadScene(activeScene.name);
    }

    private void LoadProgress()
    {
        if (!long.TryParse(PlayerPrefs.GetString(PointsKey, "0"), out long points))
            points = 0;

        if (!double.TryParse(PlayerPrefs.GetString(BestDistanceKey, "0"), out double bestDistance))
            bestDistance = 0d;

        TotalPoints = points;
        BestDistanceMeters = bestDistance;
        SpeedLevel = PlayerPrefs.GetInt(SpeedLevelKey, 0);
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetString(PointsKey, TotalPoints.ToString());
        PlayerPrefs.SetString(BestDistanceKey, BestDistanceMeters.ToString("R"));
        PlayerPrefs.SetInt(SpeedLevelKey, SpeedLevel);
        PlayerPrefs.Save();
    }
}
