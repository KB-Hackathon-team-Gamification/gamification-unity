using UnityEngine;
using TMPro;

public class StartText : MonoBehaviour
{
    public float blinkInterval = 1f; // 깜박이는 간격 (1초)
    private bool isTextVisible = true; // 텍스트의 현재 상태
    private TextMeshProUGUI textMesh;
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        // 지정된 간격으로 BlinkText 메서드를 반복 실행
        InvokeRepeating(nameof(BlinkText), 0f, blinkInterval);
    }

    void BlinkText()
    {
        // 텍스트 활성화 상태를 반전시킴
        isTextVisible = !isTextVisible;
        gameObject.SetActive(isTextVisible);
    }
}
