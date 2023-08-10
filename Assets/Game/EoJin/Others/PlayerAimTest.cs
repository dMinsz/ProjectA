using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerAimTEST : MonoBehaviour
{

    [SerializeField] GameObject aimobj;   //����(���߿� ����)

    private Vector3 aimpos;


    //���ù���,�浹�� ���̾�, ���� �̴���
    [SerializeField] public float attacksize;
    [SerializeField] LayerMask ball;
    [SerializeField] public float attackpower;
    [SerializeField] public float attacktime;
    [SerializeField] public bool isattack;

    private void OnPoint(InputValue Value)
    {
        aimpos.x = Value.Get<Vector2>().x;
        aimpos.z = Value.Get<Vector2>().y;
        aimpos = Camera.main.ScreenToWorldPoint(new Vector3(aimpos.x, 0, aimpos.z));
    }

    public Vector3 mousepos;

    private void Start()
    {
        isattack = false;
    }

    private void Update()
    {
        aimobj.transform.position = new Vector3(aimpos.x, 0, aimpos.z);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            mousepos = hit.point;
            mousepos.y = 0;
        }
        Debug.Log(mousepos);
        //���콺 ������ ����� �ν��ϴ��� ���� ���߿� ����
        aimobj.transform.position = mousepos;
    }
    private void OnAttack(InputValue Value)
    {

        Attack();
    }

    private void Attack()
    {
        StartCoroutine(AttackTimeing(attacktime));
    }


    // ���� ���� ����
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attacksize);
    }

    //����Ÿ�̹� ����
    IEnumerator AttackTimeing(float attacktime)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attacksize, ball);
        foreach (Collider collider in colliders)
        {
            if (isattack == false && collider.gameObject.layer == 7)                //���̾� 7���� ball�� ����
            {

                isattack = true;
                Vector3 dir = (mousepos - transform.position).normalized;

                collider.GetComponent<Rigidbody>().velocity = dir * attackpower;
                //Attacksound?.Invoke();
                yield return new WaitForSeconds(attacktime);
                isattack = false;
            }
            //�÷��̾� ���� ���� �ʿ�

        }
    }
}
