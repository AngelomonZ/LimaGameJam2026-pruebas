using System.Collections;
using UnityEngine;

public class MovePrueba : MonoBehaviour
{

    [Header("Movimiento")]
    public float velocidad = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Vector2 direccion;
    [Header("Dash")]
    public float fuerzaDash = 20f;
    public float tiempoDash = 0.2f;
    public float cooldownDash = 1f;
    public bool puedeDash = true;
    private bool estaDashing = false;
    public AgarrePrueba agarreBool;
    [Header("Ghost Effect")]
    public GameObject ghostPrefab; // Arrastra un Prefab con SpriteRenderer y el script GhostSprite
    public float intervaloGhost = 0.05f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        AgarrePrueba agarreBool = GetComponent<AgarrePrueba>();
    }

    // Update is called once per frame
    void Update()
    {
        if (estaDashing) return;

        float movX = Input.GetAxisRaw("Horizontal2");

        direccion = new Vector2(movX, 0).normalized;

      
       
        if (movX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movX < 0) transform.localScale = new Vector3(-1, 1, 1);
        if (agarreBool.tieneAlguien == true)
        {
            puedeDash = false;
        }
        else puedeDash = true;
        if (Input.GetKeyDown(KeyCode.H) && puedeDash && movX != 0)
        {
            StartCoroutine(EjecutarDash());
        }
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
   
        float gravedadOriginal = rb.gravityScale;
        rb.gravityScale = 0;

        Vector2 direccionDash = direccion != Vector2.zero ? direccion : (sprite.flipX ? Vector2.left : Vector2.right);
        rb.linearVelocity = direccionDash * fuerzaDash;

        // --- L�gica de fantasmas ---
        float tiempoPasado = 0;

        while (tiempoPasado < tiempoDash)
        {
            // Crea un fantasma
            GameObject ghost = Instantiate(ghostPrefab);

            ghost.GetComponent<GhostSprite>().Configurar(sprite.sprite, transform.position, transform.rotation, transform.localScale);

            yield return new WaitForSeconds(intervaloGhost);
            tiempoPasado += intervaloGhost;
        }
        // ---------------------------

        rb.gravityScale = gravedadOriginal;
        estaDashing = false;

        yield return new WaitForSeconds(cooldownDash);
        puedeDash = true;
    }
}
