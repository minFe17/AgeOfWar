using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class ButtonUi
{
    public Button _upgradeButton;
    public Button _unitAButton;
    public Button _unitBButton;
    public Button _unitCButton;

    public Image _upgradeImage;
    public Image _unitAImage;
    public Image _unitBImage;
    public Image _unitCImage;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] float _spawnUnitInterval;

    public Player _player;
    public Enemy _enemy;
    public GameObject _optionUi;
    public SoundManager _soundManager;
    
    public float _playTime;
    public TextMeshProUGUI _playTimeText;

    public string _level;
    public TextMeshProUGUI _levelText;
    public TextMeshProUGUI _moneyText;

    public GameObject _victoryImage;
    public GameObject _defeatImage;
    public GameObject _resetButton;

    public AudioSource _bgmAudio;
    public AudioClip _victroySound;
    public AudioClip _defeatSound;
    public AudioClip _buySound;
    public AudioClip _upgradeSound;

    [SerializeField] ButtonUi[] _buttonUi;


    float _curSpawnUnitATime;
    float _curSpawnUnitBTime;
    float _curSpawnUnitCTime;


    void Start()
    {
        LevelUI();
        _playTimeText.text = "00:00:00";
        _moneyText.text = "000 / 000";

        _player.GetComponent<Player>();
        _enemy.GetComponent<Enemy>();

        StartBGM();

        _buttonUi[0]._upgradeButton.onClick.AddListener(() =>
        {
            _player.Upgrade();
            _soundManager.GetComponent<SoundManager>().PlaySound(_upgradeSound);
        });

        _buttonUi[0]._unitAButton.onClick.AddListener(() => 
        { 
            _player.BuyUnitA();
            _soundManager.GetComponent<SoundManager>().PlaySound(_buySound);
            _curSpawnUnitATime = 0;
        });

        _buttonUi[0]._unitBButton.onClick.AddListener(() => 
        { 
            _player.BuyUnitB();
            _soundManager.GetComponent<SoundManager>().PlaySound(_buySound);
            _curSpawnUnitBTime = 0;
        });

        _buttonUi[0]._unitCButton.onClick.AddListener(() => 
        { 
            _player.BuyUnitC();
            _soundManager.GetComponent<SoundManager>().PlaySound(_buySound);
            _curSpawnUnitCTime = 0;
        });
    }

    void Update()
    {
        GameTime();
    }

    void LateUpdate()
    {
        TimeUI();
    }

    public void GameTime()
    {
        _playTime += Time.deltaTime;
        _curSpawnUnitATime += Time.deltaTime;
        _curSpawnUnitBTime += Time.deltaTime;
        _curSpawnUnitCTime += Time.deltaTime;

        if(_curSpawnUnitATime >= _spawnUnitInterval)
        {
            _curSpawnUnitATime = _spawnUnitInterval;
        }

        if(_curSpawnUnitBTime >= _spawnUnitInterval)
        {
            _curSpawnUnitBTime = _spawnUnitInterval;
        }

        if(_curSpawnUnitCTime >= _spawnUnitInterval)
        {
            _curSpawnUnitCTime = _spawnUnitInterval;
        }
    }

    public void TowerUpgrade(int count)
    {
        _buttonUi[count]._upgradeButton.onClick.AddListener(() =>
        {
            _player.Upgrade();
            _soundManager.GetComponent<SoundManager>().PlaySound(_upgradeSound);
        });

        _buttonUi[count]._unitAButton.onClick.AddListener(() => 
        { 
            _player.BuyUnitA();
            _soundManager.GetComponent<SoundManager>().PlaySound(_buySound);
            _curSpawnUnitATime = 0;
        });

        _buttonUi[count]._unitBButton.onClick.AddListener(() => 
        { 
            _player.BuyUnitB();
            _soundManager.GetComponent<SoundManager>().PlaySound(_buySound);
            _curSpawnUnitBTime = 0;
        });

        _buttonUi[count]._unitCButton.onClick.AddListener(() => 
        { 
            _player.BuyUnitC();
            _soundManager.GetComponent<SoundManager>().PlaySound(_buySound);
            _curSpawnUnitCTime = 0;
        });
    }

    //_playTime UI에 보여주기
    public void TimeUI()
    {
        int hour = (int)(_playTime / 3600);
        int min = (int)((_playTime - hour *3600) / 60);
        int second = (int)(_playTime % 60);
        _playTimeText.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);
    }

    //Player 돈 보여주기
    public void MoneyUI(float curMoney, float maxMoney)
    {
        _moneyText.text = string.Format("{0:000}", (int)curMoney) + " / " + string.Format("{0:000}", maxMoney);
    }

    //게임난이도 보여주기
    public void LevelUI()
    {
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "EasyGameScene")
        {
            _level = "Easy";
            _levelText.text = _level;
        }
        else if(scene.name == "NormalGameScene")
        {
            _level = "Normal";
            _levelText.text = _level;
        }
        else if(scene.name == "HardGameScene")
        {
            _level = "Hard";
            _levelText.text = _level;
        }
        else
            _levelText.text = "???";
    }

    public void Shoppable(float unitACost, float unitBCost, float unitCCost, float upgradeCost, float money, int count, int evolutionLength)
    {
        Player player = GetComponent<Player>();

        if(unitACost > money || _curSpawnUnitATime < _spawnUnitInterval)
        {
            _buttonUi[count]._unitAButton.GetComponent<Button>().interactable = false;
            _buttonUi[count]._unitAImage.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f, 0.5f);
        }
        else
        {
            _buttonUi[count]._unitAButton.GetComponent<Button>().interactable = true;
            _buttonUi[count]._unitAImage.GetComponent<Image>().color = Color.white;
        }

        if(unitBCost > money || _curSpawnUnitBTime < _spawnUnitInterval)
        {
            _buttonUi[count]._unitBButton.GetComponent<Button>().interactable = false;
            _buttonUi[count]._unitBImage.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f, 0.5f);
        }
        else
        {
            _buttonUi[count]._unitBButton.GetComponent<Button>().interactable = true;
            _buttonUi[count]._unitBImage.GetComponent<Image>().color = Color.white;
        }

        if(unitCCost > money || _curSpawnUnitCTime < _spawnUnitInterval)
        {
            _buttonUi[count]._unitCButton.GetComponent<Button>().interactable = false;
            _buttonUi[count]._unitCImage.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f, 0.5f);
        }
        else
        {
            _buttonUi[count]._unitCButton.GetComponent<Button>().interactable = true;
            _buttonUi[count]._unitCImage.GetComponent<Image>().color = Color.white;
        }

        if(upgradeCost > money || count+1 == evolutionLength)
        {
            _buttonUi[count]._upgradeButton.GetComponent<Button>().interactable = false;
            _buttonUi[count]._upgradeImage.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f, 0.5f);
        }
        else if(upgradeCost <= money && count+1 != evolutionLength)
        {
            _buttonUi[count]._upgradeButton.GetComponent<Button>().interactable = true;
            _buttonUi[count]._upgradeImage.GetComponent<Image>().color = Color.white;
        }

    }

    public void StartBGM()
    {
        _bgmAudio.GetComponent<AudioSource>().Play();
    }

    public void StopBGM()
    {
        _bgmAudio.GetComponent<AudioSource>().Stop();
    }

    public void StopButton()
    {
        StopGame();
        _optionUi.SetActive(true);
    }

    public void StopGame()
    {
        int playerUnitListLength = _player._unitList.Count;
        for(int i=0; i<playerUnitListLength; i++)
        {
            _player._unitList[i].GetComponent<PlayerObject>().StopGame();
        }

        int enemyUnitListLength = _enemy._unitList.Count;
        for(int i=0; i<enemyUnitListLength; i++)
        {
            _enemy._unitList[i].GetComponent<EnemyObject>().StopGame();
        }

        _player.StopGame();
        _enemy.StopGame();
        StopBGM();
        enabled = false;
    }

    public void ResumeGame()
    {
        _optionUi.SetActive(false);

        int playerUnitListLength = _player._unitList.Count;
        for(int i=0; i<playerUnitListLength; i++)
        {
            _player._unitList[i].GetComponent<PlayerObject>().ResumeGame();
        }

        int enemyUnitListLength = _enemy._unitList.Count;
        for(int i=0; i<enemyUnitListLength; i++)
        {
            _enemy._unitList[i].GetComponent<EnemyObject>().ResumeGame();
        }

        _player.ResumeGame();
        _enemy.ResumeGame();
        StartBGM();
        enabled = true;
    }

    public void GameOver(string gameResult)
    {
        StopBGM();
        StopGame();
        _resetButton.SetActive(true);

        if(gameResult == "Win")
        {
            _victoryImage.SetActive(true);
            _soundManager.GetComponent<SoundManager>().PlaySound(_victroySound);
        }
        else
        {
            _defeatImage.SetActive(true);
            _soundManager.GetComponent<SoundManager>().PlaySound(_defeatSound);
        }
          
    }

    public void BackMainMenu()
    {
        SceneManager.LoadScene("TitleScene");
    }
}