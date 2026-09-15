using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AttackReset : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("AttackTrigger");

        Player player = animator.GetComponent<Player>();
        if (player != null)
        {
            player.isAttack = false;
            player.EndAttack();
        }

    }
}
