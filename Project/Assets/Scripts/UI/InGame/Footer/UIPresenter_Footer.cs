using UnityEngine;
using TMPro;
using Photon.Realtime;
using UnityEngine.InputSystem.HID;

public class UIPresenter_Footer : MonoBehaviour
{
    [SerializeField] private GameObject InputOb;
    [SerializeField] private GameObject footerTextOb;
    [SerializeField] private GameObject submitOb;
    [SerializeField] private GameObject retryOb;

    [SerializeField] private Transform executePlayerElementsParent;
    private ExecutePlayerButtons[] buttons;

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text footerText;
    [SerializeField] private TMP_Text selectPlayerNameText;

    private void Awake()
    {
        buttons = executePlayerElementsParent.GetComponentsInChildren<ExecutePlayerButtons>();
    }

    public void Hide()
    {
        InputOb.SetActive(false);
        footerTextOb.SetActive(false);
        submitOb.SetActive(false);
        retryOb.SetActive(false);
    }

    public void ShowInput()
    {
        Hide();
        InputOb.SetActive(true);
    }

    public void ShowFooterText(string text)
    {
        Hide();
        footerTextOb.SetActive(true);

        footerText.text = text;
    }

    public void ShowSubmit(string selectPlayerName)
    {
        Hide();
        submitOb.SetActive(true);

        selectPlayerNameText.text = selectPlayerName;
    }

    public void ShowRetry()
    {
        Hide();
        retryOb.SetActive(true);
    }

    public string GetInputFieldText()
    {
        return inputField.text;
    }

}
