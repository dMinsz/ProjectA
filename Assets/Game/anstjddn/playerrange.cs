using anstjddn;
using UnityEngine;

public class playerrange : MonoBehaviour
{
    [SerializeField] PlayerAim player;

    private float size;
    private void Awake()
    {
        size = player.attacksize * 1f;
        transform.localScale = new Vector3(size, size, size);
    }
}
