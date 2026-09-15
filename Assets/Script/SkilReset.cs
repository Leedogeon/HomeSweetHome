using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkilReset : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("SkillTrigger");

        Player player = animator.GetComponent<Player>();
        /*if (player != null)
        {
            player.EndSkill();
        }*/
    }

}
