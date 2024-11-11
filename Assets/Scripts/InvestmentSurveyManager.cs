using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class InvestmentSurveyManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject surveyPanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private GameObject answerPanel;
    [SerializeField] private Button answerButtonPrefab;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private GameObject gradePanel;
    [SerializeField] private Button confirmButton;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private Button startButton;

    private Button yesButton;
    private Button noButton;

    [Header("Survey Questions")]
    private string[] questions = new string[]
    {
        "�ֱ� 1�Ⱓ �ֽ�, �ݵ� �� ���� ������ �����Ű���?",
        "���� �ս��� �߻��ص� ������ �� �����Ű���?",
        "�������� ������ �����Ű���?",
        "����ڱ��� ������ �����ϰ� ��Ű���?",
        "��� ����(3�� �̻�)�� ����ϰ� ��Ű���?",
        "���� ���� ������ ���������� Ȯ���Ͻó���?",
        "�л������� ������ �����ϰ� ��Ű���?",
        "���� ������ �����ϴ��� ���� ������ �߱��Ͻó���?",
        "���������� �����̳� ���ڸ� �ϰ� ��Ű���?",
        "������ǰ�� ���赵�� �����ϰ� ��Ű���?"
    };

    private int currentQuestionIndex = 0;
    private int[] selectedAnswers;
    private int yesCount = 0;

    void Start()
    {
        InitializeUI();
        if (!ValidateReferences())
        {
            Debug.LogError("Some references are missing. Please check the Inspector!");
            return;
        }

        SetupButtons();
        CreateAnswerButtons();
    }
    private bool ValidateReferences()
    {
        bool isValid = true;

        // Reference üũ�� �ϵ�, �ʱ� UI ���¿��� ������ ���� �ʵ��� ����
        if (answerButtonPrefab == null)
        {
            Debug.LogError("Answer Button Prefab is not assigned!");
            isValid = false;
        }

        if (answerPanel == null)
        {
            Debug.LogError("Answer Panel is not assigned!");
            isValid = false;
        }

        if (surveyPanel == null)
        {
            Debug.LogError("Survey Panel is not assigned!");
            isValid = false;
        }

        if (startPanel == null)
        {
            Debug.LogError("Start Panel is not assigned!");
            isValid = false;
        }

        if (gradePanel == null)
        {
            Debug.LogError("Grade Panel is not assigned!");
            isValid = false;
        }

        if (questionText == null)
        {
            Debug.LogError("Question Text is not assigned!");
            isValid = false;
        }

        if (gradeText == null)
        {
            Debug.LogError("Grade Text is not assigned!");
            isValid = false;
        }

        if (startButton == null)
        {
            Debug.LogError("Start Button is not assigned!");
            isValid = false;
        }

        if (confirmButton == null)
        {
            Debug.LogError("Confirm Button is not assigned!");
            isValid = false;
        }

        return isValid;
    }

    private void InitializeUI()
    {
        selectedAnswers = new int[questions.Length];
        for (int i = 0; i < selectedAnswers.Length; i++)
            selectedAnswers[i] = -1;

        surveyPanel.SetActive(false);
        gradePanel.SetActive(false);
        startPanel.SetActive(true);
    }

    private void CreateAnswerButtons()
    {
        if (answerPanel == null || answerButtonPrefab == null) return;

        // Clear existing buttons
        foreach (Transform child in answerPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Create "Yes" button
        yesButton = Instantiate(answerButtonPrefab, answerPanel.transform);
        TextMeshProUGUI yesText = yesButton.GetComponentInChildren<TextMeshProUGUI>();
        if (yesText != null) yesText.text = "��";
        yesButton.onClick.AddListener(() => SelectAnswer(0));

        // Create "No" button
        noButton = Instantiate(answerButtonPrefab, answerPanel.transform);
        TextMeshProUGUI noText = noButton.GetComponentInChildren<TextMeshProUGUI>();
        if (noText != null) noText.text = "�ƴϿ�";
        noButton.onClick.AddListener(() => SelectAnswer(1));
    }

    private void SetupButtons()
    {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(OnStartButtonClick);

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
        confirmButton.gameObject.SetActive(false);
    }

    private void OnStartButtonClick()
    {
        surveyPanel.SetActive(true);
        startPanel.SetActive(false);
        InitializeSurvey();
    }

    private void OnConfirmButtonClick()
    {
        ResetSurveyState();
        startPanel.SetActive(true);
        surveyPanel.SetActive(false);
        gradePanel.SetActive(false);
    }

    private void ResetSurveyState()
    {
        currentQuestionIndex = 0;
        yesCount = 0;

        for (int i = 0; i < selectedAnswers.Length; i++)
        {
            selectedAnswers[i] = -1;
        }

        if (yesButton != null) yesButton.interactable = true;
        if (noButton != null) noButton.interactable = true;

        questionText.gameObject.SetActive(true);
        answerPanel.SetActive(true);
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
        if (yesCount <= 1) return ("6���(�ſ쳷������)", "���� �ս��� ������ �ſ� ���� ��ǰ�� ��ȣ�ϴ� ������");
        if (yesCount <= 3) return ("5���(��������)", "���� �ս��� ������ ���� ��ǰ�� ��ȣ�ϴ� ������");
        if (yesCount <= 5) return ("4���(�߸���)", "���� �սǰ� ���� ���ɼ��� ������ �̷�� ��ǰ�� ��ȣ�ϴ� ������");
        if (yesCount <= 7) return ("3���(����������)", "���� ������ ���� ���� ������ ������ �� �ִ� ������");
        if (yesCount <= 9) return ("2���(����������)", "�ſ� ���� ������ ���� ���� ������ ������ �� �ִ� ������");
        return ("1���(���輱ȣ��)", "���� ������ ���������� �����ϰ� ���� ������ �߱��ϴ� ������");
    }

    private void CompleteSurvey()
    {
        questionText.gameObject.SetActive(false);
        answerPanel.SetActive(false);

        gradePanel.SetActive(true);
        confirmButton.gameObject.SetActive(true);

        var (grade, description) = CalculateGrade();
        gradeText.text = $"���ڼ��� �м� ���\n\n{grade}\n\n{description}";
    }

    private void InitializeSurvey()
    {
        currentQuestionIndex = 0;
        yesCount = 0;
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