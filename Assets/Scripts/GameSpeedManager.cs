using UnityEngine;

/// <summary>
/// Gère la vitesse du temps dans le jeu via Time.timeScale.
/// Permet de mettre en pause, de jouer à vitesse normale ou d'accélérer le jeu.
/// Les méthodes peuvent être liées à des boutons dans l'interface Unity.
/// </summary>
public class GameSpeedManager : MonoBehaviour
{
    /// <summary>
    /// États possibles de la vitesse du jeu.
    /// </summary>
    public enum SpeedState
    {
        Paused,
        Normal,
        Fast,
        UltraFast
    }

    /// <summary>
    /// Vitesse actuelle sélectionnée.
    /// </summary>
    private SpeedState currentSpeed = SpeedState.Normal;

    /// <summary>
    /// Met à jour la valeur de <see cref="Time.timeScale"/> en fonction de l’état actuel.
    /// </summary>
    private void UpdateTimeScale()
    {
        switch (currentSpeed)
        {
            case SpeedState.Paused:
                Time.timeScale = 0f;
                break;
            case SpeedState.Normal:
                Time.timeScale = 1f;
                break;
            case SpeedState.Fast:
                Time.timeScale = 2f;
                break;
            case SpeedState.UltraFast:
                Time.timeScale = 4f;
                break;
        }

        Debug.Log("Speed set to: " + currentSpeed + " (Time.timeScale = " + Time.timeScale + ")");
    }

    /// <summary>
    /// Met le jeu en pause.
    /// </summary>
    public void SetPaused()
    {
        currentSpeed = SpeedState.Paused;
        UpdateTimeScale();
    }

    /// <summary>
    /// Définit la vitesse normale (1x).
    /// </summary>
    public void SetNormal()
    {
        currentSpeed = SpeedState.Normal;
        UpdateTimeScale();
    }

    /// <summary>
    /// Définit une vitesse accélérée (2x).
    /// </summary>
    public void SetFast()
    {
        currentSpeed = SpeedState.Fast;
        UpdateTimeScale();
    }

    /// <summary>
    /// Définit une vitesse ultra-rapide (4x).
    /// </summary>
    public void SetUltraFast()
    {
        currentSpeed = SpeedState.UltraFast;
        UpdateTimeScale();
    }

    /// <summary>
    /// Retourne l’état de vitesse actuel.
    /// </summary>
    /// <returns>L'état actuel de <see cref="SpeedState"/>.</returns>
    public SpeedState GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
