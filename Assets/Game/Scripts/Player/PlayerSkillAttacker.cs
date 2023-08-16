using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerSkillAttacker : MonoBehaviour
{
    /*
     * ������ �ݸ� ����ȭ�� �޽��� ���� ť�긦 �������� �ۼ� �����Ƿ� ������ prefab�� �ִ� ť�꿡 �����ų ��
     */

    public Skill skill;
    [SerializeField] bool debug;
    [SerializeField] float control; //�÷��̾� ��ġ���� �󸶳� ������ �Ÿ����� ���� ���� �ߵ��� ��
     public float range;
    float additionalRange;
    public float angle;
    public bool isSkillingPrimary = false;
    public bool isSkillingSecondary = false;
    public bool isSkillingSpecial = false;
    public bool canSkillPrimary = true;
    public bool canSkillSecondary = true;
    public bool canSkillSpecial = true;
    bool isPlayingSkillAnim = false;
    public UnityAction OnPlaySkillAnim;
    public UnityAction OnSkillStart;
    public UnityAction<GameObject, float> OnPlayerAttack;
    [SerializeField] PlayerAim aim;

    float time;
    float coolTimeP;
    float coolTimeS;
    float coolTimeSP;

    private bool draw;

    [SerializeField] DrawSkillRange DrawRange;
    private Animator anim;
    private Transform RotatedPos;

    //����
    public playercontroll playerdash;
    [SerializeField] public bool isskilling= false;
    public Vector3 skilldir;

    private Character curCharacter;

    private AudioSource voice;
    public void Awake()
    {
        voice = GetComponent<AudioSource>();
        aim = gameObject.GetComponent<PlayerAim>();
        anim = GetComponent<Animator>();

        RotatedPos = new GameObject("RoatateObjectforAttacker").transform;


        if (GameManager.Data.CurCharacter == null)
        {//for debug
            curCharacter = GameManager.Data.characters[1];
        }
        else
        {
            curCharacter = GameManager.Data.CurCharacter;
        }
    }

    public void Update()
    {
       // RotatedPos.LookAt(aim.mousepos);
        //cubeForLookAt.transform.LookAt(aim.mousepos);
        //lookAtMouse = cubeForLookAt.transform.rotation;

        //mousePosObj.transform.position = aim.mousepos;
    }

    Coroutine primarySkillCoroutine;
    Coroutine secondarySkillCoroutine;
    Coroutine specialSkillCoroutine;


    [SerializeField]  bool isQDubleClick = false;
    [SerializeField] bool isEDubleClick = false;
    [SerializeField] bool isRDubleClick = false;

    public void OnPrimarySkill(InputValue value)
    {

        if (canSkillPrimary) //�� skill�� �ߵ��Ǵ� ���� �ٸ� skill�� �� ���� ����
        {

            skill = curCharacter.primarySkill;
            aim.attacksize = skill.range;
            var damage = curCharacter.secondarySkill.damage;

            DrawRange.SetIsDrawingTrue();

            isEDubleClick = false;
            isRDubleClick = false;

            if (isQDubleClick)
            {
                StartCoroutine(skilldirRoutin());
                anim.SetTrigger("Primary");
                canSkillPrimary = false;
                isSkillingPrimary = true;
                ApplyDamage(damage , 0 , aim.mousepos);

                isQDubleClick = false;

                if (curCharacter.primarySkill.isDash == true)
                {
                    playerdash.Dash();
                    isQDubleClick = false;
                }



                DrawRange.SetIsDrawingFalse();
                primarySkillCoroutine = StartCoroutine(skillCoolTimePrimary());
                
            }
            else 
            {
                isQDubleClick = true;
            }

            voice.Play();

        }

    }

    public void OnSecondarySkill(InputValue value)
    {

        if (canSkillSecondary) //�� skill�� �ߵ��Ǵ� ���� �ٸ� skill�� �� ���� ����
        {

            skill = curCharacter.secondarySkill;
            aim.attacksize = skill.range;

            var damage = curCharacter.secondarySkill.damage;

            DrawRange.SetIsDrawingTrue();


            isQDubleClick = false;
            isRDubleClick = false;

            if (isEDubleClick)
            {
                StartCoroutine(skilldirRoutin());
                anim.SetTrigger("Secondary");
                canSkillSecondary = false;
                isSkillingSecondary = true;
                ApplyDamage(damage,1, aim.mousepos);

                isEDubleClick = false;


                if (curCharacter.secondarySkill.isDash == true)
                {
                    playerdash.Dash();
                    isEDubleClick = false;
                }



                DrawRange.SetIsDrawingFalse();
                secondarySkillCoroutine = StartCoroutine(skillCoolTimeSecondary());
                //primarySkillCoroutine = StartCoroutine(skillDurationSecondary());
            }
            else
            {
                isEDubleClick = true;
            }
            voice.Play();
        }
    }

    public void OnSpecailSkill(InputValue value)
    {
        if (canSkillSpecial) //�� skill�� �ߵ��Ǵ� ���� �ٸ� skill�� �� ���� ����
        {

            skill = curCharacter.specialSkill;
            aim.attacksize = skill.range;

            var damage = curCharacter.specialSkill.damage;

            DrawRange.SetIsDrawingTrue();

            isQDubleClick = false;
            isEDubleClick = false;

            if (isRDubleClick)
            {
                
                anim.SetTrigger("Special");
                canSkillSpecial = false;
                isSkillingSpecial = true;
                ApplyDamage(damage,2, aim.mousepos);

                isRDubleClick = false;

                if (curCharacter.specialSkill.isDash == true) 
                {
                    playerdash.Dash();
                    isRDubleClick = false;
                }


                DrawRange.SetIsDrawingFalse();
                specialSkillCoroutine = StartCoroutine(skillCoolTimeSpecial());
            }
            else
            {
                isRDubleClick = true;
            }
            voice.Play();
        }

    }

    IEnumerator skilldirRoutin()
    {
        isskilling = true;
        skilldir = aim.mousepos;
        transform.LookAt(skilldir);
        yield return new WaitForSeconds(0.5f);
        isskilling = false;
    }



    IEnumerator skillDurationPrimary()
    {
        
        yield return new WaitForSeconds(skill.duration);
        isSkillingPrimary = false;
        isPlayingSkillAnim = false;

        primarySkillCoroutine = StartCoroutine(skillCoolTimePrimary());
    }

    IEnumerator skillDurationSecondary()
    {
        yield return new WaitForSeconds(skill.duration);
        isSkillingSecondary = false;
        isPlayingSkillAnim = false;

        secondarySkillCoroutine = StartCoroutine(skillCoolTimeSecondary());
    }

    IEnumerator skillDurationSpecial()
    {
        yield return new WaitForSeconds(skill.duration);
        isSkillingSpecial = false;
        isPlayingSkillAnim = false;

        specialSkillCoroutine = StartCoroutine(skillCoolTimeSpecial());
    }

    IEnumerator skillCoolTimePrimary()
    {
        /*   coolTimeP = 1;
           while (skill.coolTime >= coolTimeP)
           {
               yield return new WaitForSeconds(1f);
               Debug.Log($"Primary : {skill.skillName}�� ��Ÿ�� ��� {coolTimeP} / {skill.coolTime}");
               coolTimeP += 1;
           }*/
        yield return new WaitForSeconds(skill.coolTime);

        //while�� ���� �� �Ʒ� ���� �ּ� ����� ��
        //yield return new WaitForSeconds(skill.coolTime);
        isSkillingPrimary = false;
        canSkillPrimary = true;
    }

    IEnumerator skillCoolTimeSecondary()
    {
        /*  coolTimeS = 1;
          while (skill.coolTime >= coolTimeS)
          {
              yield return new WaitForSeconds(1f);
              Debug.Log($"Secondary : {skill.skillName}�� ��Ÿ�� ��� {coolTimeS} / {skill.coolTime}");
              coolTimeS += 1;
          }
        */

        yield return new WaitForSeconds(skill.coolTime);
        //while�� ���� �� �Ʒ� ���� �ּ� ����� ��
        //yield return new WaitForSeconds(skill.coolTime);
        isSkillingSecondary =false;
        canSkillSecondary = true;
    }

    IEnumerator skillCoolTimeSpecial()
    {
        /* coolTimeSP = 1;
         while (skill.coolTime >= coolTimeSP)
         {
             yield return new WaitForSeconds(1f);
             Debug.Log($"Special : {skill.skillName}�� ��Ÿ�� ��� {coolTimeSP} / {skill.coolTime}");
             coolTimeSP += 1;
         }*/
        yield return new WaitForSeconds(skill.coolTime);

        //while�� ���� �� �Ʒ� ���� �ּ� ����� ��
        //yield return new WaitForSeconds(skill.coolTime);
        isSkillingSpecial =false;
        canSkillSpecial = true;
    }

    public void ApplyDamage(int damage , int skillnum , Vector3 mousepos)
    {
        OnSkillStart?.Invoke();


        GetComponent<DrawSkillEffect>().EffectStart(skillnum,mousepos);

        if (skill == null)
            return;

        if (!isPlayingSkillAnim)
        {
            OnPlaySkillAnim?.Invoke();
            isPlayingSkillAnim = true;
        }

        angle = skill.angle;
        range = skill.range;
        additionalRange = skill.additionalRange;

        if (skill.rangeStyle == Skill.RangeStyle.Square)
            MakeSkillRangeSquareForm(damage);
        else
            MakeSkillRangeSectorForm(damage);

    }

    private void MakeSkillRangeSquareForm(int damage)
    {
        float angle = Vector3.Angle(transform.position, aim.mousepos);
        //additionalRange�� float�� �����ָ� ��ų���� �ΰ��� ��������
        Vector3 boxSize = new Vector3(additionalRange * 0.5f, 0.1f, range);


        Vector3 newPos = new Vector3(transform.position.x, 1.5f, transform.position.z);
        Vector3 dir = (aim.mousepos - newPos).normalized;

        RotatedPos.forward = new Vector3(dir.x, 0, dir.z);
        Collider[] colliders = Physics.OverlapBox(gameObject.transform.position, boxSize, RotatedPos.rotation);
        DetectObjectsCollider(colliders,damage);
    }

    private void OnDrawGizmos()
    {
        //Style Square�� Gizmos�� ��� �÷��̾��� �޺κб��� �׷����� ���� Skill ������ Gizmos�� �ݿ� �÷��̾� ������ ����

        if (skill == null || !debug || skill.rangeStyle != Skill.RangeStyle.Square || skill.rangeStyle != Skill.RangeStyle.Square)
            return;

        Gizmos.color = Color.cyan;

        Vector3 boxSize = new Vector3(additionalRange, 0.1f, range * 2);



        Vector3 newPos = new Vector3(transform.position.x, 1.5f, transform.position.z);
        Vector3 dir = (aim.mousepos - newPos).normalized;

        RotatedPos.forward = new Vector3(dir.x, 0, dir.z);


        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, RotatedPos.transform.rotation, new Vector3(1f, 1f, 1f));
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawCube(new Vector3(0f, 0f, 0f), boxSize);
    }


    private void MakeSkillRangeSectorForm(int damage)
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, skill.range);
        DetectObjectsCollider(colliders,damage);
    }

    private void DetectObjectsCollider(Collider[] colliders , int damage)
    {
        
        foreach (Collider collider in colliders)
        {
            Vector3 playerNMouse = (aim.mousepos - transform.position).normalized;
            Vector3 colliderPosButYIsZero = new Vector3(collider.transform.position.x, 0f, collider.transform.position.z);
            Vector3 playerNTarget = (colliderPosButYIsZero - transform.position).normalized;

            if (skill.rangeStyle == Skill.RangeStyle.Square)
                angle = 90f;

            if (Vector3.Dot(playerNMouse, playerNTarget) < Mathf.Cos(angle * Mathf.Deg2Rad))
                continue;

            if (collider.gameObject == this.gameObject)
                continue;

            if (collider.gameObject.layer == LayerMask.NameToLayer("Ball"))
            {
                aim.Attack(aim.attackdir);
                continue;
            }

            if (collider.gameObject.tag == "Player")
            {
                if (collider.gameObject.GetComponent<PlayerGetDamage>().damaged == false)
                {
                    collider.gameObject.GetComponent<PlayerGetDamage>().GetDamaged(this.gameObject, skill.duration , damage);
                }
            }
        }
        

    }

}