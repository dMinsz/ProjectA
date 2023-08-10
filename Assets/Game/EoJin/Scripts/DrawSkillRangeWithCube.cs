using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSkillRangeWithCube : MonoBehaviour
{
    [SerializeField] GameObject cube;
    [SerializeField] GameObject cube2;
    PlayerSkillAttacker attacker;
    PlayerAimTEST aim;
    bool isDrawing;

    public void Awake()
    {
        attacker = gameObject.GetComponent<PlayerSkillAttacker>();
        aim = gameObject.GetComponent<PlayerAimTEST>();
        cube.SetActive(false);
        cube2.SetActive(false);
    }

    public void Update()
    {
        if (isDrawing)
            DrawSkillRange();
        else
            SetCubeActiveFalse();
    }

    public void OnEnable()
    {
        attacker.OnSkillStart += SetIsDrawingTrue;
    }

    public void OnDisable()
    {
        attacker.OnSkillStart -= SetIsDrawingTrue;
    }

    public void DrawSkillRange()
    {
        cube.SetActive(true);
        cube2.SetActive(true);

        if (attacker.angle == 360)
        {
            cube.SetActive(false);
            cube2.SetActive(false);
            return;
        }

        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + attacker.angle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - attacker.angle * 0.5f);

        cube.transform.localScale = new Vector3(cube.transform.localScale.x, cube.transform.localScale.y, attacker.skill.range * 2);
        cube2.transform.localScale = new Vector3(cube.transform.localScale.x, cube.transform.localScale.y, attacker.skill.range * 2);

        Vector3 playerNMouse = (aim.mousepos - transform.position).normalized;

        cube.transform.rotation = Quaternion.Euler(cube.transform.rotation.x, attacker.angle, cube.transform.rotation.z);
        cube2.transform.rotation = Quaternion.Euler(cube.transform.rotation.x, -attacker.angle, cube.transform.rotation.z);
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
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
