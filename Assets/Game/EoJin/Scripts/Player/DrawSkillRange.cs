using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;
using static UnityEngine.LightAnchor;

public class DrawSkillRange : MonoBehaviour
{
    [SerializeField] GameObject line;
    [SerializeField] GameObject cube;
    [SerializeField] GameObject cube2;
    GameObject sphereInCube;
    GameObject sphereInCube2;
    [SerializeField] int linePoints;
    [SerializeField] float thickness;
    [SerializeField] Color color;
    PlayerSkillAttacker attacker;
    anstjddn.PlayerAim aim;
    bool isDrawing;
    Vector3 oldRotation;

    public void Awake()
    {
        attacker = gameObject.GetComponent<PlayerSkillAttacker>();
        aim = gameObject.GetComponent<anstjddn.PlayerAim>();

        sphereInCube = cube.transform.GetChild(0).gameObject;
        sphereInCube2 = cube2.transform.GetChild(0).gameObject;

        sphereInCube.SetActive(false);
        sphereInCube2.SetActive(false);

        line.SetActive(false);
        cube.SetActive(false);
        cube2.SetActive(false);

        line.transform.position = transform.position;
        cube.transform.position = transform.position;
        cube2.transform.position = transform.position;

        cube.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", color);
        cube2.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", color);
    }

    public void Update()
    {
        if (isDrawing)
            Draw();
        else
            SetActiveFalse();
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
        DrawWithTwoCubes();
        if (attacker.skill.rangeStyle == Skill.RangeStyle.Arc)
            DrawWithLineRenderer();
    }

    public void DrawWithLineRenderer()
    {
        if (attacker.angle == 180)
        {
            line.gameObject.SetActive(false);
            return;
        }

        line.gameObject.SetActive(true);
        line.GetComponent<LineRenderer>().startWidth = thickness;
        line.GetComponent<LineRenderer>().endWidth = thickness;
        line.GetComponent<LineRenderer>().materials[0].SetColor("_EmissionColor", color);

        Vector3 position1 = new Vector3(transform.position.x - sphereInCube2.transform.position.x, 0f, transform.position.z - sphereInCube2.transform.position.z);
        Vector3 position2 = new Vector3(transform.position.x - sphereInCube.transform.position.x, 0f, transform.position.z - sphereInCube.transform.position.z);

        line.GetComponent<LineRenderer>().SetPosition(0, -position1);
        line.GetComponent<LineRenderer>().SetPosition(1, (-position1 + -position2) / 2);
        line.GetComponent<LineRenderer>().SetPosition(2, -position2);

    }

    public void DrawWithTwoCubes()
    {
        if (attacker.skill.rangeStyle == Skill.RangeStyle.Circle)
        {
            cube.SetActive(false);
            cube2.SetActive(false);
            return;
        }

        cube.SetActive(true);
        cube2.SetActive(true);

        if (attacker.skill.rangeStyle == Skill.RangeStyle.Square)
        {
            cube.transform.localScale = new Vector3(attacker.skill.additionalRange * 0.5f, 0.1f, attacker.skill.range * 0.8f);
            cube2.transform.localScale = new Vector3(-attacker.skill.additionalRange * 0.5f, 0.1f, attacker.skill.range * 0.8f);

            cube.transform.LookAt(aim.mousepos);
            cube2.transform.LookAt(aim.mousepos);
        }

        if (attacker.skill.rangeStyle == Skill.RangeStyle.Arc)
        {
            cube.transform.localScale = new Vector3(thickness, 0.1f, -attacker.skill.range * 0.8f);
            cube2.transform.localScale = new Vector3(thickness, 0.1f, -attacker.skill.range * 0.8f);
            //localScale.z가 -인 이유는 mesh를 거꾸로 입혀서 cube를 뒤집어줘야 하기 때문

            cube.transform.LookAt(aim.mousepos);
            cube2.transform.LookAt(aim.mousepos);

            
        }

    }

    public void SetIsDrawingTrue()
    {
        isDrawing = true;
    }

    public void SetIsDrawingFalse()
    {
        isDrawing = false;
    }

    public void SetActiveFalse()
    {
        line.SetActive(false);
        cube.SetActive(false);
        cube2.SetActive(false);
    }
}