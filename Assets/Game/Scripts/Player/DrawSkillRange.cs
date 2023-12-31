using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    PlayerAim aim;
    bool isDrawing;
    Vector3 oldRotation;
    [SerializeField] List<Vector3> arcPoints;
    float time;

    //[HideInInspector]public Transform RotatedPos; 

    public void Awake()
    {
        attacker = gameObject.GetComponent<PlayerSkillAttacker>();
        aim = gameObject.GetComponent<PlayerAim>();

        sphereInCube = cube.transform.GetChild(0).gameObject;
        sphereInCube2 = cube2.transform.GetChild(0).gameObject;

        sphereInCube.SetActive(false);
        sphereInCube2.SetActive(false);

        line.SetActive(false);
        cube.SetActive(false);
        cube2.SetActive(false);


        Vector3 newPos = new Vector3(transform.position.x, 1f, transform.position.z);

        line.transform.position = newPos;
        cube.transform.position = newPos;
        cube2.transform.position = newPos;

        cube.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", color);
        cube2.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", color);

    }

    public void Update()
    {

        if (isDrawing)
        {
            Draw();
        }
        else
            SetActiveFalse();
    }

    //public void OnEnable()
    //{
    //    attacker.OnSkillStart += SetIsDrawingTrue;
    //}

    //public void OnDisable()
    //{
    //    attacker.OnSkillStart -= SetIsDrawingTrue;
    //}

    public void Draw()
    {
        DrawWithTwoCubes();
        if (attacker.skill.rangeStyle == Skill.RangeStyle.Arc)
        {
            DrawWithLineRenderer();
            MakeArcWithLine();
        }
        else if (attacker.skill.rangeStyle == Skill.RangeStyle.Circle) 
        {
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

            //cube.transform.rotation = RotatedPos.transform.rotation;
            //cube2.transform.rotation = RotatedPos.transform.rotation;


            Vector3 newPos = new Vector3(transform.position.x, 1.5f, transform.position.z);
            Vector3 dir = (aim.mousepos - newPos).normalized;

            cube.transform.forward = new Vector3(dir.x, 0, dir.z);
            cube2.transform.forward = new Vector3(dir.x, 0, dir.z);

        }

        if (attacker.skill.rangeStyle == Skill.RangeStyle.Arc)
        {
            cube.transform.localScale = new Vector3(thickness, 0.1f, -attacker.skill.range * 0.8f);
            cube2.transform.localScale = new Vector3(thickness, 0.1f, -attacker.skill.range * 0.8f);
            //localScale.z가 -인 이유는 mesh를 거꾸로 입혀서 cube를 뒤집어줘야 하기 때문


            Vector3 newPos = new Vector3(transform.position.x, 1f, transform.position.z);

            Vector3 dir = (aim.mousepos - newPos).normalized;
            Vector3 leftDir = Quaternion.Euler(0f, attacker.skill.angle, 0f) * dir;
            Vector3 rightDir = Quaternion.Euler(0f, -attacker.skill.angle, 0f) * dir;

            //rotation 말고 forward로 해도 따라가서 사용
            cube.transform.forward = new Vector3(leftDir.x, 0, leftDir.z);
            cube2.transform.forward = new Vector3(rightDir.x, 0, rightDir.z);
        }

    }

    public void MakeArcWithLine()
    {
        line.SetActive(true);
        line.GetComponent<LineRenderer>().startWidth = thickness;
        line.GetComponent<LineRenderer>().endWidth = thickness;
        line.GetComponent<LineRenderer>().materials[0].SetColor("_EmissionColor", color);
        line.GetComponent<LineRenderer>().positionCount = segments;

        Vector3 dir = (aim.mousepos - transform.position).normalized;
        arcPoints = new List<Vector3>();
        float startAngle = -attacker.skill.angle;
        float arcLength = attacker.skill.angle - -attacker.skill.angle;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * startAngle) * attacker.skill.range;
            float z = Mathf.Cos(Mathf.Deg2Rad * startAngle) * attacker.skill.range;
            arcPoints.Add(new Vector3(x, 0.2f, z));

            startAngle += (arcLength / segments);
        }

        for (int i = 0; i < segments; i++)
        {
            line.GetComponent<LineRenderer>().SetPosition(i, arcPoints[i]);
        }
        //  line.transform.position = transform.position;  원본
        // line.transform.LookAt(aim.mousepos);       원본


        Vector3 linerotation = new Vector3(aim.mousepos.x, transform.position.y, aim.mousepos.z);
        line.transform.position = transform.position;
        line.transform.LookAt(linerotation);
    }

    Coroutine DrawLineCoroutine;

    public void SetIsDrawingTrue()
    {
        isDrawing = true;
        //StopAllCoroutines();
        //DrawLineCoroutine = StartCoroutine(DrawLineRoutine());
    }

    IEnumerator DrawLineRoutine()
    {
        time = 1;
        while (attacker.skill.duration >= time)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log($"{attacker.skill.skillName}이 {time}초동안 실행되는 중");
            time += 1;
        }

        //yield return new WaitForSeconds(attacker.skill.duration);
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

    public bool GetIsDraw()
    {
        return isDrawing;
    }
}