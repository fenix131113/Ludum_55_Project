using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
    [SerializeField] private float animationTime;
    [SerializeField] private float escMenuUpperY;
    [SerializeField] private float volumePanelMenuUpperY;
    [SerializeField] private float volumePanelMenuDownY;
    [SerializeField] private Image escMenuBlocker;
    [SerializeField] private GameObject interactionBlocker;
    [SerializeField] private RectTransform escMenuRect;
    [SerializeField] private RectTransform volumeSliderMenuRect;
    [SerializeField] private PlayerController player;
    [SerializeField] private Slider volumeSlider;

    private bool isMenuOpened;
    private bool isOpenProcess;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (isMenuOpened && !isOpenProcess)
                CloseMenu();
            else if (!isOpenProcess)
                OpenMenu();
    }
    public void OpenMenu()
    {
        player.SetControllStatus(false);
        isMenuOpened = true;
        isOpenProcess = true;
        interactionBlocker.SetActive(true);
        escMenuBlocker.gameObject.SetActive(true);
        escMenuBlocker.DOFade(0.6f, animationTime).onComplete += () =>
        {
            interactionBlocker.SetActive(false);
            isOpenProcess = false;
        };
        escMenuRect.DOLocalMoveY(0, animationTime);
        volumeSliderMenuRect.DOLocalMoveY(volumePanelMenuUpperY, animationTime);
    }

    public void CloseMenu()
    {
        isMenuOpened = false;
        isOpenProcess = true;
        interactionBlocker.SetActive(true);
        escMenuBlocker.gameObject.SetActive(true);
        escMenuBlocker.DOFade(0f, animationTime).onComplete += () =>
        {
            escMenuBlocker.gameObject.SetActive(false);
            interactionBlocker.SetActive(false);
            isOpenProcess = false;
            player.SetControllStatus(true);
        };
        escMenuRect.DOLocalMoveY(escMenuUpperY, animationTime);
        volumeSliderMenuRect.DOLocalMoveY(volumePanelMenuDownY, animationTime);
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void OnVolumeSliderChanged()
    {
        AudioListener.volume = volumeSlider.value;
    }
}