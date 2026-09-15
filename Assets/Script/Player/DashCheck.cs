using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCheck : StateMachineBehaviour
{

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("DashTrigger");
    }
}
