using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] PlayerSkillAttacker attacker;
    [SerializeField] AnimatorController controller;

    public void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        attacker = gameObject.GetComponent<PlayerSkillAttacker>();
    }

    public void OnEnable()
    {
        attacker.OnPlaySkillAnim += PlaySkillAnim;
    }

    public void OnDisable()
    {
        attacker.OnPlaySkillAnim -= PlaySkillAnim;
    }

    public void PlaySkillAnim()
    {
        ChangeSkillAnim();
        anim.SetTrigger("Skill");
    }

    public void ChangeSkillAnim()
    {
        AnimatorControllerLayer layer = controller.layers[0];
        AnimatorStateMachine machine = layer.stateMachine;
        ChildAnimatorState[] states = machine.states;
        foreach (ChildAnimatorState state in states)
        {
            if (state.state.name == "Skill")
            {
                if (state.state.motion != attacker.skill.skillAnimation)
                {
                    state.state.motion = attacker.skill.skillAnimation;
                    Invoke("SetSkillTrigger", 0.1f); //motion을 바꾸자마자 setTrigger을 하면 씹히기 때문에 invoke로 시간차를 줌
                }
                break;
            }
        }
    }

    public void SetSkillTrigger()
    {
        anim.SetTrigger("Skill");
    }

}

