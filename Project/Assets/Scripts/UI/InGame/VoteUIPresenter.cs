// Assets/Scripts/UI/InGame/VotePresenter.cs (新規作成)

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TMPro; // TextMeshProを使用している場合はこれも必要

public class VoteUIPresenter : MonoBehaviour
{
    [SerializeField] private GameObject voterOb;
    [SerializeField] private GameObject nonvoterOb;

    [Header("Answers")]
    [SerializeField] private Transform elementsParent; // 投票UI全体の親オブジェクト
    private AnswerElement[] answerElements;

    [Header("UI Panels")]
    [SerializeField] private GameObject mainVotePanel; // 投票UI全体の親オブジェクト

    [Header("UI Components")]
    [SerializeField] private Transform voteButtonContainer; // ボタンを生成する場所
    [SerializeField] private GameObject voteButtonPrefab; // 投票ボタンのプレハブ
    [SerializeField] private TextMeshProUGUI instructionText; // 指示テキスト (TextMeshProの場合)

    // 他のパネルの参照（今後のステップで使用）
    // [SerializeField] private GameObject waitingPanel;
    // [SerializeField] private GameObject confirmationPanel;

    private Action<IPlayerCharacter> onTargetSelectedCallback;

    void Awake()
    {
        // 開始時は全てのパネルを非表示にしておく
        HideAllVotePanels();

        answerElements = elementsParent.GetComponentsInChildren<AnswerElement>();
    }

    // このPresenterが管理する全てのUIを非表示に
    public void HideAllVotePanels()
    {
        if (mainVotePanel != null) mainVotePanel.SetActive(false);
        // if (waitingPanel != null) waitingPanel.SetActive(false);
        // if (confirmationPanel != null) confirmationPanel.SetActive(false);
    }

    //答えを表示
    public void ShowAnswers((string name, string answer)[] answers)
    {
        for (int i = 0; i < answerElements.Length; i++)
        {
            if (i < answers.Length)
            {
                (string name, string answer) = answers[i];
                answerElements[i].SetView(name, answer);
                answerElements[i].gameObject.SetActive(true);
            }
            else
            {
                answerElements[i].gameObject.SetActive(false);
            }
        }   
    }

    //投票者か投票者じゃないかで見た目を変える
    public void ShowVoterView(bool isVoter)
    {
        if (isVoter)
        {
            voterOb.SetActive(true);
            nonvoterOb.SetActive(false);
        }
        else
        {
            voterOb.SetActive(false);
            nonvoterOb.SetActive(true);
        }
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
            return;
        }

        HideAllVotePanels(); // 他のパネルを隠す
        mainVotePanel.SetActive(true);

        this.onTargetSelectedCallback = onSelected;

        // 指示テキストを更新
        if (instructionText != null)
        {
            instructionText.text = "投票してください"; // 今回の目的に合わせてテキストを変更
        }

        // 以前のボタンをクリア
        foreach (Transform child in voteButtonContainer)
        {
            Destroy(child.gameObject);
        }

        if (targetList == null || targetList.Count == 0)
        {
            if (instructionText != null) instructionText.text = "表示するプレイヤーがいません";
            return;
        }

        // リスト内の各キャラクターに対応するボタンを生成
        foreach (IPlayerCharacter target in targetList)
        {
            Debug.Log("player:" + target.Displayname);
            GameObject buttonGO = Instantiate(voteButtonPrefab, voteButtonContainer);

            // TextMeshProを使っている場合
            TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
            // 標準のUI Textを使っている場合は、下の行のコメントを解除して上の行をコメントアウト
            // Text buttonText = buttonGO.GetComponentInChildren<Text>();

            if (buttonText != null)
            {
                buttonText.text = target.Displayname; // キャラクターの名前を表示
            }

            Button button = buttonGO.GetComponent<Button>();
            if (button != null)
            {
                // onSelectedがnull、または投票機能がまだ不要な場合は、
                // ボタンを操作不可にしておく
                button.interactable = (onSelected != null);

                if (onSelected != null)
                {
                    button.onClick.AddListener(() => {
                        this.onTargetSelectedCallback?.Invoke(target);
                    });
                }
            }
        }
    }
}
