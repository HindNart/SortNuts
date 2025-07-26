using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button quitBtn;

    void Start()
    {
        if (playBtn != null)
        {
            playBtn.onClick.RemoveAllListeners();
            playBtn.onClick.AddListener(OnPlayBtnClick);
        }

        if (quitBtn != null)
        {
            quitBtn.onClick.RemoveAllListeners();
            quitBtn.onClick.AddListener(OnQuitBtnClick);
        }
    }

    private void OnPlayBtnClick()
    {
        SceneManager.LoadScene("Game");
    }

    private void OnQuitBtnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        if (playBtn != null)
        {
            playBtn.onClick.RemoveListener(OnPlayBtnClick);
        }
        if (quitBtn != null)
        {
            quitBtn.onClick.RemoveListener(OnQuitBtnClick);
        }
    }
}