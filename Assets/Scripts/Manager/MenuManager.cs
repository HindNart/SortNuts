using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button quitBtn;

    void Start()
    {
        if (playBtn != null)
        {
            playBtn.onClick.RemoveAllListeners();
            playBtn.onClick.AddListener(OnPlayBtnClick);
        }

        if (continueBtn != null)
        {
            if (!PlayerPrefs.HasKey("CURRENT_LEVEL"))
            {
                continueBtn.interactable = false;
            }
            else continueBtn.interactable = true;

            continueBtn.onClick.RemoveAllListeners();
            continueBtn.onClick.AddListener(OnContinueBtnClick);
        }

        if (quitBtn != null)
        {
            quitBtn.onClick.RemoveAllListeners();
            quitBtn.onClick.AddListener(OnQuitBtnClick);
        }
    }

    private void OnPlayBtnClick()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Game");
    }

    private void OnContinueBtnClick()
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
        if (continueBtn != null)
        {
            continueBtn.onClick.RemoveListener(OnContinueBtnClick);
        }
        if (quitBtn != null)
        {
            quitBtn.onClick.RemoveListener(OnQuitBtnClick);
        }
    }
}