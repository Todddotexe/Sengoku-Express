using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack1_State : StateMachineBehaviour {
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        PAS.on_state_exit(animator, "Attack2");
    }
}
