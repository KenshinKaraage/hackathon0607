using UnityEngine;
using TMPro;

public class UIPresenter_Header : MonoBehaviour
{
    [SerializeField] private GameObject infoOb;

    [SerializeField] private TMP_Text phaseText;
    [SerializeField] private TMP_Text roleText;
    [SerializeField] private CharacterIcon characterIcon;

    public void HideInfo()
    {
        infoOb.SetActive(false);
    }

    public void SetView(string phase, Role role, Sprite sprite, string displayName, string character)
    {
        infoOb.SetActive(true);

        phaseText.text = phase;
        roleText.text = $"役職：{role}\n性格：{character}";
        characterIcon.Set(sprite, displayName);
    }

    public void SetView(string phase)
    {
        phaseText.text = phase;
    }
}
