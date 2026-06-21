using UnityEngine;
using UnityEngine.InputSystem;

public class Hits : MonoBehaviour
{
    private Animator animator;
    private PlayerInput playerInput;

    // Acciones especificas para combate
    private InputAction softHitAction;
    private InputAction hardHitAction;
    private InputAction parryAction;
    private InputAction grabAction;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        // BUSCAMOS LAS ACCIONES EN EL COMPONENTE LOCAL (Igual que en Movement)
        // Aseg�rate que estos nombres coincidan con tu Input Actions
        softHitAction = playerInput.actions["SoftHit"];
        hardHitAction = playerInput.actions["HardHit"];
        parryAction = playerInput.actions["Parry"];
        grabAction = playerInput.actions["Grab"];
    }

    private void OnEnable()
    {
        // Nos suscribimos
        softHitAction.performed += ctx => PerformAttack("Soft");
        hardHitAction.performed += ctx => PerformAttack("Hard");
        parryAction.performed += ctx => PerformAttack("Parry");
        grabAction.performed += ctx => PerformAttack("Grab");

        softHitAction.Enable();
        hardHitAction.Enable();
        parryAction.Enable();
        grabAction.Enable();
    }

    private void OnDisable()
    {
        // Nos desuscribimos
        softHitAction.performed -= ctx => PerformAttack("Soft");
        hardHitAction.performed -= ctx => PerformAttack("Hard");
        parryAction.performed -= ctx => PerformAttack("Parry");
        grabAction.performed -= ctx => PerformAttack("Grab");

        softHitAction.Disable();
        hardHitAction.Disable();
        parryAction.Disable();
        grabAction.Disable();
    }

    private void PerformAttack(string type)
    {
        // Aqu� podr�as activar una corrutina para resetear 'isAttacking' tras X tiempo
        // Por ahora, solo lanzamos la animaci�n.

        switch (type)
        {
            case "Soft":
               // SoundManager.Instance.PlaySFX();
                AnimationHandler.SoftAttackAnim(animator);
                break;
            case "Hard":
                AnimationHandler.HardAttackAnim(animator);
                break;
            case "Parry":
                AnimationHandler.ParryAnim(animator);
                break;
            case "Grab":
                AnimationHandler.CatchAnim(animator);
                break;
        }
    }
    
}
