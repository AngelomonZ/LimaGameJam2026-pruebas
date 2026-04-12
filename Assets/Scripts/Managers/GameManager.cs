using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Referencias")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private string mainMenuSceneName = "MainMenu"; // Nombre exacto de tu escena de menú

    private void Awake()
    {
        // Configuración Singleton básica
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Asegurarnos de que el panel empiece apagado y el tiempo corra
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void EndGame(int playerIndexWinner)
    {
        gameOverPanel.SetActive(true);

        if (playerIndexWinner == 1)
        {
            winnerText.text = "PLAYER 1";

        }
        else
        {
            winnerText.text = "PLAYER 2";
        }

        // Pausar el juego para que dejen de moverse
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        // Es vital regresar el tiempo a 1 antes de recargar
        Time.timeScale = 1f;

        // Recarga la escena activa actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}