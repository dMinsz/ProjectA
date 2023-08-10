using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine.Events;

public class PlayerAttacker : MonoBehaviour
{
    public Skill skill;
    DataManager data;
    [SerializeField] bool debug;
    [SerializeField] float control; //플레이어 위치에서 얼마나 떨어진 거리에서 어택 범위 발동할 지
    float rangeAmount;
    float angle;
    bool isAttack = false;
    public UnityAction OnPlaySkillAnim;
    public UnityAction<GameObject> OnPlayerAttack;
    [SerializeField] PlayerAimTest aim;
    [SerializeField] public GameObject mousePosObj;

    public void Awake()
    {
        data = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
        aim = gameObject.GetComponent<PlayerAimTest>();
    }

    public void Update()
    {
        if (isAttack)
            ApplyDamage();

        mousePosObj.transform.position = aim.mousepos;
    }

    Coroutine ApplyDamageRoutine;

    public void OnPrimarySkill(InputValue value)
    {
        skill = data.CurCharacter.primarySkill;
        isAttack = true;
        ApplyDamageRoutine = StartCoroutine(skillDuration());
    }

    public void OnSecondarySkill (InputValue value)
    {
        skill = data.CurCharacter.secondarySkill;
        isAttack = true;
        ApplyDamageRoutine = StartCoroutine(skillDuration());
    }

    public void OnSpecailSkill(InputValue value)
    {
        skill = data.CurCharacter.specialSkill;
        isAttack = true;
        ApplyDamageRoutine = StartCoroutine(skillDuration());
    }

    IEnumerator skillDuration()
    {
        yield return new WaitForSeconds(skill.duration);
        isAttack = false;
    }

    public void ApplyDamage()
    {
        if (skill == null)
            return;


    
        rangeAmount = skill.range;

        
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, skill.range);

        foreach (Collider collider in colliders)
        {
            Vector3 playerNMouse = (transform.position - aim.mousepos).normalized;
            Vector3 playerNTarget = (collider.transform.position - transform.position).normalized;
            Vector3 dirTarget = (collider.transform.position - transform.position).normalized;

            if (!(collider.tag == "Player"))
                continue;

            if (Vector3.Dot(-playerNMouse, playerNTarget) < Mathf.Cos(angle * Mathf.Deg2Rad))
                continue;

            if (collider.isTrigger == true)
                continue;

            /*
            if (collider.gameObject.layer == 7)
            {
                aim.Attack();
                Debug.Log($"{collider.gameObject.name}에게 PlayerAim.Attack");
                return;
            }
            */

            //collider.GetComponent<PlayerGetDamage>().GetDamaged(this.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!debug)
            return;

        //Handles.color = Color.cyan;
        //Handles.DrawSolidArc(transform.position, Vector3.up, (aim.mousepos - transform.position).normalized, -angle, rangeAmount);
        //Handles.DrawSolidArc(transform.position, Vector3.up, (aim.mousepos - transform.position).normalized, angle, rangeAmount);
    }
}
