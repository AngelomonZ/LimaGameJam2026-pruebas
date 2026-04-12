using System.Collections;
using UnityEngine;

public class VidaBoxeoKB : MonoBehaviour
{
    [Header("Estadisticas")]
    public float vidaMaxima = 10000f;
    private float vidaActual;
    private Rigidbody2D rb;
    private bool recibiendoKnockback = false;
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
        if (recibiendoKnockback) return;

        // Si la dirección tiene fuerza en Y, activamos el salto simulado
        if (Mathf.Abs(direccion.y) > 0.1f)
        {
            StartCoroutine(SaltoPorDano(direccion.y * fuerza));
        }

        // El empuje en X sigue funcionando normal con AddForce
        Vector2 fuerzaHorizontal = new Vector2(direccion.x * fuerza, 0);
        rb.AddForce(fuerzaHorizontal, ForceMode2D.Impulse);
    }

    


    private IEnumerator SaltoPorDano(float fuerzaY)
    {
        Vector3 posicionInicial = transform.position;
        float tiempoAnimacion = 0.15f;
        float transcurrido = 0;

        // SUBIDA
        while (transcurrido < tiempoAnimacion)
        {
            transform.Translate(Vector3.up * fuerzaY * Time.deltaTime);
            transcurrido += Time.deltaTime;
            yield return null;
        }

        transcurrido = 0;

        // BAJADA (Regresa a la posición original)
        while (transcurrido < tiempoAnimacion)
        {
            // Se mueve hacia abajo con la misma fuerza
            transform.Translate(Vector3.down * fuerzaY * Time.deltaTime);
            transcurrido += Time.deltaTime;
            yield return null;
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
