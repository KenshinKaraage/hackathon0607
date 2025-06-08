using UnityEngine;
using TMPro;
using Photon.Realtime;

public class UIPresenter_Footer : MonoBehaviour
{
    [SerializeField] private GameObject InputOb;
    [SerializeField] private GameObject footerTextOb;
    [SerializeField] private GameObject playerListOb;
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
        playerListOb.SetActive(false);
        submitOb.SetActive(false);
        retryOb.SetActive(false);
    }

    public void ShowInput()
    {
        InputOb.SetActive(true);
        footerTextOb.SetActive(false);
        playerListOb.SetActive(false);
        submitOb.SetActive(false);
        retryOb.SetActive(false);
    }

    public void ShowFooterText(string text)
    {
        InputOb.SetActive(false);
        footerTextOb.SetActive(true);
        playerListOb.SetActive(false);
        submitOb.SetActive(false);
        retryOb.SetActive(false);

        footerText.text = text;
    }

    public void ShowPlayerList((string name, Sprite sprite, IPlayerCharacter player ,System.Action<IPlayerCharacter> action)[] values )
    {
        InputOb.SetActive(false);
        footerTextOb.SetActive(false);
        playerListOb.SetActive(true);
        submitOb.SetActive(false);
        retryOb.SetActive(false);

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < values.Length)
            {
                (string name, Sprite sprite, IPlayerCharacter player, System.Action<IPlayerCharacter> action) = values[i];
                buttons[i].gameObject.SetActive(true);
                buttons[i].SetView(name, sprite);
                buttons[i].AddEvent(player, action);
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowSubmit(string selectPlayerName)
    {
        InputOb.SetActive(false);
        footerTextOb.SetActive(false);
        playerListOb.SetActive(false);
        submitOb.SetActive(true);
        retryOb.SetActive(false);

        selectPlayerNameText.text = selectPlayerName;
    }

    public void ShowRetry()
    {
        InputOb.SetActive(false);
        footerTextOb.SetActive(false);
        playerListOb.SetActive(false);
        submitOb.SetActive(false);
        retryOb.SetActive(true);
    }

    public string GetInputFieldText()
    {
        return inputField.text;
    }

}
