using UnityEngine;
using System.Collections;

public class VidaSacoBox : MonoBehaviour
{
    [Header("Estadisticas")]
    public float vidaMaxima = 10000f;
    private float vidaActual;
    private Rigidbody2D rb;
    void Start()
    {
        vidaActual = vidaMaxima;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TomarDano(float cantidad)
    {
        vidaActual -= cantidad;
        Debug.Log(gameObject.name + " recibir dano. Vida restante: " + vidaActual);
        // Inicia la corrutina del parpadeo
        StartCoroutine(EfectoDanado());

        if (vidaActual <= 0)
        {
            Morir();
        }
    }
    // Esta es la funcion magica para el empuje
    public void AplicarKnockback(Vector2 direccion, float fuerza)
    {
        if (rb != null)
        {
            print("Se esta empujando");
            // Reseteamos la velocidad actual para que el empuje sea limpio
            rb.linearVelocity = Vector2.zero;
            // Aplicamos un impulso instantaneo
            rb.AddForce(direccion * fuerza, ForceMode2D.Impulse);
        }
    }

    private IEnumerator EfectoDanado()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color colorOriginal = sr.color;
            sr.color = Color.red; // Se pone rojo
            yield return new WaitForSeconds(0.1f); // Espera un instante
            sr.color = colorOriginal; // Vuelve a la normalidad
        }
    }

    private void Morir()
    {
        // Aqui podrias instanciar particulas antes de destruir
        Destroy(gameObject);
    }

  
   
}
