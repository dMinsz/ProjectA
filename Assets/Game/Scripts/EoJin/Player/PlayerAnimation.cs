using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] PlayerSkillAttacker attacker;
    [SerializeField] RuntimeAnimatorController controller;
    [SerializeField] Skill curSkill;

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
        if (attacker.skill.key == Skill.Key.Primary)
        {
            anim.SetTrigger("Primary");
        }
        else if (attacker.skill.key == Skill.Key.Secondary)
        {
            anim.SetTrigger("Secondary");
        }
        else
        {
            anim.SetTrigger("Special");
        }
    }

}

