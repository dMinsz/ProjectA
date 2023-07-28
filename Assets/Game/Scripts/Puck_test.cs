using System;
using UnityEngine;

public class Puck_test : MonoBehaviour
{

    [SerializeField] bool isDebug;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float DebugrayDistance;
    [SerializeField] float bouncingForce= 2f;
    [SerializeField] float radiusCast = 2f;

    private Rigidbody rb;
    private RaycastHit hit;


    private Vector3 direction;
    private Vector3 oldPosition;
    private Vector3 currentPosition;
    private float velocity;

    private Vector3 testRvector = Vector3.zero;

    void Start()
    {
        oldPosition = transform.position;
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //Time.timeScale = 0.5f;
    }

    private void Update()
    {
    
        WallCheck();

    }

    private void FixedUpdate()
    {
        CheckPos();
        //WallCheck();
    }

    private void LateUpdate()
    {
        
    }

    private void CheckPos() 
    {
        currentPosition = transform.position;
        var dis = (currentPosition - oldPosition);
        direction = dis.normalized;
        var distance = Math.Sqrt(Math.Pow(dis.x, 2) + Math.Pow(dis.y, 2) + Math.Pow(dis.z, 2));
        velocity = (float)distance / Time.deltaTime;
        oldPosition = currentPosition;
    }

    private void WallCheck()
    {

        var afterMove = transform.position + (direction * velocity * Time.deltaTime);

        if (Physics.Raycast(afterMove, direction, out RaycastHit hitInfo, 0.5f, layerMask))
        {
            Debug.Log("RayCast Hit");
            //충돌한 물체의 법선 벡터
            var normalVector = hitInfo.normal;

            Vector3 reflectVec = Vector3.Reflect(transform.position.normalized, normalVector);


            direction = reflectVec;
            testRvector = reflectVec;

            rb.velocity = direction * bouncingForce;
            //rb.AddForce(reflectVec * bouncingForce, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        if (isDebug) 
        {
            Gizmos.DrawRay(transform.position, direction * DebugrayDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, testRvector * DebugrayDistance );
            Gizmos.color = Color.white;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radiusCast);
            Gizmos.color = Color.white;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            var direct = (transform.position - collision.gameObject.transform.position).normalized;

            rb.AddForce(direct * bouncingForce, ForceMode.Impulse);
        }
    }

}
