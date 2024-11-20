using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Button oButton;
    public Button xButton;

    public Button linkButton1;
    public Button linkButton2;
    public Image financeImage;

    public GameObject financeQuizPanel; 

    private string[] questions = {
        "주식은 회사의 소유권을 나타낸다.",
        "채권은 주식보다 위험하다."
    };
    private bool[] answers = { true, false };

    private int currentQuestionIndex = 0;
    private int score = 0;

    void Start()
    {
        ShowQuestion();
        linkButton1.gameObject.SetActive(false);    
        linkButton2.gameObject.SetActive(false);
        financeImage.gameObject.SetActive(false);
    }

    public void OnOButtonClicked()
    {
        Debug.Log("O 버튼이 눌렸습니다.");
        CheckAnswer(true);
    }

    public void OnXButtonClicked()
    {
        Debug.Log("X 버튼이 눌렸습니다.");
        CheckAnswer(false);
    }

    public void OpenLink1()
    {
       RestartGame(); // 게임을 다시 시작

    }

    public void OpenLink2()
    {
        EndGame();
    }

    void ShowQuestion()
    {


        questionText.text = questions[currentQuestionIndex];
    }

    void CheckAnswer(bool playerAnswer)
    {
        // (1) 범위 초과 확인
        if (currentQuestionIndex >= answers.Length)
        {
            Debug.LogError("QuizManager 범위 초과 " + currentQuestionIndex);
            return;
        }

        if (playerAnswer == answers[currentQuestionIndex])
        {
            score++;
        }

        currentQuestionIndex++;

        if (currentQuestionIndex < questions.Length)
        {
            ShowQuestion();
        }
        else
        {
            ShowResults();
        }
    }

    void ShowResults()
    {

        if (score == questions.Length) // 모든 문제를 맞힌 경우
        {
             // 결과 화면에서는 텍스트를 원래 위치로 복구
            AdjustQuestionTextPosition(80f);
            
            questionText.text = "축하합니다!\n모든 문제를 맞히셨습니다!";

            // 이미지를 보여줌
            financeImage.gameObject.SetActive(true);
        }
        else // 틀린 문제가 있는 경우
        {
            questionText.text = $"아쉽습니다.\n최종 점수: {score}/{questions.Length}";

            // 이미지를 숨김
            financeImage.gameObject.SetActive(false);
        }


        // O, X 버튼 비활성화하고, 링크 버튼과 이미지를 활성화합니다.
        oButton.gameObject.SetActive(false);
        xButton.gameObject.SetActive(false);

        linkButton1.gameObject.SetActive(true);
        linkButton2.gameObject.SetActive(true);
    }

    void RestartGame()
{
    // 점수 초기화
    score = 0;
    // 질문 인덱스 초기화
    currentQuestionIndex = 0;

    // UI 초기화
    oButton.gameObject.SetActive(true);
    xButton.gameObject.SetActive(true);
    linkButton1.gameObject.SetActive(false);
    linkButton2.gameObject.SetActive(false);
    financeImage.gameObject.SetActive(false);

    // 첫 번째 질문 다시 표시
    ShowQuestion();

    Debug.Log("게임이 다시 시작되었습니다.");
}

void EndGame()
{
    financeQuizPanel.SetActive(false);

    Debug.Log("게임이 종료되었습니다.");
}

void AdjustQuestionTextPosition(float yOffset)
{
    // RectTransform 가져오기
    RectTransform rectTransform = questionText.GetComponent<RectTransform>();

    // Y 위치를 절대적으로 설정
    rectTransform.anchoredPosition = new Vector2(
        rectTransform.anchoredPosition.x, // X 위치는 유지
        rectTransform.anchoredPosition.y + yOffset // 현재 Y 위치에 yOffset 추가
    );
    Debug.Log($"Adjusted Text Position: {rectTransform.anchoredPosition}");
    Canvas.ForceUpdateCanvases();

}


}
