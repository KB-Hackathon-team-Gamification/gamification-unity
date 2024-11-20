using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartScreen : MonoBehaviour
{
    public Transform sceneTransform;
    public float targetScale = 0.1f;
    public float scaleDuration = 1.0f;
    private Vector3 initialScale;  // 시작 스케일 값

    void Start()
    {
        initialScale = sceneTransform.localScale;
        sceneTransform.localScale = new Vector3(5f, 5f, 5f);
    }

    void Update()
    {
        // 스페이스 바를 눌렀을 때 씬 전환과 스케일 축소
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ScaleDownAndSwitch("SampleSceneHansol"));
        }
    }

    // 씬 전환 시 스케일을 줄여가며 진행하는 코루틴
    private IEnumerator ScaleDownAndSwitch(string sceneName)
    {
        float elapsedTime = 0f;

        // 스케일을 점진적으로 줄여가는 부분
        while (elapsedTime < scaleDuration)
        {
            sceneTransform.localScale = Vector3.Lerp(sceneTransform.localScale, new Vector3(targetScale, targetScale, targetScale), elapsedTime / scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 정확히 목표 스케일로 설정
        sceneTransform.localScale = new Vector3(targetScale, targetScale, targetScale);

        // 씬 전환
        SceneManager.LoadScene(sceneName);
    }
}
