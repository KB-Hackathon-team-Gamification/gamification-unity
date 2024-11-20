using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData; // 대화 데이터를 저장할 Dictionary
    Dictionary<int, Sprite> portraitData; // 캐릭터 초상화 데이터를 저장할 Dictionary

    public Sprite[] portraitArr;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData(); // 대화 데이터 생성
    }

    // 대화 데이터를 생성하는 메서드
    void GenerateData()
    {
    // 인물
    talkData.Add(1000, new string[] { 
        "안녕하세요! 저는 이 마을의 촌장, 민준입니다.", 
        "우리 금융 마을에 오신 걸 환영합니다.", 
        "과연 당신이 얼마나 금융 지식을 가지고 있는지 궁금하군요.",
        "모든 문제를 맞히면 특별한 상품을 드릴 테니 기대해보세요!"
    }); // ID 1000 대화 데이터

    talkData.Add(2000, new string[] { 
        "안녕! 잘 찾아왔군요.", 
        "저는 금융 성향을 알려주는 전문가, 한솔이에요.",
        "당신의 금융 성향이 궁금하다면, 저를 찾아오세요!"
    }); // ID 2000 대화 데이터
    }

    // 대화 데이터를 가져오는 메서드
    public string GetTalk(int id, int talkIndex)
    {
        Debug.Log("talkData.ContainsKey(id): " + talkData.ContainsKey(id));
        Debug.Log("talkIndex: " + talkIndex);
        Debug.Log("talkData[id].Length: " + talkData[id].Length);
        // 대화가 끝났을 경우 null 반환
        if (talkData.ContainsKey(id) && talkIndex == talkData[id].Length)
        {
            return null;
        }
        // 유효한 대화 데이터 반환
        else if (talkData.ContainsKey(id) && talkIndex < talkData[id].Length)
        {
            return talkData[id][talkIndex];
        }
        // 유효하지 않은 경우 안내 메시지 반환
        else
        {
            return "TalkManager 스크립트에 대화 데이터를 연결해주세요.";
        }
    }

    // 대화 데이터의 길이를 반환하는 메서드
    public int GetTalkDataLength(int id)
    {
        if (talkData.ContainsKey(id))
        {
            return talkData[id].Length;
        }
        return 0; // 대화 데이터가 없으면 0을 반환
    }

}
