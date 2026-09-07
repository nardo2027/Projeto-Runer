using UnityEngine;

public class RunHUD : MonoBehaviour
{
    private readonly double[] milestoneMeters =
    {
        10d,
        100d,
        1_000d,
        10_000d,
        100_000d,
        1_000_000d,
        10_000_000d,
        40_075_000d,
        384_400_000d,
        149_597_870_700d
    };

    private readonly string[] milestoneNames =
    {
        "10 m",
        "100 m",
        "1 km",
        "10 km",
        "100 km",
        "1.000 km",
        "10.000 km",
        "Volta ao mundo",
        "Distância da Lua",
        "1 UA (escala Terra-Sol)"
    };

    private GUIStyle titleStyle;
    private GUIStyle labelStyle;
    private GUIStyle centerStyle;
    private GUIStyle buttonStyle;

    private void Update()
    {
        if (RunGameManager.Instance != null &&
            !RunGameManager.Instance.IsRunning &&
            RunnerInput.RestartPressedThisFrame())
        {
            RunGameManager.Instance.RestartRun();
        }
    }

    private void OnGUI()
    {
        RunGameManager game = RunGameManager.Instance;
        if (game == null)
            return;

        EnsureStyles();

        DrawPanel(new Rect(15, 15, 265, 155));
        GUI.Label(new Rect(25, 28, 245, 30), "RUNER / PARQUE 0.2", titleStyle);
        GUI.Label(new Rect(30, 62, 300, 24), $"Distância: {FormatDistance(game.DistanceMeters)}", labelStyle);
        GUI.Label(new Rect(30, 88, 300, 24), $"Velocidade: {game.CurrentSpeed:F1} m/s", labelStyle);
        GUI.Label(new Rect(30, 114, 300, 24), $"Pontos: {game.TotalPoints:N0}", labelStyle);
        GUI.Label(new Rect(30, 140, 300, 24), $"Próximo: {GetNextMilestone(game.DistanceMeters)}", labelStyle);

        GUI.Label(
            new Rect(Screen.width - 300, 22, 280, 25),
            $"Recorde: {FormatDistance(game.BestDistanceMeters)}",
            labelStyle);

        if (game.IsRunning)
        {
            GUI.Label(
                new Rect(0, Screen.height - 55, Screen.width, 35),
                "ESPAÇO / ↑ / W para pular",
                centerStyle);
            return;
        }

        DrawGameOver(game);
    }

    private void DrawGameOver(RunGameManager game)
    {
        float width = 440f;
        float height = 300f;
        float x = (Screen.width - width) * 0.5f;
        float y = (Screen.height - height) * 0.5f;

        DrawPanel(new Rect(x, y, width, height));
        GUI.Label(new Rect(x + 20, y + 20, width - 40, 40), "CORRIDA ENCERRADA", titleStyle);
        GUI.Label(new Rect(x + 20, y + 70, width - 40, 30), $"Distância: {FormatDistance(game.DistanceMeters)}", centerStyle);
        GUI.Label(new Rect(x + 20, y + 103, width - 40, 30), $"+ {game.LastRunPoints:N0} pontos", centerStyle);
        GUI.Label(new Rect(x + 20, y + 136, width - 40, 30), $"Velocidade nível {game.SpeedLevel}", centerStyle);

        long cost = game.CurrentSpeedUpgradeCost;
        bool canBuy = game.TotalPoints >= cost;
        string upgradeText = canBuy
            ? $"Melhorar velocidade ({cost:N0} pts)"
            : $"Velocidade custa {cost:N0} pts";

        GUI.enabled = canBuy;
        if (GUI.Button(new Rect(x + 55, y + 180, width - 110, 42), upgradeText, buttonStyle))
            game.BuySpeedUpgrade();
        GUI.enabled = true;

        if (GUI.Button(new Rect(x + 55, y + 235, width - 110, 42), "Correr novamente (R)", buttonStyle))
            game.RestartRun();
    }

    private string GetNextMilestone(double distance)
    {
        for (int i = 0; i < milestoneMeters.Length; i++)
        {
            if (distance < milestoneMeters[i])
            {
                double remaining = milestoneMeters[i] - distance;
                return $"{milestoneNames[i]} ({FormatDistance(remaining)} faltando)";
            }
        }

        return "Espaço interestelar";
    }

    public static string FormatDistance(double meters)
    {
        if (meters < 1_000d)
            return $"{meters:F1} m";

        double kilometers = meters / 1_000d;
        if (kilometers < 1_000_000d)
            return $"{kilometers:N1} km";

        return $"{kilometers / 1_000_000d:N2} milhões km";
    }

    private static void DrawPanel(Rect area)
    {
        Color previous = GUI.color;
        GUI.color = new Color(.045f, .075f, .10f, .94f);
        GUI.DrawTexture(area, Texture2D.whiteTexture);
        GUI.color = previous;
    }

    private void EnsureStyles()
    {
        if (titleStyle != null)
            return;

        titleStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 18,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };

        labelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 16
        };

        centerStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 17,
            alignment = TextAnchor.MiddleCenter
        };

        buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 16,
            fontStyle = FontStyle.Bold
        };
    }
}
