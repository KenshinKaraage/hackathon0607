using UnityEngine;

public class LobbyUIPresentor : MonoBehaviour
{
    [SerializeField] private NameInputManager nameInput;
    [SerializeField] private RoomTableManager roomTable;
    [SerializeField] private PasswordWindow joinPasswordWindow;

    private void Awake()
    {
        ShowNameInput();
    }

    public void ShowNameInput()
    {
        nameInput.gameObject.SetActive(true);
        roomTable.gameObject.SetActive(false);
        joinPasswordWindow.gameObject.SetActive(false);
    }

    public void ShowRoomTable()
    {
        nameInput.gameObject.SetActive(false);
        roomTable.gameObject.SetActive(true);
        roomTable.GeenrateRoomTable();
        joinPasswordWindow.gameObject.SetActive(false);
    }

    public void ShowJoinPasswordWindow()
    {
        nameInput.gameObject.SetActive(false);
        roomTable.gameObject.SetActive(false);
        joinPasswordWindow.gameObject.SetActive(true);
    }
}
