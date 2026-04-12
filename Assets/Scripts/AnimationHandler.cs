using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public static void MovementAnim(Animator animator, Vector2 movement) => animator.SetFloat("Speed", Mathf.Abs(movement.x));
    public static void DashAnim(Animator animator) => animator.SetTrigger("TriggerDash");
    public static void SoftAttackAnim(Animator animator) => animator.SetTrigger("TriggerAttack1");
    public static void HardAttackAnim(Animator animator) => animator.SetTrigger("TriggerAttack2");
    public static void CatchAnim(Animator animator) => animator.SetTrigger("TriggerCatch");
    public static void ParryAnim(Animator animator) => animator.SetTrigger("TriggerParry");
}
