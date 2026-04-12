using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeCharacter : MonoBehaviour
{
    [Header("Player 1 Components")]
    [SerializeField] MovementScript player1Movement;
    [SerializeField] Hits player1Hits; 
    [Header("Player 2 Components")]
    [SerializeField] MovementScript player2Movement;
    [SerializeField] Hits player2Hits;

    private void Start()
    {
        // Inicializamos: P1 Activo, P2 Apagado
        SetPlayerState(true, false);
    }

    private void Update()
    {
        // Usamos el botón 'Select' (o el que prefieras) para cambiar
        bool gamepadSwitch = Gamepad.current != null && Gamepad.current.rightTrigger.wasPressedThisFrame;

        if (gamepadSwitch) SwitchPlayer();
    }

    private void SwitchPlayer()
    {
        // Invertimos el estado: Si P1 está encendido, pasa a estar apagado
        bool isP1Active = !player1Movement.enabled;

        SetPlayerState(isP1Active, !isP1Active);

        if (isP1Active) Debug.Log("Controlando al JUGADOR 1");
        else Debug.Log("Controlando al JUGADOR 2");
    }

    // Función auxiliar para activar/desactivar todo junto
    private void SetPlayerState(bool p1State, bool p2State)
    {
        // Player 1
        player1Movement.enabled = p1State;
        player1Hits.enabled = p1State; // Al apagar esto, el Update de Hits deja de correr

        // Player 2
        player2Movement.enabled = p2State;
        player2Hits.enabled = p2State;
    }
}
