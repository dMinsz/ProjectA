using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class DrawSkillEffect : MonoBehaviour
{
    PlayerSkillAttacker skillAttacker;
    [SerializeField] List<GameObject> effects = new List<GameObject>();
    [SerializeField] GameObject cubeForLookAt;
    [SerializeField] int effectsNum; //두 배가 들어감
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
            Vector3 playerNInst = startPos - effects[effectsNum].transform.position;
            Vector3 instVecButYIsZero = new Vector3(playerNInst.x, 0f, playerNInst.z);
            
            if (Mathf.Abs(instVecButYIsZero.x) > Mathf.Abs(destination.x) || Mathf.Abs(instVecButYIsZero.z) > Mathf.Abs(destination.z))
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

        for (int i = -effectsNum; i <= effectsNum; i++)
        {
            float angle = skillAttacker.skill.angle * (0.25f * i);
            GameObject instance = Instantiate(skillAttacker.skill.effectPrefab, transform.position, cubeForLookAt.transform.rotation * Quaternion.Euler(0f, angle, 0f));
            
            effects.Add(instance);
        }
    }

}
