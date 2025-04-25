using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private string loadingSceneName = "Loading";
    [SerializeField] private float minLoadTime = 1f;
    [SerializeField] private string progressBarName = "ProgressBar";
    [SerializeField] private string progressTextName = "ProgressText";

    private Image _progressBar;
    private TextMeshProUGUI _progressText;
    private GameObject _loadingScreenRoot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName) => StartCoroutine(LoadSceneRoutine(sceneName));

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // Загружаем сцену загрузки (режим Single выгружает предыдущую сцену)
        yield return SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Single);

        // Кешируем корневой объект и компоненты
        CacheLoadingScreenComponents();

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
        loadOperation.allowSceneActivation = false;

        float timer = 0f;

        while (!loadOperation.isDone)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);

            UpdateProgressUI(progress);

            if (loadOperation.progress >= 0.9f && timer >= minLoadTime)
                loadOperation.allowSceneActivation = true;

            yield return null;
        }
    }

    private void CacheLoadingScreenComponents()
    {
        // Ищем корневой объект по специальному имени
        _loadingScreenRoot = GameObject.Find("LoadingScreenUI");

        if (_loadingScreenRoot != null)
        {
            // Поиск через Transform.Find() - оптимальнее чем GameObject.Find()
            Transform progressBarTr = _loadingScreenRoot.transform.Find(progressBarName);
            Transform progressTextTr = _loadingScreenRoot.transform.Find(progressTextName);

            _progressBar = progressBarTr?.GetComponent<Image>();
            _progressText = progressTextTr?.GetComponent<TextMeshProUGUI>();
        }
    }

    private void UpdateProgressUI(float progress)
    {
        if (_progressBar != null)
            _progressBar.fillAmount = progress;

        if (_progressText != null)
            _progressText.text = $"{progress * 100:F0}%";
    }
}