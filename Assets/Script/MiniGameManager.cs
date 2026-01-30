using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;

    public bool isPlaying;

    void Awake()
    {
        Instance = this;
    }

    public void StartMiniGame()
    {
        Debug.Log("MiniGame START");
        isPlaying = true;
    }

    public void Win()
    {
        Debug.Log("MiniGame WIN");
        isPlaying = false;
    }

    public void Fail()
    {
        Debug.Log("MiniGame FAIL");
        isPlaying = false;
    }
}
