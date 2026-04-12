using UnityEngine;
using UnityEngine.InputSystem;

public class LocalControllerSetup : MonoBehaviour
{
    [Header("Arrastra aquí tus personajes de la escena")]
    public PlayerInput player1Input;
    public PlayerInput player2Input;

    void Start()
    {
        // 1. Obtener todos los gamepads conectados
        var gamepads = Gamepad.all;

        // 2. Asignar Mando 1 al Jugador 1
        if (gamepads.Count > 0)
        {
            // Le decimos al PlayerInput: "Usa el esquema 'Gamepad' y este dispositivo específico"
            player1Input.SwitchCurrentControlScheme("Gamepad", gamepads[0]);
            Debug.Log($"Jugador 1 asignado al mando: {gamepads[0].name}");
        }
        else
        {
            Debug.LogWarning("No hay mandos conectados para el Jugador 1. (¿Usando teclado?)");
            // Opcional: Asignar teclado si no hay mando
            // player1Input.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);
        }

        // 3. Asignar Mando 2 al Jugador 2
        if (gamepads.Count > 1)
        {
            player2Input.SwitchCurrentControlScheme("Gamepad", gamepads[1]);
            Debug.Log($"Jugador 2 asignado al mando: {gamepads[1].name}");
        }
        else
        {
            Debug.LogWarning("Falta el segundo mando para el Jugador 2.");
            // Aquí podrías desactivar al P2 o ponerlo en modo "CPU" si tuvieras IA
            // player2Input.DeactivateInput(); 
        }
    }
}