using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSkillRange : MonoBehaviour
{
    [SerializeField] GameObject cube;
    [SerializeField] GameObject cube2;
    [SerializeField] float thickness;
    [SerializeField] Color color;
    PlayerSkillAttacker attacker;
    PlayerAim aim;
    bool isDrawing;

    public void Awake()
    {
        attacker = gameObject.GetComponent<PlayerSkillAttacker>();
        aim = gameObject.GetComponent<PlayerAim>();

        cube.SetActive(false);
        cube2.SetActive(false);
    }

    public void Update()
    {
        if (isDrawing)
            Draw();
        else
            SetCubeActiveFalse();
    }

    public void OnEnable()
    {
        attacker.OnSkillStart += SetIsDrawingTrue;
        attacker.OnSkillEnd += SetIsDrawingFalse;
    }

    public void OnDisable()
    {
        attacker.OnSkillStart -= SetIsDrawingTrue;
        attacker.OnSkillEnd -= SetIsDrawingFalse;
    }

    public void Draw()
    {
        if (attacker.angle == 180)
        {
            cube.SetActive(false);
            cube2.SetActive(false);
            return;
        }
        
        cube.SetActive(true);
        cube2.SetActive(true);

        cube.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", color);
        cube2.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", color);
        //��ų���� ���� �ٸ��� �����ϴ� �� �ƴ� �÷��̾�� ������ ��ų �� �ϳ��� �ִ� ���� SetColor�� Awake�� �ű� ��

        cube.transform.localScale = new Vector3(thickness, 0.1f, attacker.skill.range * -2);
        cube2.transform.localScale = new Vector3(thickness, 0.1f, attacker.skill.range * -2);
        //localScale.y�� -�� ������ mesh�� �Ųٷ� ������ cube�� ��������� �ϱ� ����

        float angle = Vector3.SignedAngle(transform.position, (aim.mousepos - transform.position), Vector3.up) + 153f;
        //���� ������ �� �̷��� ���Դ����� �𸣰���..

        cube.transform.rotation = Quaternion.Euler(cube.transform.rotation.x, attacker.angle + angle, cube.transform.rotation.z);
        cube2.transform.rotation = Quaternion.Euler(cube.transform.rotation.x, -attacker.angle + angle, cube.transform.rotation.z);

    }

    public void SetIsDrawingTrue()
    {
        isDrawing = true;
    }

    public void SetIsDrawingFalse()
    {
        isDrawing = false;
    }

    public void SetCubeActiveFalse()
    {
        cube.SetActive(false);
        cube2.SetActive(false);
    }
}
