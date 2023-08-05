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
        //스킬마다 색을 다르게 구현하는 게 아닌 플레이어마다 고유의 스킬 색 하나가 있는 경우는 SetColor을 Awake로 옮길 것

        cube.transform.localScale = new Vector3(thickness, 0.1f, attacker.skill.range * -2);
        cube2.transform.localScale = new Vector3(thickness, 0.1f, attacker.skill.range * -2);
        //localScale.y가 -인 이유는 mesh를 거꾸로 입혀서 cube를 뒤집어줘야 하기 때문

        float angle = Vector3.SignedAngle(transform.position, (aim.mousepos - transform.position), Vector3.up) + 153f;
        //각도 계산식이 왜 이렇게 나왔는지는 모르겠음..

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
