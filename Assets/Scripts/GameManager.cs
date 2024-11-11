using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// 대화창 UI 구축하기 [유니티 기초 강좌 B22] - 호진
// PlayerAction 과 연결되어 있음. 
public class GameManager :  MonoBehaviour
{
    public TalkManager talkManager;
    public GameObject talkPanel;
    public Image protraitImg;
    public TextMeshProUGUI talkText;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex; 
    public void Action(GameObject scanObj) 
    {
       scanObject = scanObj;
       ObjData objData = scanObject.GetComponent<ObjData>();
       Talk(objData.id, objData.isNpc);

       talkPanel.SetActive(isAction);
    }

    void Talk(int id, bool isNpc)
    {
      string talkData = talkManager.GetTalk(id, talkIndex);

        if (talkData == null) // 대화가 더 이상 없으면 종료
        {
            isAction = false;
            talkIndex = 0; // 이야기가 종료되면 초기화. 
            return;
        }

        if (isNpc)
        {
            talkText.text = talkData;

            protraitImg.color = new Color(1, 1, 1, 1); // npc라면 투명도 1(드러냄)
        }
        else { 
            talkText.text = talkData;
            protraitImg.color = new Color(1, 1, 1, 0); // npc가 아니라면 투명도 0(숨김) 

        }
        isAction = true;
        talkIndex++; // 문장 뽑아내기. 
    }
}
