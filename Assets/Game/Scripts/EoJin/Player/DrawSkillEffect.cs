using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class DrawSkillEffect : MonoBehaviour
{
    PlayerSkillAttacker skillAttacker;
    List<GameObject> effects = new List<GameObject>();
    //[SerializeField] GameObject cubeForLookAt;
    Vector3 startPos;
    Vector3 destination;
    PlayerAim aim;

    private void Awake()
    {
        skillAttacker = GetComponent<PlayerSkillAttacker>();
        aim = GetComponent<PlayerAim>();
    }

    private void FixedUpdate()
    {
        if (effects.Count > 0)
        {
            Vector3 playerNInst = startPos - effects[10].transform.position;
            Vector3 instVecButYIsZero = new Vector3(playerNInst.x, 0f, playerNInst.z);
            
            //2.5f더한 이유는 조금 오바되는 게 시각적으로 좋을 것 같아서 이펙트 속도 0.3기준
            if (Mathf.Abs(instVecButYIsZero.x) > Mathf.Abs(destination.x) + 4f || Mathf.Abs(instVecButYIsZero.z) > Mathf.Abs(destination.z) + 4f)
                DestroyAllEffects();
        }
    }

    private void DestroyAllEffects()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            Destroy(effects[i]);
        }
        effects.Clear();
    }

    public void OnEnable()
    {
        skillAttacker.OnSkillStart += EffectStart;
    }

    public void OnDisable()
    {
        skillAttacker.OnSkillStart -= EffectStart;
    }

    Coroutine destroyRoutine;
    private void EffectStart()
    {
        startPos = transform.position;
        Vector3 playerNmouse = (startPos - aim.mousepos).normalized;
        destination = playerNmouse * skillAttacker.skill.range;

        Vector3 dir = (transform.position - aim.mousepos);
        var effectRotation = Quaternion.LookRotation(transform.position - aim.mousepos);

        for (int i = -10; i <= 10; i++)
        {
            float angle = skillAttacker.skill.angle * (0.1f * i);
            GameObject instance = Instantiate(skillAttacker.skill.effectPrefab, transform.position, effectRotation * Quaternion.Euler(0f, angle, 0f));
            
            effects.Add(instance);
        }
    }

}
