using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Fuentes de Audio")]
    [SerializeField] private AudioSource musicSource; // Arrastra el AudioSource para música aquí
    [SerializeField] private AudioSource sfxSource;   // Arrastra el AudioSource para efectos aquí

    private void Awake()
    {
        // Singleton: Solo puede haber un SoundManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- MÚSICA (BGM) ---
    public void PlayMusic(AudioClip clip)
    {
        // Si ya está sonando esa misma canción, no la reinicies
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = true; // La música de fondo siempre debe loopear
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        musicSource.clip = null; // Limpiamos la referencia
    }

    // --- EFECTOS DE SONIDO (SFX) ---
    public void PlaySFX(AudioClip clip)
    {
        // PlayOneShot permite que suenen varios efectos a la vez sin cortarse
        sfxSource.PlayOneShot(clip);
    }
}