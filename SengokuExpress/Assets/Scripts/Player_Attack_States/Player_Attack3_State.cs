using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack3_State : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        Debug.Log("started animation attack 1");
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        PAS.on_state_exit(animator, "Attack1");
    }
}
