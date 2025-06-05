// Assets/Scripts/UI/InGame/VotePresenter.cs (新規作成)

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TMPro; // TextMeshProを使用している場合はこれも必要

public class VoteUIPresenter : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject mainVotePanel; // 投票UI全体の親オブジェクト
    [SerializeField] private GameObject waitingPanel; // 待機中パネル
    [SerializeField] private GameObject confirmationPanel; // 投票完了後パネル

    [Header("UI Components")]
    [SerializeField] private Transform voteButtonContainer; // ボタンを生成する場所
    [SerializeField] private GameObject voteButtonPrefab; // 投票ボタンのプレハブ
    [SerializeField] private TextMeshProUGUI instructionText; // 指示テキスト (TextMeshProの場合)
    [SerializeField] private TextMeshProUGUI waitingText; // 待機中テキスト
    [SerializeField] private TextMeshProUGUI confirmationText; // 投票完了後テキスト

    private Action<IPlayerCharacter> onTargetSelectedCallback;

    /// <summary>
    /// このPresenterが管理する全てのUIを非表示にします。
    /// </summary>
    public void HideAllVotePanels()
    {
        if (mainVotePanel != null) mainVotePanel.SetActive(false);
        if (waitingPanel != null) waitingPanel.SetActive(false);
        if (confirmationPanel != null) confirmationPanel.SetActive(false);
    }

    /// <summary>
    /// 投票UIを表示し、キャラクターのリストからボタンを生成します。
    /// </summary>
    /// <param name="targetList">表示するキャラクターのリスト</param>
    /// <param name="onSelected">ボタンが押されたときに呼び出す処理（コールバック）</param>
    public void ShowVoteUI(List<IPlayerCharacter> targetList, Action<IPlayerCharacter> onSelected)
    {
        // UI要素が設定されているかチェック
        if (mainVotePanel == null || voteButtonContainer == null || voteButtonPrefab == null)
        {
            Debug.LogError("Vote UI elements are not assigned in VotePresenter's inspector.");
            //return;
        }

        HideAllVotePanels(); // 他のパネルを隠す
        mainVotePanel.SetActive(true);

        this.onTargetSelectedCallback = onSelected;

        // 指示テキストを更新
        if (instructionText != null)
        {
            instructionText.text = "追放する容疑者を選んでください";
        }

        // 以前のボタンをクリア
        foreach (Transform child in voteButtonContainer)
        {
            Destroy(child.gameObject);
        }

        if (targetList == null || targetList.Count == 0)
        {
            if (instructionText != null) instructionText.text = "投票可能な対象がいません";
            return;
        }

        // リスト内の各キャラクターに対応するボタンを生成
        foreach (IPlayerCharacter target in targetList)
        {
            GameObject buttonGO = Instantiate(voteButtonPrefab, voteButtonContainer);

            TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = target.Displayname;
            }

            Button button = buttonGO.GetComponent<Button>();
            if (button != null)
            {
                // onSelectedがnullでなければ、クリックイベントを設定
                if (onSelected != null)
                {
                    button.onClick.AddListener(() => {
                        this.onTargetSelectedCallback?.Invoke(target);
                    });
                }
                else
                {
                    // 表示するだけの場合はボタンを操作不可にする
                    button.interactable = false;
                }
            }
        }
    }

    /// <summary>
    /// 「代表者の投票を待っています...」などの待機UIを表示します。
    /// </summary>
    public void ShowWaitingUI()
    {
        HideAllVotePanels();
        if (waitingPanel != null)
        {
            waitingPanel.SetActive(true);
            if (waitingText != null)
            {
                waitingText.text = "代表者の投票を待っています...";
            }
        }
    }

    /// <summary>
    /// 「〇〇に投票しました」という確認UIを表示します。
    /// </summary>
    /// <param name="votedTarget">投票した対象</param>
    public void ShowConfirmationUI(IPlayerCharacter votedTarget)
    {
        HideAllVotePanels();
        if (confirmationPanel != null && votedTarget != null)
        {
            confirmationPanel.SetActive(true);
            if (confirmationText != null)
            {
                confirmationText.text = $"{votedTarget.Displayname} に投票しました。";
            }
        }
    }
}
