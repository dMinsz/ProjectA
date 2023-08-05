using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.LightAnchor;

public class DrawSkillRange : MonoBehaviour
{
    [SerializeField] GameObject line;
    [SerializeField] GameObject cube;
    [SerializeField] GameObject cube2;
    [SerializeField] GameObject sphereInCube;
    [SerializeField] GameObject sphereInCube2;
    [SerializeField] int linePoints;
    [SerializeField] float thickness;
    [SerializeField] Color color;
    PlayerSkillAttacker attacker;
    PlayerAim aim;
    bool isDrawing;

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
        line.GetComponent<LineRenderer>().materials[0].SetColor("_EmissionColor", color);

        Vector3 position1 = new Vector3(transform.position.x - sphereInCube2.transform.position.x, 0f, transform.position.z - sphereInCube2.transform.position.z);
        Vector3 position2 = new Vector3(transform.position.x - sphereInCube.transform.position.x, 0f, transform.position.z - sphereInCube.transform.position.z);

        //MakeArc(position2, position1);

        line.GetComponent<LineRenderer>().SetPosition(0, -position1);
        line.GetComponent<LineRenderer>().SetPosition(1, (-position1 + -position2) / 2);
        line.GetComponent<LineRenderer>().SetPosition(2, -position2);

    }

    public void MakeLineToArc(Vector3 startLoc, Vector3 endLoc)
    {
        float angle = attacker.skill.angle;
        Vector3 outDirection = (transform.position - aim.mousepos).normalized;

        float radAngle = angle * (Mathf.PI / 360);

        List<Vector3> points = new List<Vector3>();
        Vector3 initialDirection = (endLoc - startLoc).normalized;
        Vector3 maxOutDirection = Vector3.RotateTowards(initialDirection, outDirection, radAngle, 0);

        Vector3 maxInDirection = Vector3.RotateTowards(initialDirection, outDirection * -1, radAngle, 0);

        Vector3 currentDirection = maxOutDirection;
        points.Add(startLoc);

        float tChange = linePoints - 1;
        tChange /= (linePoints - 1) * (linePoints - 1);

        for (int index = 1; index <= linePoints; index++)
        {
            points.Add(points[index - 1] + (currentDirection / linePoints));
            currentDirection = Vector3.Lerp(maxOutDirection, maxInDirection, (index * tChange));
        }

        line.GetComponent<LineRenderer>().positionCount = points.Count;
        line.GetComponent<LineRenderer>().SetPositions(points.ToArray());
    }

    public void DrawWithTwoCubes()
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

        cube.transform.localScale = new Vector3(thickness, 0.1f, attacker.skill.range * -1.35f);
        cube2.transform.localScale = new Vector3(thickness, 0.1f, attacker.skill.range * -1.35f);
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

    public void SetActiveFalse()
    {
        line.SetActive(false);
        cube.SetActive(false);
        cube2.SetActive(false);
    }
}
