using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NicknameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField nicknameInput;


    private void Start()
    {
        // 入力が終了したときにニックネームを設定する
        nicknameInput.onEndEdit.AddListener(SetNickname);
    }

    public string getNickname()
    {
        return nicknameInput.text.Trim();
    }

    private void SetNickname(string input)
    {
        string inputName = nicknameInput.text.Trim();

        if (!string.IsNullOrEmpty(inputName))
        {
            PhotonNetwork.NickName = inputName;
            Debug.Log("ニックネーム設定完了: " + PhotonNetwork.NickName);
            // 次の画面に進むならここで Scene 遷移してもよい
        }
        else
        {
            Debug.LogWarning("ニックネームが空です！");
        }
    }
}
