using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack3_State : StateMachineBehaviour {
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        if (animator.GetBool(PAS.internal_animation_queued_combo)) {
            animator.SetTrigger("Attack1"); // do attack 1
            animator.SetBool(PAS.internal_animation_queued_combo, false); // reset is_queued
            animator.SetBool(PAS.internal_code_current_combo_finished, true); // tell code to update another combo
        }
    }
}
