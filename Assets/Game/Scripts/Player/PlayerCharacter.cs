using Photon.Pun;
using UnityEngine;


public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] GameObject charactersObj;
    Animator anim;
    [SerializeField] RuntimeAnimatorController animatorController;

    public void Awake()
    {

        anim = GetComponent<Animator>();

        animatorController = GetComponent<Animator>().runtimeAnimatorController;


        if (PhotonNetwork.LocalPlayer.GetCharacterName() == "None")
        { // for debug mode
            ChangeCharacter("Mario");
        }
        else
        {
            ChangeCharacter(PhotonNetwork.LocalPlayer.GetCharacterName());
        }

    }

    public void ChangeCharacter(string name) 
    {
        for (int i = 0; i < charactersObj.transform.childCount; i++)
        {
            if (charactersObj.transform.GetChild(i).name == name)
                charactersObj.transform.GetChild(i).gameObject.SetActive(true);
            else
                charactersObj.transform.GetChild(i).gameObject.SetActive(false);
        }


        anim.avatar = GameManager.Data.GetCharacter(name).avatar;
        anim.runtimeAnimatorController = GameManager.Data.GetCharacter(name).runtimeAnimator;
    }

}
