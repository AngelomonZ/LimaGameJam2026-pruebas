using UnityEngine;

public class CamaraFollow : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player1 == null || player2 == null) return;
            
        // 1. Calcular el punto medio entre los dos jugadores
        Vector3 puntoMedio = (player1.position + player2.position) / 2f;

        // 2. Aplicar tus límites (Clamp) al punto medio
        float clampX = Mathf.Clamp(puntoMedio.x, minX, maxX);
        float clampY = Mathf.Clamp(puntoMedio.y, minY, maxY);

        // 3. Mover la cámara (manteniendo su propia Z)
        transform.position = new Vector3(clampX, clampY, transform.position.z);
    }
}
