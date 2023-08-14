using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class DrawSkillEffect : MonoBehaviour
{
    PlayerSkillAttacker skillAttacker;
    List<GameObject> effects = new List<GameObject>();
    [SerializeField] GameObject cubeForLookAt;
    Vector3 startPos;
    Vector3 destination;
    PlayerAimTest aim;

    private void Awake()
    {
        skillAttacker = GetComponent<PlayerSkillAttacker>();
        aim = GetComponent<PlayerAimTest>();
    }

    private void FixedUpdate()
    {
        if (effects.Count > 0)
        {
            Vector3 playerNInst = startPos - effects[5].transform.position;
            Vector3 instVecButYIsZero = new Vector3(playerNInst.x, 0f, playerNInst.z);
            
            //0.2f더한 이유는 너무 바로 사라져서
            if (Mathf.Abs(instVecButYIsZero.x) > Mathf.Abs(destination.x) + 0.2f || Mathf.Abs(instVecButYIsZero.z) > Mathf.Abs(destination.z) + 0.2f)
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
        cubeForLookAt.transform.rotation = Quaternion.LookRotation(transform.position - aim.mousepos);

        for (int i = -5; i <= 5; i++)
        {
            float angle = skillAttacker.skill.angle * (0.2f * i);
            GameObject instance = Instantiate(skillAttacker.skill.effectPrefab, transform.position, cubeForLookAt.transform.rotation * Quaternion.Euler(0f, angle, 0f));
            
            effects.Add(instance);
        }
    }

}
