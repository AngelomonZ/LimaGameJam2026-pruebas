using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashElfo : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Vector2 direccion;

    [Header("Configuración de Dash")]
    public float fuerzaDash = 20f;
    public float tiempoDash = 0.2f;
    public float cooldownDash = 1f;
    public bool puedeDash = true;
    private bool estaDashing = false;
    private GolpeMelee agarreBool;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        GolpeMelee agarreBool = GetComponent<GolpeMelee>();
    }

    void Update()
    {
        // Si está haciendo dash, no procesamos el input normal
        if (estaDashing) return;

        // Capturar Input
        float movX = Input.GetAxisRaw("Horizontal");
        float movY = Input.GetAxisRaw("Vertical");
        direccion = new Vector2(movX, movY).normalized;
        if (agarreBool.tieneAlguien == true)
        {
            puedeDash = false;
        }
        
        // Control del Flip
        if (movX > 0) sprite.flipX = false;
        else if (movX < 0) sprite.flipX = true;

        // Activar Dash (por ejemplo, con Shift Izquierdo o Espacio)
        if (Input.GetKeyDown(KeyCode.LeftShift) && puedeDash)
        {
            StartCoroutine(EjecutarDash());
        }
       

        // 1. Obtienes el script y lo guardas en la variable 'ff'

    }

    void FixedUpdate()
    {
        if (estaDashing) return;
        rb.linearVelocity = direccion * velocidad;
    }

    private IEnumerator EjecutarDash()
    {
        puedeDash = false;
        estaDashing = true;

        // Guardamos la gravedad actual y la desactivamos para un dash recto
        float gravedadOriginal = rb.gravityScale;
        rb.gravityScale = 0;

        // Aplicamos la velocidad del dash en la dirección actual
        // Si no se mueve, dashea hacia donde mira el sprite
        Vector2 direccionDash = direccion != Vector2.zero ? direccion : (sprite.flipX ? Vector2.left : Vector2.right);
        rb.linearVelocity = direccionDash * fuerzaDash;

        yield return new WaitForSeconds(tiempoDash);

        // Restauramos el estado normal
        rb.gravityScale = gravedadOriginal;
        estaDashing = false;

        // Esperamos el tiempo de recarga antes de poder usarlo de nuevo
        yield return new WaitForSeconds(cooldownDash);
        puedeDash = true;
    }
}

