using UnityEngine;

public class Detection : MonoBehaviour
{
    [SerializeField] private Transform headDetector;
    [SerializeField] private Transform bodyDetector;
    [SerializeField] private Transform parryDetector;
    [SerializeField] private Transform armDetector;
    [SerializeField] private Transform agarreDetector;




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hitBox"))
        {
            Debug.Log("Enemigo Detectado");
        }
        if (collision.CompareTag("hurtBox"))
        {
            Debug.Log("Enemigo Detectado");
        }
    }

}
