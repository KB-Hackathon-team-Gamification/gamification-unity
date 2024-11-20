using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// 대화창 UI 구축하기 [유니티 기초 강좌 B22] - 호진
// PlayerAction 과 연결되어 있음. 
public class GameManager :  MonoBehaviour
{
    public GameObject choicePanel;
    public GameObject financeQuizPanel;  
    public GameObject surveyPanel; // SurveyPanel 추가
    private int currentNpcId; // 현재 NPC의 ID를 저장할 변수
    public Button yesButton;
    public Button noButton;
    public TextMeshProUGUI yesText;  
    public TextMeshProUGUI noText;
    public Image arrow;

    private bool isYesSelected = true; // 현재 선택이 "예"인지 "아니오"인지 확인하는 변수


    public TalkManager talkManager;
    public QuestManager questManager;
    public Animator talkPanel;
    public Animator protraitIAnim;
    public Image protraitImg;
    public Sprite prevPortrait;
    public TypeEffect talk;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;

    void Start()
    {
        // choicePanel 초기화 및 HideFlags 설정
        if (choicePanel != null)
        {
            choicePanel.hideFlags = HideFlags.None;
            choicePanel.SetActive(false);
        }
        else
        {
            Debug.LogError("choicePanel이 할당되지 않았습니다");
        }


        // financeQuizPanel 비활성화
        if (financeQuizPanel != null)
        {
            financeQuizPanel.SetActive(false);
            Debug.Log("FinanceQuizPanel 초기화됨.");
        }
        else
        {
            Debug.LogError("FinanceQuiz 오브젝트가 Inspector에서 연결되지 않았습니다.");
        }

        // surveyPanel 초기화 및 비활성화
        if (surveyPanel != null)
        {
            surveyPanel.SetActive(false); // 기본적으로 비활성화
            Debug.Log("SurveyPanel 초기화됨.");

        }else
        {
            Debug.LogError("surveyPanel 오브젝트가 Inspector에서 연결되지 않았습니다.");

        }

        // 버튼 클릭 이벤트 연결
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
    }

    void Update()
    {
        if (choicePanel.activeSelf)
        {
            HandleArrowMovement();
        }
    }

    public void Action(GameObject scanObj) 
    {
        // Get Current Object
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        currentNpcId = objData.id; // NPC의 ID를 저장
        Talk(objData.id, objData.isNpc);

        // visible talk for action
        isAction = true;
        Debug.Log("isShow: 0 " + isAction);
       talkPanel.SetBool("isShow", isAction);
    }
        


    void Talk(int id, bool isNpc)
    {
        string talkData = "";
         // 애니메이션 실행 중이면 대화를 초기화하지 않고 리턴
        if (talk.isAnim){ 
            talk.SetMsg("");  
            return;
        }

        // 기본 대화 데이터 가져오기
        talkData = talkManager.GetTalk(id, talkIndex);
     
        // 대화가 더 이상 없으면 종료
        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0; // 대화 종료 후 초기화
            return;
        }

        // 대화 출력 로직
        if (isNpc)
        {
            talk.SetMsg(talkData); // 대화 텍스트 설정
            protraitImg.color = new Color(1, 1, 1, 0); // NPC 초상화 표시
            protraitIAnim.SetTrigger("doEffect"); // 초상화 애니메이션
            prevPortrait = protraitImg.sprite; // 이전 초상화 저장
        }
        else 
        {
            talk.SetMsg(talkData); // 대화 텍스트 설정
            protraitImg.color = new Color(1, 1, 1, 0); // NPC 초상화 숨기기
        }

        isAction = true; // 대화 활성화
        talkIndex++; // 다음 문장으로 진행. 

        // 대화의 마지막 문장인지 확인
        int totalLines = talkManager.GetTalkDataLength(id);
        if (talkIndex == totalLines) // 마지막 문장일 경우
        {
            ShowChoice();
        }
    }


    void ShowChoice()
    {
        // 선택지를 활성화합니다.
        choicePanel.SetActive(true);
    }

void OnYesButtonClicked()
{
    choicePanel.SetActive(false);

        talkIndex = 0;
        Debug.Log("talkIdx  초기화");
    // NPC ID에 따라 패널 활성화
    if (currentNpcId == 1000)
    {
        Debug.Log("현재 대화정은 npc의 id는" + currentNpcId);

        // financeQuizPanel 실행
        if (financeQuizPanel != null)
        {
            // talkPanel이 활성화되어 있다면 비활성화
            if (talkPanel != null && talkPanel.gameObject.activeSelf)
            {
                // talkPanel.gameObject.SetActive(false);
        isAction = false;
       talkPanel.SetBool("isShow", isAction);
                // Debug.Log("TalkPanel이 비활성화되었습니다.1");
            }

            financeQuizPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("FinanceQuizPanel이 연결되지 않았습니다.");
        }
    }
    else if (currentNpcId == 2000)
    {
        // surveyPanel 실행
        if (surveyPanel != null)
        {
            // talkPanel이 활성화되어 있다면 비활성화
            if (talkPanel != null && talkPanel.gameObject.activeSelf)
            {
        isAction = false;
       talkPanel.SetBool("isShow", isAction);
                // talkPanel.gameObject.SetActive(false);
                // Debug.Log("TalkPanel이 비활성화되었습니다.2");
            }

            surveyPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("SurveyPanel이 연결되지 않았습니다.");
        }
    }
    else
    {
        Debug.LogWarning("알 수 없는 NPC ID입니다: " + currentNpcId);
    }

    // 대화 상태 종료
    isAction = false;  
}


    void OnNoButtonClicked()
    {
        // Debug.Log("플레이어가 '아니오'를 선택했습니다.");
        choicePanel.SetActive(false);
        isAction = false;
       talkPanel.SetBool("isShow", isAction);
        talkIndex = 0;
        Debug.Log("talkIdx  초기화");
        // 아니오를 선택했을 때 실행할 로직 추가
    }

    public void CloseQuiz()
    {
        Debug.Log("닫기 버튼이 클릭되었습니다.");
        if (financeQuizPanel != null)
        {
            financeQuizPanel.SetActive(false); // FinanceQuiz 패널을 비활성화하여 창을 닫음
        }
        else
        {
            Debug.LogError("FinanceQuiz 패널을 찾을 수 없습니다.");
        }

        // 대화창 다시 활성화
        if (talkPanel != null)
        {
            talkPanel.gameObject.SetActive(true); // 대화창 다시 활성화
            talkPanel.SetBool("isShow", true); // 애니메이션 상태 설정
            Debug.Log("TalkPanel이 다시 활성화되었습니다.");
        }
        else
        {
            Debug.LogError("talkPanel이 할당되지 않았습니다. Inspector에서 talkPanel을 연결해주세요.");
        }
    }

    public void CloseSurvey()
    {
        Debug.Log("설문조사가 종료되었습니다.");
        if (surveyPanel != null)
        {
            surveyPanel.SetActive(false); // SurveyPanel 비활성화하여 창을 닫음
        }
        else
        {
            Debug.LogError("SurveyPanel을 찾을 수 없습니다.");
        }

        // 대화창 다시 활성화
        if (talkPanel != null)
        {
            talkPanel.gameObject.SetActive(true); // 대화창 다시 활성화
            Debug.Log("TalkPanel이 다시 활성화되었습니다.");
        }
        else
        {
            Debug.LogError("talkPanel이 할당되지 않았습니다. Inspector에서 talkPanel을 연결해주세요.");
        }
    }


    // 화살표 위치를 변경하는 함수
    void HandleArrowMovement()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            isYesSelected = !isYesSelected; // 선택지를 전환
            UpdateArrowPosition();
        }

        if (Input.GetKeyDown(KeyCode.Return)) // Enter 키를 누르면 선택이 확정됨
        {
            ConfirmChoice();
        }
    }

    // 화살표 위치를 업데이트하는 함수
    void UpdateArrowPosition()
    {
        if (isYesSelected)
        {
            arrow.transform.position = new Vector3(arrow.transform.position.x, yesText.transform.position.y, 0);
        }
        else
        {
            arrow.transform.position = new Vector3(arrow.transform.position.x, noText.transform.position.y, 0);
        }
    }

    // 선택 확정 처리
    void ConfirmChoice()
    {
        choicePanel.SetActive(false);
        if (isYesSelected)
        {
            OnYesButtonClicked();
        }
        else
        {
            OnNoButtonClicked();
        }
    }




}
