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
        talkData.Add(1000, new string[] { "안녕?", "이 곳에 처음 왔구나?" });  // ID 1000에 대한 대화 데이터 추가
        talkData.Add(2000, new string[] { "여어", "이 호수는 정말 아름답지?", "사실 이 호수에는 무언가의 비밀이 숨겨져 있다고 해" });  // ID 1000에 대한 대화 데이터 추가
        talkData.Add(3000, new string[] { "너가 한솔이니?" });  // ID 1000에 대한 대화 데이터 추가
        talkData.Add(4000, new string[] { "너가 민준이니?" });  // ID 1000에 대한 대화 데이터 추가
        talkData.Add(5000, new string[] { "너가 서원이니?" });  // ID 1000에 대한 대화 데이터 추가
        talkData.Add(6000, new string[] { "너가 호진이니?" });  // ID 1000에 대한 대화 데이터 추가
        
        // 사물
        talkData.Add(100, new string[] { "평범한 나무상자다." });  // ID 100에 대한 대화 데이터 추가
        talkData.Add(200, new string[] { "누군가 사용했던 흔적이 있는 책상이다." });  // ID 200에 대한 대화 데이터 추가

        // B23 대화창 캐릭터 반영 보류 (스프라이트 부족) 
        //portraitData.Add(1000 + 0,);
        //portraitData.Add(1000 + 1,);
        //portraitData.Add(1000 + 2,);
        //portraitData.Add(1000 + 3,);
    }

    // 대화 데이터를 가져오는 메서드
    public string GetTalk(int id, int talkIndex)
    {
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
}
