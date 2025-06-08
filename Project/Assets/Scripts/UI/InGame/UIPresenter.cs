using UnityEngine;

public class UIPresenter : MonoBehaviour
{
    [SerializeField] private GameObject distributionOb;
    [SerializeField] private GameObject questionOb;
    [SerializeField] private GameObject answerOb;
    [SerializeField] private GameObject voteOb;
    [SerializeField] private GameObject resultOb;


    public void ResetView()
    {
        distributionOb.SetActive(false);
        questionOb.SetActive(false);
        answerOb.SetActive(false);
        voteOb.SetActive(false);
        resultOb.SetActive(false);
    }
}
