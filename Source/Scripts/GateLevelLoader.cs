using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GateLevelLoader : MonoBehaviour
{
    [SerializeField] private int loadLevelIndex;
    [SerializeField] private Sprite openedDoorSprite;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            ObjectContainer.Instance.player.SetControllStatus(false);
            SoundController.Instance.PlayOneShot(ObjectContainer.Instance.doorOpenSound);
            GetComponent<Light2D>().enabled = true;
            ObjectContainer.Instance.screenFader.gameObject.SetActive(true);
            GetComponent<SpriteRenderer>().sprite = openedDoorSprite;
            ObjectContainer.Instance.screenFader.DOFade(1f, 3f).onComplete += () =>
            {
                SceneManager.LoadScene(loadLevelIndex);
            };
        }
    }

    public void loadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
}