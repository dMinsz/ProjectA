using Unity.Burst.Intrinsics;
using UnityEngine;

public class Playeraimdir : MonoBehaviour
{
    //[SerializeField] private GameObject playerPos;
    [SerializeField] public PlayerAim aim;   //쓰고있으신 playeraim 으로 바꾸시면 됩니다 

    private void Update()
    {

        Vector3 newPos = new Vector3(transform.position.x, 1f, transform.position.z);
        Vector3 dir = (aim.mousepos - newPos).normalized;

        transform.forward = new Vector3(dir.x, 0, dir.z);
    }
}
