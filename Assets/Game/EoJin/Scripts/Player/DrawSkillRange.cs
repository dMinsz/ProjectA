using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;
using static UnityEngine.LightAnchor;
using static UnityEngine.Rendering.HableCurve;

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
    [SerializeField] int segments;
    PlayerSkillAttacker attacker;
    anstjddn.PlayerAim aim;
    bool isDrawing;
    Vector3 oldRotation;
    [SerializeField] List<Vector3> arcPoints;

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
        {
            DrawWithLineRenderer();
            MakeArcWithLine();
        }
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

        

        line.transform.position = Vector3.zero;
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
            cube.transform.localScale = new Vector3(attacker.skill.additionalRange * 0.5f, 0.1f, -attacker.skill.range * 0.8f);
            cube2.transform.localScale = new Vector3(-attacker.skill.additionalRange * 0.5f, 0.1f, -attacker.skill.range * 0.8f);

            cube.transform.LookAt(aim.mousepos);
            cube2.transform.LookAt(aim.mousepos);
        }

        if (attacker.skill.rangeStyle == Skill.RangeStyle.Arc)
        {
            cube.transform.localScale = new Vector3(thickness, 0.1f, -attacker.skill.range * 0.8f);
            cube2.transform.localScale = new Vector3(thickness, 0.1f, -attacker.skill.range * 0.8f);
            //localScale.z가 -인 이유는 mesh를 거꾸로 입혀서 cube를 뒤집어줘야 하기 때문

            Vector3 dir = (aim.mousepos - transform.position).normalized;
            Vector3 leftDir = Quaternion.Euler(0f, attacker.skill.angle, 0f) * dir;
            Vector3 rightDir = Quaternion.Euler(0f, -attacker.skill.angle, 0f) * dir;

            cube.transform.rotation = Quaternion.LookRotation(leftDir);
            cube2.transform.rotation = Quaternion.LookRotation(rightDir);
        }

    }

    public void MakeArcWithLine()
    {
        line.GetComponent<LineRenderer>().positionCount = segments;

        Vector3 dir = (aim.mousepos - transform.position).normalized;
        arcPoints = new List<Vector3>();
        float startAngle = -attacker.skill.angle;
        float arcLength = attacker.skill.angle - -attacker.skill.angle;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * startAngle) * attacker.skill.range;
            float z = Mathf.Cos(Mathf.Deg2Rad * startAngle) * attacker.skill.range;
            arcPoints.Add(new Vector3(x, 0f, z));

            startAngle += (arcLength / segments);
        }

        for (int i = 0; i < segments; i++)
        {
            line.GetComponent<LineRenderer>().SetPosition(i, arcPoints[i]);
        }

        line.transform.LookAt(aim.mousepos);
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