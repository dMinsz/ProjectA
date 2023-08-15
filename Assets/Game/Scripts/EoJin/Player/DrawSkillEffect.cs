using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class DrawSkillEffect : MonoBehaviour
{
    [SerializeField] DataManager data;
    PlayerSkillAttacker skillAttacker;
    List<GameObject> effectClones = new List<GameObject>();
    [SerializeField] GameObject cubeForLookAt;
    Vector3 startPos;
    Vector3 destination;
    PlayerAimTest aim;

    private void Awake()
    {
        data = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
        skillAttacker = GetComponent<PlayerSkillAttacker>();
        aim = GetComponent<PlayerAimTest>();
    }

    private void Update()
    {
        if (effectClones.Count > 0)
        {
            Vector3 playerNInst = startPos - effectClones[10].transform.position;
            Vector3 instVecButYIsZero = new Vector3(playerNInst.x, 0f, playerNInst.z);
            
            //2.5f더한 이유는 조금 오바되는 게 시각적으로 좋을 것 같아서 이펙트 속도 0.3기준
            if (Mathf.Abs(instVecButYIsZero.x) > Mathf.Abs(destination.x) + data.CurCharacter.skillEffect.additionalTime || Mathf.Abs(instVecButYIsZero.z) > Mathf.Abs(destination.z) + data.CurCharacter.skillEffect.additionalTime)
                DestroyAllEffects();
        }
    }

    private void DestroyAllEffects()
    {
        for (int i = 0; i < effectClones.Count; i++)
        {
            Destroy(effectClones[i]);
        }
        effectClones.Clear();
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

        for (int i = -10; i <= 10; i++)
        {
            float angle = skillAttacker.skill.angle * (0.1f * i);
            GameObject instance = Instantiate(data.CurCharacter.skillEffect.effectPrefab, transform.position, cubeForLookAt.transform.rotation * Quaternion.Euler(0f, angle, 0f));
            
            effectClones.Add(instance);
        }
    }

}
