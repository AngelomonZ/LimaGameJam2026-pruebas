using UnityEngine;

public class GhostSprite : MonoBehaviour
{
    private SpriteRenderer sr;
    public float tiempoDeVida = 0.5f;
    private float tiempoInicio;
    private Color colorActual;

    public void Configurar(Sprite spriteOriginal, Vector3 posicion, Quaternion rotacion, Vector3 escalaOriginal)
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = spriteOriginal;
        
        transform.position = posicion;
        transform.rotation = rotacion;
        // Esto hace que si el padre mide -1 en X, el fantasma también
        transform.localScale = escalaOriginal;
        tiempoInicio = Time.time;
        colorActual = sr.color;
    }

    void Update()
    {
        // Hace que el rastro se vuelva transparente poco a poco
        float tiempoTranscurrido = (Time.time - tiempoInicio) / tiempoDeVida;
        colorActual.a = Mathf.Lerp(0.5f, 0, tiempoTranscurrido);
        sr.color = colorActual;

        if (tiempoTranscurrido >= 1) Destroy(gameObject);
    }
}
