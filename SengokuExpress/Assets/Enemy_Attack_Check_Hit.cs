using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack_Check_Hit : StateMachineBehaviour
{
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
       if (stateInfo.normalizedTime >= 0.5f) {
           animator.SetBool(EACH.check_for_hit, true); // tell code we can now check for player collision with enemy's swing
       }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool(EACH.check_for_hit, false); // reset if code hasn't already
    }
}
