using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PAS { // Player_Attack_State is just a namespace for me to compile check animator parameter names
    public static string internal_animation_queued_combo = "internal_queued_combo"; // tell animator that another combo is queued
    public static string internal_code_current_combo_finished = "internal_code_current_combo_finished"; // tell code that we finished current combo
}

public class Player_Attack1_State : StateMachineBehaviour {
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        if (animator.GetBool(PAS.internal_animation_queued_combo)) {
            animator.SetTrigger("Attack2"); // do attack 2
            animator.SetBool(PAS.internal_animation_queued_combo, false); // reset is_queued
            animator.SetBool(PAS.internal_code_current_combo_finished, true); // tell code to update another combo
        }
    }
}
