using UnityEngine;
using System.IO;

/// <summary>
/// Permet de capturer une capture d’écran et de l’enregistrer automatiquement
/// dans un dossier défini à l’intérieur du projet Unity.
/// </summary>
public class ScreenshotManager : MonoBehaviour
{
    /// <summary>Nom du dossier où les captures d’écran seront enregistrées (relatif à <c>Application.dataPath</c>).</summary>
    public string folderName = "Screenshots";

    /// <summary>Préfixe à utiliser pour nommer les fichiers de capture d’écran.</summary>
    public string filePrefix = "screenshot_";

    /// <summary>
    /// Prend une capture d’écran et l’enregistre dans le dossier spécifié avec un nom basé sur la date/heure.
    /// </summary>
    public void TakeScreenshot()
    {
        string path = Path.Combine(Application.dataPath, folderName);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string fileName = filePrefix + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string fullPath = Path.Combine(path, fileName);

        ScreenCapture.CaptureScreenshot(fullPath);
        Debug.Log("Capture d'écran enregistrée : " + fullPath);
    }
}
