using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Permet de réinitialiser la scène actuelle en la rechargeant.
/// Utile pour redémarrer le jeu ou tester rapidement en mode développement.
/// </summary>
public class Reset : MonoBehaviour
{
    /// <summary>
    /// Recharge la scène active, réinitialisant tous les objets à leur état initial.
    /// </summary>
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
