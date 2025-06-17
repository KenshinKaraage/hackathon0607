using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


[RequireComponent(typeof(Button))]
public class ButtonSoundManager : MonoBehaviour
{
    private Button button;
    private AudioManager manager;

    void Start()
    {
        manager = FindAnyObjectByType<AudioManager>();
        button = GetComponent<Button>();

        button.onClick.AddListener(() => manager.PlaySE("click"));
    }
}
