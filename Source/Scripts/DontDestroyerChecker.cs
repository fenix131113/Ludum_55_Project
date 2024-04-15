using UnityEngine;

public class DontDestroyerChecker : MonoBehaviour
{
    [SerializeField] private GameObject musicPrefab;
    private void Start()
    {
        DontDestroyerMusic musicDontDestroyer = FindObjectOfType<DontDestroyerMusic>();
        if (!musicDontDestroyer)
            Instantiate(musicPrefab, Vector3.zero, Quaternion.identity);
    }
}