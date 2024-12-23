using System.Collections;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public Animator animator;

    public CombatController combatController;   
    public string animationName;

    public float triggerAtPercent = 50f;

    private bool canAnime = false;

   
    void Update()
    {
        if (!canAnime) { return; }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(animationName))
        {
            float progress = stateInfo.normalizedTime % 1 * 100;
          //  Debug.Log("progress"+progress);
            if (progress >= triggerAtPercent)
            {
                SetAnimTrigger1();
                Debug.Log("Trigger mid!");
                
            }
            if (progress >= 90)
            {

                SetAnimEnd();
                Debug.Log("Trigger end!");

            }
        }
    }
   
    public void SetAnimStart(string animName)
    {
        animator.SetBool(animName, true);
        animationName = "sword";
        canAnime = true;
    }
    public void SetAnimTrigger1()
    {
        combatController.SetAnimTrigger1();
    }
    public void SetAnimEnd()
    {
        combatController.SetAnimEnd();
        SetAnimWalk();
        canAnime = false;
    }
    public void SetAnimWalk()
    {
        animator.SetBool("attack", false);
     }
}
