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
    PlayerAimTest aim;
    bool isDrawing;
    Vector3 oldRotation;
    [SerializeField] List<Vector3> arcPoints;
    float time;
    [SerializeField] public GameObject cubeForLookAt;
    [SerializeField] public GameObject mousePosObj;


    public void Awake()
    {
        attacker = gameObject.GetComponent<PlayerSkillAttacker>();
        aim = gameObject.GetComponent<PlayerAimTest>();

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
        cubeForLookAt.transform.LookAt(mousePosObj.transform);

        if (isDrawing)
        {
            Draw();
        }
        else
            SetActiveFalse();
    }

    public void OnEnable()
    {
        attacker.OnSkillStart += SetIsDrawingTrue;
    }

    public void OnDisable()
    {
        attacker.OnSkillStart -= SetIsDrawingTrue;
    }

    public void Draw()
    {
        DrawWithTwoCubes();
        if (attacker.skill.rangeStyle == Skill.RangeStyle.Arc)
        {
            DrawWithLineRenderer();
            MakeArcWithLine();
        }
        else
            line.SetActive(false);
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

            cube.transform.rotation = cubeForLookAt.transform.rotation;
            cube2.transform.rotation = cubeForLookAt.transform.rotation;
        }

        if (attacker.skill.rangeStyle == Skill.RangeStyle.Arc)
        {
            cube.transform.localScale = new Vector3(thickness, 0.1f, -attacker.skill.range * 0.8f);
            cube2.transform.localScale = new Vector3(thickness, 0.1f, -attacker.skill.range * 0.8f);
            //localScale.z가 -인 이유는 mesh를 거꾸로 입혀서 cube를 뒤집어줘야 하기 때문

            cube.transform.rotation = cubeForLookAt.transform.rotation;
            cube2.transform.rotation = cubeForLookAt.transform.rotation;
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

        line.transform.position = transform.position;
        line.transform.LookAt(aim.mousepos);
    }

    Coroutine DrawLineCoroutine;

    public void SetIsDrawingTrue()
    {
        isDrawing = true;
        StopAllCoroutines();
        DrawLineCoroutine = StartCoroutine(DrawLineRoutine());
    }

    IEnumerator DrawLineRoutine()
    {
        time = 1;
        /*
        while (attacker.skill.duration >= time)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log($"{attacker.skill.skillName}이 {time}초동안 실행되는 중");
            time += 1;
        }
        */

        yield return new WaitForSeconds(attacker.skill.duration);
        //while문 지우고 위 문장 주석 해지할 것
        SetIsDrawingFalse();
    }

    public void SetIsDrawingFalse()
    {
        isDrawing = false;
        SetActiveFalse();
    }

    public void SetActiveFalse()
    {
        line.SetActive(false);
        cube.SetActive(false);
        cube2.SetActive(false);
    }

}