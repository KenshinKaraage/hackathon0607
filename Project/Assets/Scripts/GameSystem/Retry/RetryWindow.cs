using UnityEngine;
using Photon.Pun;

public class RetryWindow : MonoBehaviour
{
    public void OnClickRetry()
    {
        //リトライ！！
        GameFlowController controller = FindAnyObjectByType<GameFlowController>();
        controller.Initialize();
    }

    public void OnClickBack()
    {
        //部屋に戻る
        PhotonNetwork.LoadLevel("RoomScene2");
    }
}
