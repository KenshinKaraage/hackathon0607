using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ButtonSoundManager : MonoBehaviour
{
    [SerializeField] private Button button;
    private AudioManager manager;

    void Start()
    {
        manager = FindAnyObjectByType<AudioManager>();
        button = GetComponent<Button>();

        button.onClick.AddListener(() => manager.PlaySE("click"));
    }
}
