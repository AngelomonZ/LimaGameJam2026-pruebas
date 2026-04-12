using UnityEngine;

public class SceneMusicStarter : MonoBehaviour
{
    [Header("Configuración de la Escena")]
    public AudioClip musicToPlay; // Arrastra aquí la canción de ESTA escena

    void Start()
    {
        if (musicToPlay != null)
        {
            // Le decimos al Manager: "Acabamos de entrar a esta escena, pon esta música"
            SoundManager.Instance.PlayMusic(musicToPlay);
        }
    }
}