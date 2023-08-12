using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class DrawSkillEffect : MonoBehaviour
{
    PlayerSkillAttacker skillAttacker;
    [SerializeField] List<Effect> effects = new List<Effect>();
    [SerializeField] GameObject cubeForLookAt;
    Vector3 startPos;
    Vector3 destination;

    class Effect
    {
        public GameObject instance;
        public Vector3 destination;
        public bool isProjectile; //발사체인지 만약 발사체라면 여러개를 발사해 부채꼴로 만듦

        public Effect(GameObject go, bool b) 
        {
            this.instance = go;
            this.isProjectile = b;
        }
    }

    PlayerAimTest aim;

    Vector3 EffectStartPos;
  
    private void Awake()
    {
        skillAttacker = GetComponent<PlayerSkillAttacker>();
        aim = GetComponent<PlayerAimTest>();
    }

    private void FixedUpdate()
    {
        if (effects.Count > 5)
        {
            Vector3 playerNInst = startPos - effects[5].instance.transform.position;
            Vector3 instVecButYIsZero = new Vector3(playerNInst.x, 0f, playerNInst.z);
            
            if (Mathf.Abs(instVecButYIsZero.x) > Mathf.Abs(destination.x) || Mathf.Abs(instVecButYIsZero.z) > Mathf.Abs(destination.z))
                DestroyAllEffects();
        }
    }

    private void DestroyAllEffects()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            Destroy(effects[i].instance);
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
            float angle = skillAttacker.skill.angle * (0.25f * i);
            GameObject instance = Instantiate(skillAttacker.skill.effectPrefab, transform.position, cubeForLookAt.transform.rotation * Quaternion.Euler(0f, angle, 0f));
            
            Effect effect = new Effect(instance, skillAttacker.skill.isProjectileEffect);
            effects.Add(effect);
        }
    }

}
