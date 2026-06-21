using UnityEngine;
using UnityEngine.Serialization;

public class GolpeMelee : MonoBehaviour
{
    [Header("Configuraci�n de Ataque")]
    public Transform controladorGolpe; // Arrastra un objeto vacio situado frente al jugador
    public Transform radioGolpe;
    public Transform radioAgarre;
    public float sizeGolpe;
    public float daño = 20f;
    public float tiempoEntreAtaques = 0.5f;
    private float tiempoSiguienteAtaque = 0f;
    public float posControlador;
    public KeyCode teclAtaque = KeyCode.E;
    public KeyCode teclAgarre = KeyCode.Q;
    private Animator anim;
    
    [Header("Knockback")]
    public float fuerzaEmpuje = 5f;
    
    [Header("Configuración de Agarre")]
    public Transform puntoAgarre; // Objeto vacio donde se posicionar� el enemigo agarrado
    public float fuerzaLanzamiento = 5f;
    private VidaSacoBox enemigoAgarrado; // Referencia al enemigo actual
    public bool tieneAlguien = false;

    private SpriteRenderer sprite;

    [Header("Capa de Enemigos")]
    public LayerMask capaEnemigo; // Selecciona la capa "Enemigo" en el Inspector

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        posControlador= controladorGolpe.position.x;
        anim = GetComponent<Animator>();
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

        if (Input.GetKeyDown(teclAtaque) && tiempoSiguienteAtaque<=0 && tieneAlguien == false) 
            { 
                Golpear();
                print("Golpeando");
                tiempoSiguienteAtaque = tiempoEntreAtaques;
               
            }

        if (tiempoSiguienteAtaque > 0)
        {
            tiempoSiguienteAtaque -= Time.deltaTime;
        }

    }
  

    private void Golpear()
    {

        // Opcional: Activar animaci�n de ataque aqu�
        anim.SetTrigger("Atacando");

        // Detectar enemigos en el rango de golpe
        Collider2D[] objetosDetectados = Physics2D.OverlapBoxAll(controladorGolpe.position, radioGolpe.localScale, capaEnemigo);

        foreach (Collider2D enemigo in objetosDetectados)
        {
            if (enemigo.gameObject == this.gameObject) continue;
            Debug.Log("Golpeaste a: " + enemigo.name);
          

            // 1. Obtienes el script y lo guardas en la variable 'ff'
            VidaBoxeoKB ff = enemigo.GetComponent<VidaBoxeoKB>();

            // 2. Verificas que 'ff' (el script) no sea nulo
            if (ff != null)
            {
                // 3. LLAMAS AL MeTODO DESDE 'ff', NO DESDE 'enemigo'
                ff.TomarDano(20f);
                float diferenciaY = ff.transform.position.y - transform.position.y;
                Vector2 dir = new Vector2(0, 1); // Empuje puramente hacia arriba
                ff.AplicarKnockback(dir, fuerzaEmpuje);

            }
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

        // Reactivamos sus f�sicas
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

    // Dibuja el c�rculo en el editor para que puedas ajustarlo visualmente
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(controladorGolpe.position, radioGolpe.localScale);
    }
}



