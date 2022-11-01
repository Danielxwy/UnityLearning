using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�������Trigger�źŵģ�Ŀǰ���ã�
public class ResetSignal : StateMachineBehaviour
{
    public string[] clearAtEnter;
    public string[] clearAtExit;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach(var signal in clearAtEnter)
        {
            animator.ResetTrigger(signal);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach(var signal in clearAtExit)
        {
            animator.ResetTrigger(signal);
        }
    }

}
