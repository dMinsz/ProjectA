using UnityEngine;

public class playerrange : MonoBehaviour
{
    [SerializeField] PlayerAimTest aim;

    private float size;
    private void Awake()
    {
        size = aim.attacksize *1f;
        transform.localScale = new Vector3(size, size, size);
    }
}
