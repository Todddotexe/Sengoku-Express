using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PAS { // Player_Attack_State is just a namespace for me to compile check animator parameter names
    public static string internal_animation_queued_combo = "internal_queued_combo"; // tell animator that another combo is queued
    public static string internal_code_current_combo_finished = "internal_code_current_combo_finished"; // tell code that we finished current combo
    public static void on_state_exit(Animator animator, string next_combo) {
        // -- manually tell player's combat that current attack has finished animating
        Global.player_controller.combat.toggle_attack_current_combo_finished = true;
    }
}

public class Player_Attack1_State : StateMachineBehaviour {
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        PAS.on_state_exit(animator, "Attack2");
    }
}
