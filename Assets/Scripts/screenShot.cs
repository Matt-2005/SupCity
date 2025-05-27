using UnityEngine;
using System.IO;

public class ScreenshotManager : MonoBehaviour
{
    public string folderName = "Screenshots";
    public string filePrefix = "screenshot_";

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
