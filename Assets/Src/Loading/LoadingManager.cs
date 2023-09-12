using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;
    public static int sceneIndex;

    public Slider loadingBar;
    public TextMeshProUGUI loadingText;
    public TextMeshProUGUI tipsText;
    public CanvasGroup alphaCanvas;
    public string[] tips;
    public GameObject networkErrorGO;
    public Button retryConnection;

    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    private float totalSceneProgress;
    private int tipCount;

    private void Awake()
    {
        instance = this;
        StartCoroutine(InternetConnectionManager.checkInternetConnection(networkErrorGO, retryConnection));
    }

    public async void loadScene()
    {
        loadingBar.value = 0;
        StartCoroutine(generateTips());

        await Task.Delay(1000); //wait some time for more realistic experience
        scenesLoading.Add(SceneManager.LoadSceneAsync(sceneIndex));

        if (sceneIndex > 2)
        {
            StartCoroutine(getSceneLoadProgress(MenuManager.levelToLoad));
        }
        else
        {
            StartCoroutine(getSceneLoadProgress());
        }
    }

    private IEnumerator getSceneLoadProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;

                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;
                loadingBar.value = Mathf.RoundToInt(totalSceneProgress);
                loadingText.text = "Loading Main Menu " + totalSceneProgress.ToString("F0") + "%";

                yield return null;
            }
        }
    }

    private IEnumerator getSceneLoadProgress(int levelNum)
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;

                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;
                loadingBar.value = Mathf.RoundToInt(totalSceneProgress);
                loadingText.text = "Loading Level " + levelNum.ToString() + " " + totalSceneProgress.ToString("F0") + "%";

                yield return null;
            }
        }
    }

    private IEnumerator generateTips()
    {
        tipCount = Random.Range(0, tips.Length);
        tipsText.text = tips[tipCount];

        while (loadingBar.value < 0.9f) //idk why but unity has loaded scene on 90% :O
        {
            yield return new WaitForSeconds(3f);

            if(!alphaCanvas.IsDestroyed())
            {
                LeanTween.alphaCanvas(alphaCanvas, 0, 0.5f);
            }

            yield return new WaitForSeconds(0.5f);

            tipCount++;
            if (tipCount >= tips.Length)
            {
                tipCount = 0;
            }

            tipsText.text = tips[tipCount];

            if (!alphaCanvas.IsDestroyed())
            {
                LeanTween.alphaCanvas(alphaCanvas, 1, 0.5f);
            }
        }
    }
}
