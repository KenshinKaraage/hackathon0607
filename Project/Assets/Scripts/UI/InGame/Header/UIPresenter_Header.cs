using UnityEngine;
using TMPro;

// このスクリプトは Role enum がどこかで定義されていることを前提としています。
// 例: public enum Role { WEREWOLF, REPRESENTATIVE, VILLAGER }
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

    /// <summary>
    /// ヘッダーの表示を設定します。役職に応じて表示テキストを変更します。
    /// </summary>
    public void SetView(string phase, Role role, Sprite sprite, string displayName, string character)
    {
        infoOb.SetActive(true);

        phaseText.text = phase;
        characterIcon.Set(sprite, displayName);

        // --- ここからが修正部分 ---

        // roleTextに表示する役職名を一時的に保持する変数
        string roleDiplayName = "";

        // role の値によって表示する役職名を切り替える
        if (role == Role.Werewolf)
        {
            roleDiplayName = "解答者";
        }
        else if (role == Role.Representative)
        {
            roleDiplayName = "質問者";
        }
        else
        {
            // 上記以外の役職の場合のデフォルト表示（元のコードのまま、または別の表示）
            // ここでは enum の名前をそのまま使う例
            roleDiplayName = role.ToString();
        }

        // 最終的なテキストを設定
        roleText.text = $"役職：{roleDiplayName}\n性格：{character}";

        // --- ここまでが修正部分 ---
    }

    public void SetView(string phase)
    {
        phaseText.text = phase;
    }
}

// ---- 注意 ----
// このコードが正しく動作するには、プロジェクト内のどこかで
// Role という名前の enum（列挙型）が以下のように定義されている必要があります。
// 例:
/*
public enum Role
{
    VILLAGER,
    WEREWOLF,
    REPRESENTATIVE
    // 他の役職...
}
*/
