using UnityEngine;
using UnityEngine.InputSystem;

public class playercontroll : MonoBehaviour
{
    //�÷��̾� ���� ����
    private Rigidbody playerrb;
    private Vector3 movedir;
    [SerializeField] public float movespeed;

    private Animator anim;
    [SerializeField] PlayerAimTest playerat;


    [HideInInspector]public Vector2 playerdir;
    private void Awake()
    {
        playerrb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();


    }

    private void Update()
    {
        if (!playerat.isattack)
        {
            Move();
            Look();
        }
        else
        {
            //�÷��̾� �����Ҷ����� ���̴°� ����
            Vector3 aimpos = new Vector3(playerat.attackdir.x, transform.position.y, playerat.attackdir.z);
            transform.LookAt(aimpos);
            Move();
        }

        if (playerrb.velocity == new Vector3(0, 0, 0))
        {
            anim.SetBool("move", false);
        }
        else
        {
            anim.SetBool("move", true);
        }
        if (movedir.magnitude == 0)
        {
            return;
        }
    }
    private void Move()
    {

        playerrb.velocity = new Vector3(movedir.z * movespeed, 0, -movedir.x * movespeed);

    }
    private void Look()                     //������ ���� �� �ٶ󺸴� ���� ����
    {
        if (movedir.magnitude == 0)
            return;
        Vector3 viewVector = new Vector3(movedir.z, 0, -movedir.x);
        Quaternion lookrotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(viewVector), 0.1f);
        transform.rotation = lookrotation;
    }

    private void OnMove(InputValue Value)
    {
        movedir.x = Value.Get<Vector2>().x;
        movedir.z = Value.Get<Vector2>().y;
    }

}

