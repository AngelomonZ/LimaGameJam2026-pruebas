using UnityEngine;
using UnityEngine.Serialization;

public class AgarrePrueba : MonoBehaviour
{
    public Transform controladorGolpe; // Arrastra un objeto vac�o situado frente al jugador
    public Transform radioAgarre;
    public float sizeGolpe;
    public float damage = 20f;
    public float tiempoEntreAtaques = 0.5f;
    private float tiempoSiguienteAtaque = 0f;
    public float posControlador;
    public KeyCode teclAgarre = KeyCode.U;
    private Animator anim;
    [Header("Knockback")]
    public float fuerzaEmpuje = 5f;
    [Header("Configuraci�n de Agarre")]
    public Transform puntoAgarre; // Objeto vac�o donde se posicionar� el enemigo agarrado
    public float fuerzaLanzamiento = 5f;
    private VidaSacoBox enemigoAgarrado; // Referencia al enemigo actual
    public bool tieneAlguien = false;

    private SpriteRenderer sprite;

    [Header("Capa de Enemigos")]
    public LayerMask capaEnemigo; // Selecciona la capa "Enemigo" en el Inspector

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        posControlador = controladorGolpe.position.x;
    }

    void Update()
    {

        // Si presionas 'E' (o el bot�n que elijas)
        if (Input.GetKeyDown(teclAgarre))
        {
            if (!tieneAlguien)
            {
                IntentarAgarrar();
            }
            else
            {
                LanzarEnemigo();
            }
        }
        // Solo ataca si ha pasado el tiempo de cooldown

       

        if (tiempoSiguienteAtaque > 0)
        {
            tiempoSiguienteAtaque -= Time.deltaTime;
        }

    }


   
    public void IntentarAgarrar()
    {
        // Detectamos si hay un enemigo cerca para agarrar
        Collider2D[] detectados = Physics2D.OverlapBoxAll(controladorGolpe.position, radioAgarre.localScale, capaEnemigo);

        foreach (Collider2D col in detectados)
        {
            if (col.gameObject == this.gameObject) continue;

            VidaSacoBox enemigo = col.GetComponent<VidaSacoBox>();
            if (enemigo != null)
            {
                enemigoAgarrado = enemigo;
                tieneAlguien = true;

                // Desactivamos sus f�sicas para que no pelee con el movimiento del jugador
                enemigoAgarrado.GetComponent<Rigidbody2D>().simulated = false;

                // Lo ponemos en el punto de agarre y lo hacemos hijo del jugador
                enemigoAgarrado.transform.position = puntoAgarre.position;
                enemigoAgarrado.transform.SetParent(transform);
                break;
            }
        }
    }

    private void LanzarEnemigo()
    {
        tieneAlguien = false;

        // Lo soltamos (quitamos el padre)
        enemigoAgarrado.transform.SetParent(null);

        // Reactivamos sus fisicas
        Rigidbody2D rbEnemigo = enemigoAgarrado.GetComponent<Rigidbody2D>();
        rbEnemigo.simulated = true;

        // Calculamos el lado opuesto a donde mira el jugador
        // Si usas el Pro-Tip del localScale:
        float direccionLanzamiento = transform.localScale.x > 0 ? -1f : 1f;
        Vector2 fuerzaFinal = new Vector2(direccionLanzamiento, 1.5f).normalized; // 0.5f para que vuele un poco hacia arriba

        // Aplicamos el lanzamiento
        enemigoAgarrado.AplicarKnockback(fuerzaFinal, fuerzaLanzamiento);

        enemigoAgarrado = null;
    }

   
  
}
