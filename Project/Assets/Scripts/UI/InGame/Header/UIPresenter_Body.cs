using UnityEngine;
using TMPro;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class UIPresenter_Body : MonoBehaviour
{
    [SerializeField] private GameObject jobDistributionOb;
    [SerializeField] private GameObject waitOb;
    [SerializeField] private GameObject questionOb;

    [SerializeField] private GameObject answerOb;
    [SerializeField] private GameObject executeOb;
    [SerializeField] private GameObject resultOb;

    [SerializeField] private TMP_Text roleText;
    [SerializeField] private CharacterIcon distributionCharacterIcon;
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private TMP_Text waitText;

    [SerializeField] private TMP_Text questionText;
    [SerializeField] private CharacterIcon questionCharacterIcon;
    [SerializeField] private CharacterIcon executeCharacterIcon;

    [SerializeField] private TMP_Text executeText;
    [SerializeField] private TMP_Text resultText;

    [Header("Answers")]
    [SerializeField] private Transform elementsParent; // 投票UI全体の親オブジェクト
    private AnswerElement[] answerElements;

    void Awake()
    {
        answerElements = elementsParent.GetComponentsInChildren<AnswerElement>();
    }

    public void Hide()
    {
        jobDistributionOb.SetActive(false);
        waitOb.SetActive(false);
        answerOb.SetActive(false);
        executeOb.SetActive(false);
        resultOb.SetActive(false);
        questionOb.SetActive(false);
    }

    public void ShowDistribution(Role role, Sprite sprite, string chara, string description)
    {
        jobDistributionOb.SetActive(true);
        waitOb.SetActive(false);
        answerOb.SetActive(false);
        executeOb.SetActive(false);
        resultOb.SetActive(false);

        roleText.text = $"あなたは{role}です";
        distributionCharacterIcon.Set(sprite, chara);
        descriptionText.text = $"性格：{chara}\n{description}";
    }

    public void ShowWait(string text)
    {
        jobDistributionOb.SetActive(false);
        waitOb.SetActive(true);
        answerOb.SetActive(false);
        executeOb.SetActive(false);
        resultOb.SetActive(false);

        waitText.text = text;
    }

    public void ShowQuestion(Sprite sprite, string name, string question)
    {
        jobDistributionOb.SetActive(false);
        waitOb.SetActive(false);
        questionOb.SetActive(true);

        questionCharacterIcon.Set(sprite, name);
        questionText.text = question;
    }

    public void ShowQuestion(string question)
    {
        jobDistributionOb.SetActive(false);
        waitOb.SetActive(false);
        questionOb.SetActive(true);
        questionText.text = question;
    }

    public void HideQuestion()
    {
        questionOb.SetActive(false);
    }

    public void ShowAnswerIcons((Sprite sprite, string name)[] icons)
    {
        waitOb.SetActive(false);
        answerOb.SetActive(true);
        executeOb.SetActive(false);
        resultOb.SetActive(false);

        for (int i = 0; i < answerElements.Length; i++)
        {
            if (i < icons.Length)
            {
                (Sprite sprite, string name) = icons[i];
                answerElements[i].SetIcon(sprite, name);
                answerElements[i].SetAnswerText("");
                answerElements[i].HideButton();
                answerElements[i].gameObject.SetActive(true);
            }
            else
            {
                answerElements[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowAnswersThinking()
    {
        waitOb.SetActive(false);
        answerOb.SetActive(true);
        executeOb.SetActive(false);
        resultOb.SetActive(false);

        for (int i = 0; i < answerElements.Length; i++)
        {
            answerElements[i].SetAnswerText("");

            if (answerElements[i].gameObject.activeSelf)
            {
                answerElements[i].SetAnswerText("考え中・・・");
                answerElements[i].HideButton();
                answerElements[i].gameObject.SetActive(true);
            }
            else
            {
                answerElements[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowAnswers(string[] text)
    {
        waitOb.SetActive(false);
        answerOb.SetActive(true);
        executeOb.SetActive(false);
        resultOb.SetActive(false);

        for (int i = 0; i < answerElements.Length; i++)
        {
            if (i < text.Length)
            {
                answerElements[i].SetAnswerText(text[i]);
                answerElements[i].HideButton();
                answerElements[i].gameObject.SetActive(true);
            }
            else
            {
                answerElements[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowAnswers((string, IPlayerCharacter playerCharacter)[] answers,System.Action<IPlayerCharacter> action)
    {
        jobDistributionOb.SetActive(false);
        waitOb.SetActive(false);
        answerOb.SetActive(true);
        executeOb.SetActive(false);
        resultOb.SetActive(false);

        for (int i = 0; i < answerElements.Length; i++)
        {
            if (i < answers.Length)
            {
                (string answer, IPlayerCharacter player) = answers[i];
                answerElements[i].SetAnswerText(answer);
                answerElements[i].ShowButton(player, action);
                answerElements[i].gameObject.SetActive(true);
            }
            else
            {
                answerElements[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowExecute(Sprite executePlayerSprite, string executePlayerName)
    {
        jobDistributionOb.SetActive(false);
        waitOb.SetActive(false);
        answerOb.SetActive(false);
        executeOb.SetActive(true);
        resultOb.SetActive(false);

        executeCharacterIcon.Set(executePlayerSprite, executePlayerName);
        executeText.text = $"{executePlayerName}が処刑されました...。";
    }

    public void ShowResult(string result)
    {
        jobDistributionOb.SetActive(false);
        waitOb.SetActive(false);
        answerOb.SetActive(false);
        executeOb.SetActive(false);
        resultOb.SetActive(true);

        resultText.text = result;
    }
}
