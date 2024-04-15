using UnityEngine;
using UnityEngine.SceneManagement;

public class TimedLoad : MonoBehaviour
{
    [SerializeField] private int loadLevelIndex;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            SceneManager.LoadScene(loadLevelIndex);
        }
    }
}