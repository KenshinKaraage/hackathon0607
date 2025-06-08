using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ButtonSoundManager : MonoBehaviour
{
    [SerializeField] private Button button;
    public AudioManager manager;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            manager = FindAnyObjectByType<AudioManager>();

            button.onClick.AddListener(() => manager.PlaySE("click"));

        }
    }
}
