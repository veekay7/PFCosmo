using System;
using System.Collections;
using UnityEngine;

public class CosmoAnimBehaviour : MonoBehaviour
{
    public PlayerCharaCosmo owner { get; set; }
    public Animator animator { get; private set; }


    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }


    private void LateUpdate()
    {
        if (animator == null || owner == null)
            return;

        // TODO:
        // Set animator to do schit!
        animator.SetBool("OnGround", owner.onGround);
        //animator.SetBool("IsJump", owner.movementMode == CosmoMovementComponent.ECosmoMovementMode.Jump);
        animator.SetBool("IsJump", owner.isJumping || owner.isLaunched);
        animator.SetFloat("Xsp", owner.velocity.x);
        animator.SetFloat("Ysp", owner.velocity.y);
    }


    public IEnumerator Co_PlayVictoryAnim(Action onComplete = null)
    {
        if (owner == null)
            yield break;

        //// Wait till Cosmo is on the ground
        //yield return new WaitUntil(() => owner.onGround);

        // Play the animation
        animator.SetBool("IsVictory", true);
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        if (onComplete != null)
            onComplete.Invoke();
    }
}
