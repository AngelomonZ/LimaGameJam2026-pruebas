using UnityEngine;
using UnityEngine.InputSystem;

public class Hits : MonoBehaviour
{
    private Animator animator;
    private PlayerInput playerInput;

    // Acciones específicas para combate
    private InputAction softHitAction;
    private InputAction hardHitAction;
    private InputAction parryAction;
    private InputAction grabAction;

    private bool isAttacking = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        // BUSCAMOS LAS ACCIONES EN EL COMPONENTE LOCAL (Igual que en Movement)
        // Asegúrate que estos nombres coincidan con tu Input Actions
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
        // Aquí podrías activar una corrutina para resetear 'isAttacking' tras X tiempo
        // Por ahora, solo lanzamos la animación.

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

    // Método opcional para llamar desde un Animation Event al final de la animación de ataque
    public void FinishAttack()
    {
        isAttacking = false;
    }
}
