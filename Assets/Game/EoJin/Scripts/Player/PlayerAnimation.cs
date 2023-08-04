using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] PlayerAttacker attacker;
    [SerializeField] AnimatorController controller;

    public void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        attacker = gameObject.GetComponent<PlayerAttacker>();
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
                state.state.motion = attacker.skill.skillAnimation;
                break;
            }
        }
    }
}

