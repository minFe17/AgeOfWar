using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [SerializeField] GameObject _titleUi;
    [SerializeField] GameObject _levelUi;
    [SerializeField] GameObject _optionUi;

    [SerializeField] Button _easyButton;
    [SerializeField] Button _normalButton;
    [SerializeField] Button _hardButton;


    void Start()
    {
        ShowTitleUi();
        
        // _easyButton.onClick.AddListener(() => { SceneManager.LoadScene(); });
        _normalButton.onClick.AddListener(() => { SceneManager.LoadScene("NormalGameScene"); });
        // _hardButton.onClick.AddListener(() => { SceneManager.LoadScene();});
        
    }

    // void Update()
    // {
        
    // }

    public void ShowTitleUi()
    {
        _titleUi.SetActive(true);
        _levelUi.SetActive(false);
        _optionUi.SetActive(false);
    }

    public void ShowLevelUi()
    {
        _levelUi.SetActive(true);
        _titleUi.SetActive(false);
    }

    public void ShowOptionUi()
    {
        _optionUi.SetActive(true);
        _titleUi.SetActive(false);
    }
}
