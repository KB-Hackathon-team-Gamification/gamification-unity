using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class InvestmentSurveyManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject surveyPanel;
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private GameObject gradePanel;
    [SerializeField] private GameObject answerPanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button answerButtonPrefab;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button urlButton;

    private Button yesButton;
    private Button noButton;
    private bool buttonsInitialized = false;

    [Header("Survey Questions")]
    private string[] questions = new string[]
    {
        "최근 1년간 주식, 펀드 등 투자 경험이 있으신가요?",
        "투자 손실이 발생해도 감내할 수 있으신가요?",
        "정기적인 수입이 있으신가요?",
        "비상자금을 별도로 보유하고 계신가요?",
        "장기 투자(3년 이상)를 고려하고 계신가요?",
        "투자 관련 정보를 정기적으로 확인하시나요?",
        "분산투자의 개념을 이해하고 계신가요?",
        "투자 위험을 감수하더라도 높은 수익을 추구하시나요?",
        "정기적으로 저축이나 투자를 하고 계신가요?",
        "금융상품의 위험도를 이해하고 계신가요?"
    };

    private int currentQuestionIndex = 0;
    private int[] selectedAnswers;
    private int yesCount = 0;

    void Start()
    {
        if (!ValidateReferences())
        {
            Debug.LogError("Some references are missing. Please check the Inspector!");
            return;
        }
        InitializeUI();
        SetupButtons();
        // 시작 시 바로 설문 시작
        BeginSurvey();
    }
    private void InitializeUI()
    {
        selectedAnswers = new int[questions.Length];
        for (int i = 0; i < selectedAnswers.Length; i++)
            selectedAnswers[i] = -1;

        // 초기 UI 상태 설정
        surveyPanel.SetActive(true);
        contentPanel.SetActive(true);
        gradePanel.SetActive(false);

        if (!buttonsInitialized)
        {
            CreateAnswerButtons();
            buttonsInitialized = true;
        }
    }
    private void BeginSurvey()
    {
        surveyPanel.SetActive(true);
        contentPanel.SetActive(true);
        gradePanel.SetActive(false);
        InitializeSurvey();
    }

    private void CreateAnswerButtons()
    {
        if (answerPanel == null || answerButtonPrefab == null) return;

        // 기존 버튼 모두 제거
        foreach (Transform child in answerPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // 예 버튼 생성
        yesButton = Instantiate(answerButtonPrefab, answerPanel.transform);
        RectTransform yesRect = yesButton.GetComponent<RectTransform>();
        yesRect.sizeDelta = new Vector2(230f, 50f);  // 너비 230, 높이 50으로 설정
        TextMeshProUGUI yesText = yesButton.GetComponentInChildren<TextMeshProUGUI>();
        if (yesText != null)
        {
            yesText.text = "예";
            //yesText.fontSize = 50;
            yesText.alignment = TextAlignmentOptions.Center; // 텍스트 중앙 정렬
        }
        yesButton.onClick.AddListener(() => SelectAnswer(0));

        // 아니오 버튼 생성
        noButton = Instantiate(answerButtonPrefab, answerPanel.transform);
        RectTransform noRect = noButton.GetComponent<RectTransform>();
        noRect.sizeDelta = new Vector2(230f, 50f);  // 너비 230, 높이 50으로 설정
        TextMeshProUGUI noText = noButton.GetComponentInChildren<TextMeshProUGUI>();
        if (noText != null)
        {
            noText.text = "아니오";
            noText.alignment = TextAlignmentOptions.Center; // 텍스트 중앙 정렬
        }
        noButton.onClick.AddListener(() => SelectAnswer(1));
    }
    private bool ValidateReferences()
    {
        if (surveyPanel == null || contentPanel == null || gradePanel == null ||
            questionText == null || answerPanel == null ||
            answerButtonPrefab == null || gradeText == null || confirmButton == null)
        {
            Debug.LogError("Required references are missing!");
            return false;
        }
        return true;
    }

    private void SetupButtons()
    {
        // Confirm 버튼 설정
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
        confirmButton.gameObject.SetActive(false);

        // URL 버튼 설정
        urlButton.onClick.RemoveAllListeners();
        urlButton.onClick.AddListener(OnUrlButtonClick);
        urlButton.gameObject.SetActive(false);
    }
    private void OnUrlButtonClick()
    {
        Application.OpenURL("https://obank.kbstar.com/quics?page=C020710");
    }
    private void OnConfirmButtonClick()
    {
        // 설문 재시작
        contentPanel.SetActive(true);
        gradePanel.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        urlButton.gameObject.SetActive(false);    // URL 버튼 비활성화
        ResetSurveyState();
        BeginSurvey();
    }

    private void ResetSurveyState()
    {
        currentQuestionIndex = 0;
        yesCount = 0;

        for (int i = 0; i < selectedAnswers.Length; i++)
        {
            selectedAnswers[i] = -1;
        }

        if (questionText != null)
            questionText.gameObject.SetActive(true);

        if (answerPanel != null)
            answerPanel.SetActive(true);

        ResetButtonStates();
        buttonsInitialized = false;
    }


    public void SelectAnswer(int answerIndex)
    {
        if (currentQuestionIndex >= questions.Length) return;

        if (selectedAnswers[currentQuestionIndex] == 0)
            yesCount--;

        selectedAnswers[currentQuestionIndex] = answerIndex;

        if (answerIndex == 0)
            yesCount++;

        HighlightSelectedButton(answerIndex == 0 ? yesButton : noButton);
        StartCoroutine(MoveToNextQuestionAfterDelay());
    }

    private IEnumerator MoveToNextQuestionAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        if (currentQuestionIndex < questions.Length - 1)
        {
            currentQuestionIndex++;
            ShowCurrentQuestion();
            ResetButtonStates();
        }
        else
        {
            CompleteSurvey();
        }
    }

    private void ResetButtonStates()
    {
        if (yesButton != null) yesButton.interactable = true;
        if (noButton != null) noButton.interactable = true;
    }

    private (string grade, string description) CalculateGrade()
    {
        if (yesCount <= 1) return ("6등급(매우낮은위험)", "원금 손실의 위험이 매우 낮은 상품을 선호하는 투자자");
        if (yesCount <= 3) return ("5등급(낮은위험)", "원금 손실의 위험이 낮은 상품을 선호하는 투자자");
        if (yesCount <= 5) return ("4등급(중립형)", "원금 손실과 이익 가능성이 균형을 이루는 상품을 선호하는 투자자");
        if (yesCount <= 7) return ("3등급(적극투자형)", "높은 수익을 위해 높은 위험을 감수할 수 있는 투자자");
        if (yesCount <= 9) return ("2등급(공격투자형)", "매우 높은 수익을 위해 높은 위험을 감수할 수 있는 투자자");
        return ("1등급(위험선호형)", "투자 위험을 적극적으로 수용하고 높은 수익을 추구하는 투자자");
    }

    private void CompleteSurvey()
    {
        // ContentPanel 비활성화
        contentPanel.SetActive(false);

        // GradePanel 활성화 및 결과 표시
        gradePanel.SetActive(true);
        confirmButton.gameObject.SetActive(true);
        urlButton.gameObject.SetActive(true);    // URL 버튼 활성화

        var (grade, description) = CalculateGrade();
        gradeText.text = $"고객님의 투자성향은\n\n{grade}\n{description}입니다.";
    }
    private void InitializeSurvey()
    {
        currentQuestionIndex = 0;
        yesCount = 0;

        // UI 초기화
        questionText.gameObject.SetActive(true);
        answerPanel.SetActive(true);
        ResetButtonStates();
        ShowCurrentQuestion();
    }
    private void ShowCurrentQuestion()
    {
        if (questionText != null && currentQuestionIndex < questions.Length)
        {
            questionText.text = $"Q{currentQuestionIndex + 1}. {questions[currentQuestionIndex]}";
        }
    }

    private void HighlightSelectedButton(Button selectedButton)
    {
        if (yesButton != null) yesButton.interactable = true;
        if (noButton != null) noButton.interactable = true;
        selectedButton.interactable = false;
    }
}