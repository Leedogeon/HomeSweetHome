using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamaged : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Enemy enemy = animator.GetComponent<Enemy>();
        enemy.isMove = false;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Enemy enemy = animator.GetComponent<Enemy>();
        if(enemy.Hp > 0)
        {
            enemy.isMove = true;
            animator.ResetTrigger("Damaged");
        }

    }
}
