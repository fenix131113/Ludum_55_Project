using UnityEngine;
using UnityEngine.UI;

public class ObjectContainer : MonoBehaviour
{
    public static ObjectContainer Instance;

    public Image screenFader;
    public PlayerController player;
    public AudioClip doorOpenSound;

    private void Awake()
    {
        if(!Instance)
            Instance = this;
    }
}
