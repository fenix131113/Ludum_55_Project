using UnityEngine;

public class DontDestroyerMusic : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}