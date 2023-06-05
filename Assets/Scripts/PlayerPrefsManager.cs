using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    static PlayerPrefsManager Instance;

    [SerializeField] string _level;

    void Awake() {
        
        if(Instance != null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        Instance = this;
    }

    public static void SetClearTime(float time)
    {
        switch(Instance._level)
        {
            case "Easy":
                PlayerPrefs.SetFloat("EasyClearTime", time);
                break;
            case "Normal":
                PlayerPrefs.SetFloat("NormalClearTime", time);
                break;
            case "Hard":
                PlayerPrefs.SetFloat("HardClearTime", time);
                break;
            default:
                break;
        }
    }

    public static float GetEasyClearTime()
    {
        return PlayerPrefs.GetFloat("EasyClearTime");
    }

    public static float GetNormalClearTime()
    {
        return PlayerPrefs.GetFloat("NormalClearTime");
    }

    public static float GetHardClearTime()
    {
        return PlayerPrefs.GetFloat("HardClearTime");
    }

    public static void GetLevel(string level)
    {
        Instance._level = level;
    }
}
