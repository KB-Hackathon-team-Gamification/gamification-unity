using UnityEngine;

public class Back3 : MonoBehaviour
{
    public GameObject back3;      // 첫 번째 오브젝트
    public GameObject back3_1;    // 두 번째 오브젝트
    public float interval = 1f;   // 깜박이는 간격 (1초)

    private bool isBack3Active = true; // 현재 활성화된 오브젝트 상태

    void Start()
    {
        // 지정된 간격으로 ToggleObjects 메서드를 반복 실행
        InvokeRepeating(nameof(ToggleObjects), 0f, interval);
    }

    void ToggleObjects()
    {
        // 현재 상태를 반전시킴
        isBack3Active = !isBack3Active;

        // 두 오브젝트의 활성화 상태를 전환
        back3.SetActive(isBack3Active);
        back3_1.SetActive(!isBack3Active);
    }
}