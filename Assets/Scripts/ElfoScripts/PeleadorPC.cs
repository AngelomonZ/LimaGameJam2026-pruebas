using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
public class PeleadorPC : MonoBehaviour
{
    [Header("Configuración de IA")]
    public float rangoDeteccion = 10f;
    public float rangoAtaque = 1.5f;
    public float velocidadClon = 3.5f;
    public LayerMask capaEnemigo;

    [Header("Referencias")]
    private Rigidbody2D rb;
    private Transform objetivo;
     private GolpeMelee sistemaCombate;
    private bool estaAtacando = false;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sistemaCombate = GetComponent<GolpeMelee>();
        anim = GetComponent<Animator>();
        // Buscamos enemigos cada 0.5 segundos para ahorrar procesador
        InvokeRepeating("BuscarObjetivo", 0f, 0.5f);
    }

    void Update()
    {
        if (objetivo == null || estaAtacando)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetBool("Caminanding", false);
            return;
            
        }

        float distancia = Vector2.Distance(transform.position, objetivo.position);

        if (distancia <= rangoAtaque)
        {
            StartCoroutine(SecuenciaAtaque());
        }
        else
        {
            Perseguir();

        }

        GestionarFlip();
    }

    void BuscarObjetivo()
    {
        Collider2D enemigoCercano = Physics2D.OverlapCircle(transform.position, rangoDeteccion, capaEnemigo);
        if (enemigoCercano != null)
        {
            objetivo = enemigoCercano.transform;

        }
        else
        {
            objetivo = null;
        }
    }

    void Perseguir()
    {
        anim.SetBool("Caminanding", true);
        Vector2 direccion = (objetivo.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direccion.x * velocidadClon, rb.linearVelocity.y);
    }

    IEnumerator SecuenciaAtaque()
    {
        estaAtacando = true;
        rb.linearVelocity = Vector2.zero;

        // Llamamos al método Golpear que ya creamos en tu otro script
        sistemaCombate.SendMessage("Golpear");

        yield return new WaitForSeconds(sistemaCombate.tiempoEntreAtaques);
        estaAtacando = false;
    }

    void GestionarFlip()
    {
        if (objetivo.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    // Para ver los rangos en el Editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}
