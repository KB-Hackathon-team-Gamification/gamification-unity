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

    private List<Button> currentAnswerButtons = new List<Button>();

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
    private string[] answers;

    private string[][] answerOptions = new string[][]
    {
        new string[] { "��", "�ƴϿ�" },
        new string[] { "��", "�ƴϿ�" },
        new string[] { "��", "�ƴϿ�" },
        new string[] { "��", "�ƴϿ�" },
        new string[] { "��", "�ƴϿ�" },
        new string[] { "��", "�ƴϿ�" },
        new string[] { "��", "�ƴϿ�" },
        new string[] { "��", "�ƴϿ�" },
        new string[] { "��", "�ƴϿ�" },
        new string[] { "��", "�ƴϿ�" }
    };

    private int[] selectedAnswers;
    private int yesCount = 0;

    void Start()
    {
        // �迭 �ʱ�ȭ�� ���� ���� ����
        selectedAnswers = new int[questions.Length];
        for (int i = 0; i < selectedAnswers.Length; i++)
        {
            selectedAnswers[i] = -1;  // -1�� ���� �亯���� ������ �ǹ�
        }
        // GradePanel �ʱ⿡ ����
        if (gradePanel != null)
            gradePanel.SetActive(false);
        // Ȯ�� ��ư �̺�Ʈ ������ �߰�
        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveAllListeners();  // ���� ������ ����
            confirmButton.onClick.AddListener(OnConfirmButtonClick);
        }
        // ����Ʈ �ʱ�ȭ
        currentAnswerButtons = new List<Button>();

        // ���� �ʱ�ȭ
        InitializeSurvey();
    }
    private void OnConfirmButtonClick()
    {
        // ���� �Ϸ� �� �г� �ݱ�
        surveyPanel.SetActive(false);

        // �ʿ��� ��� �ʱ� ���·� ����
        currentQuestionIndex = 0;
        yesCount = 0;

        // ��ư�� ����
        foreach (var button in currentAnswerButtons)
        {
            if (button != null)
            {
                Destroy(button.gameObject);
            }
        }
        currentAnswerButtons.Clear();
    }
    public void SelectAnswer(int answerIndex)
    {
        // �ʱ�ȭ �˻�
        if (selectedAnswers == null)
        {
            Debug.LogError("Selected answers array is not properly initialized");
            selectedAnswers = new int[questions.Length];
            for (int i = 0; i < selectedAnswers.Length; i++)
            {
                selectedAnswers[i] = -1;
            }
        }

        if (currentQuestionIndex >= questions.Length)
        {
            Debug.LogError($"Question index {currentQuestionIndex} is out of range");
            return;
        }

        // ���� �亯�� "��"���ٸ� ī��Ʈ ����
        if (selectedAnswers[currentQuestionIndex] == 0)
        {
            yesCount--;
        }

        selectedAnswers[currentQuestionIndex] = answerIndex;

        // ���ο� �亯�� "��"��� ī��Ʈ ����
        if (answerIndex == 0)
        {
            yesCount++;
        }

        UpdateInvestmentGrade();

        if (currentAnswerButtons != null && answerIndex < currentAnswerButtons.Count)
        {
            HighlightSelectedButton(currentAnswerButtons[answerIndex]);
        }

        // �亯 ���� �� ��� ��� �� ���� �������� �̵�
        StartCoroutine(MoveToNextQuestionAfterDelay());
    }
    private IEnumerator MoveToNextQuestionAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);

        if (currentQuestionIndex < questions.Length - 1)
        {
            currentQuestionIndex++;
            ShowCurrentQuestion();
        }
        else
        {
            CompleteSurvey();
        }
    }
    private void UpdateInvestmentGrade()
    {
        // ��� ������ ���ϱ� �������� ����� �������� ����
        if (gradeText == null) return;

        bool allQuestionsAnswered = true;
        for (int i = 0; i < selectedAnswers.Length; i++)
        {
            if (selectedAnswers[i] == -1)
            {
                allQuestionsAnswered = false;
                break;
            }
        }

        if (!allQuestionsAnswered)
        {
            gradeText.text = "";  // �ؽ�Ʈ�� ���
            return;
        }

        string grade;
        string description = "";
        if (yesCount <= 1)
        {
            grade = "6���(�ſ쳷������)";
            description = "���� �ս��� ������ �ſ� ���� ��ǰ�� ��ȣ�ϴ� ������";
        }
        else if (yesCount <= 3)
        {
            grade = "5���(��������)";
            description = "���� �ս��� ������ ���� ��ǰ�� ��ȣ�ϴ� ������";
        }
        else if (yesCount <= 5)
        {
            grade = "4���(�߸���)";
            description = "���� �սǰ� ���� ���ɼ��� ������ �̷�� ��ǰ�� ��ȣ�ϴ� ������";
        }
        else if (yesCount <= 7)
        {
            grade = "3���(����������)";
            description = "���� ������ ���� ���� ������ ������ �� �ִ� ������";
        }
        else if (yesCount <= 9)
        {
            grade = "2���(����������)";
            description = "�ſ� ���� ������ ���� ���� ������ ������ �� �ִ� ������";
        }
        else
        {
            grade = "1���(���輱ȣ��)";
            description = "���� ������ ���������� �����ϰ� ���� ������ �߱��ϴ� ������";
        }

        gradeText.text = $"���ڼ��� �м� ���\n\n{grade}\n\n{description}";
    }

    private void CompleteSurvey()
    {
        // ���� ��� ������Ʈ
        string grade = "";
        string description = "";

        if (yesCount <= 1)
        {
            grade = "6���(�ſ쳷������)";
            description = "���� �ս��� ������ �ſ� ���� ��ǰ�� ��ȣ�ϴ� ������";
        }
        else if (yesCount <= 3)
        {
            grade = "5���(��������)";
            description = "���� �ս��� ������ ���� ��ǰ�� ��ȣ�ϴ� ������";
        }
        else if (yesCount <= 5)
        {
            grade = "4���(�߸���)";
            description = "���� �սǰ� ���� ���ɼ��� ������ �̷�� ��ǰ�� ��ȣ�ϴ� ������";
        }
        else if (yesCount <= 7)
        {
            grade = "3���(����������)";
            description = "���� ������ ���� ���� ������ ������ �� �ִ� ������";
        }
        else if (yesCount <= 9)
        {
            grade = "2���(����������)";
            description = "�ſ� ���� ������ ���� ���� ������ ������ �� �ִ� ������";
        }
        else
        {
            grade = "1���(���輱ȣ��)";
            description = "���� ������ ���������� �����ϰ� ���� ������ �߱��ϴ� ������";
        }

        // ��� ǥ��
        if (questionText != null)
        {
            questionText.text = $"\n\n���ڼ��� �м� ���\n\n{grade}\n\n{description}";
        }

        // �亯 �г� �����
        if (answerPanel != null)
        {
            answerPanel.SetActive(false);
        }

        Debug.Log($"������ �Ϸ�Ǿ����ϴ�. ���� ���: {grade}");
    }
    private IEnumerator CloseSurveyAfterDelay()
    {
        yield return new WaitForSeconds(3f);  // 3�� �Ŀ� �ݱ�
        surveyPanel.SetActive(false);
    }

    private void InitializeSurvey()
    {
        if (selectedAnswers == null || selectedAnswers.Length != questions.Length)
        {
            selectedAnswers = new int[questions.Length];
            for (int i = 0; i < selectedAnswers.Length; i++)
            {
                selectedAnswers[i] = -1;
            }
        }

        currentQuestionIndex = 0;
        yesCount = 0;
        ShowCurrentQuestion();
    }

    private void ShowCurrentQuestion()
    {
        if (questionText != null)
        {
            questionText.text = $"Q{currentQuestionIndex+1}. {questions[currentQuestionIndex]}";
        }

        // ���� ��ư�� ����
        if (currentAnswerButtons != null)
        {
            foreach (var button in currentAnswerButtons)
            {
                if (button != null)
                {
                    Destroy(button.gameObject);
                }
            }
            currentAnswerButtons.Clear();
        }

        // null üũ �߰�
        if (answerPanel == null || answerButtonPrefab == null)
        {
            Debug.LogError("Answer panel or button prefab is not assigned");
            return;
        }

        string[] currentAnswers = answerOptions[currentQuestionIndex];
        for (int i = 0; i < 2; i++)
        {
            // ��ư ����
            Button newButton = Instantiate(answerButtonPrefab, answerPanel.transform);
            newButton.name = $"AnswerButton_{i}";

            // ��� Image ������Ʈ ã��
            Image[] images = newButton.GetComponentsInChildren<Image>();
            foreach (var image in images)
            {
                // ��ư�� ���� Image�� �����ϰ� �������� ��Ȱ��ȭ
                if (image.gameObject == newButton.gameObject)
                {
                    image.color = Color.white;
                }
                else
                {
                    image.enabled = false;
                }
            }

            // Text ����
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = currentAnswers[i];  // "��" �Ǵ� "�ƴϿ�"
            }

            int answerIndex = i;
            newButton.onClick.AddListener(() => SelectAnswer(answerIndex));
            currentAnswerButtons.Add(newButton);
        }
    }
    private void HighlightSelectedButton(Button selectedButton)
    {
        if (currentAnswerButtons == null) return;

        foreach (var button in currentAnswerButtons)
        {
            if (button != null)
            {
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.color = Color.black;
                }
            }
        }

        TextMeshProUGUI selectedText = selectedButton.GetComponentInChildren<TextMeshProUGUI>();
        if (selectedText != null)
        {
            selectedText.color = Color.blue;  // ���õ� �ؽ�Ʈ ���� ����
        }
    }


}