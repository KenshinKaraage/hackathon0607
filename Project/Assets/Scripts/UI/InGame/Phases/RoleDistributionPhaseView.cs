using UnityEngine;

public class RoleDistributionPhaseView : MonoBehaviour
{
    private UIPresenter_Body body;
    private UIPresenter_Footer footer;
    private UIPresenter_Header header;

    void Start()
    {
        header = FindAnyObjectByType<UIPresenter_Header>();
        body = FindAnyObjectByType<UIPresenter_Body>();
        footer = FindAnyObjectByType<UIPresenter_Footer>();
    }

    public void ShowWait()
    {
        UIPresenter_Body body = FindAnyObjectByType<UIPresenter_Body>();
        body.ShowWait("割り当て中");
        header.SetView("役職配布");
        header.HideInfo();
        footer.Hide();
    }

    public void ShowDistribution(IPlayerCharacter player)
    {
        CharacterDataList characterDataList = FindAnyObjectByType<CharacterDataList>();
        header.SetView("役職配布", player.Job, characterDataList.CharacterDatas[player.CharacterIndex].imageSprite, player.Displayname, characterDataList.CharacterDatas[player.CharacterIndex].characterName);
        body.ShowDistribution(player.Job, characterDataList.CharacterDatas[player.CharacterIndex].imageSprite, characterDataList.CharacterDatas[player.CharacterIndex].characterName, characterDataList.CharacterDatas[player.CharacterIndex].description);
    }
}
