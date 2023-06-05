using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Evolution
{
    public float MakeMoneySpeed;
    public GameObject Background;
    public GameObject UnitUI;
    public GameObject Level;
    public float UpgradeCost;
    public float MaxMoney;
    public GameObject UnitA;
    public GameObject UnitB;
    public GameObject UnitC;
}

public class Player : MonoBehaviour
{
    public System.Action<float> OnUpdateHealthRatio;

    public int _hp;
    [SerializeField] float _money;

    public GameManager _gameManager;
    
    public Transform _unitSpawnPos;
    public Sprite _destroyTower;
    public GameObject _destroyEffeect;
    public List<GameObject> _unitList;
    
    [SerializeField] Evolution[] _evolution;


    public int _count;

    SpriteRenderer _sprite;

    float _unitACost;
    float _unitBCost;
    float _unitCCost;
    float _upgradeCost;

    void Awake()
    {
        OnUpdateHealthRatio += GetComponentInChildren<HpBar>().SetFillAmount;
    }

    void Start()
    {
        _count = 0;
        GetComponent<PlayerObject>().GetHp(_hp);
        _gameManager.GetComponent<GameManager>();
        _sprite = GetComponent<SpriteRenderer>();
        _evolution[0].Level.SetActive(true);
        _unitACost = _evolution[_count].UnitA.GetComponent<PlayerObject>().SetCost();
        _unitBCost = _evolution[_count].UnitB.GetComponent<PlayerObject>().SetCost();
        _unitCCost = _evolution[_count].UnitC.GetComponent<PlayerObject>().SetCost();
        _upgradeCost = _evolution[_count].UpgradeCost;
        _unitList = new List<GameObject>();
        
    }

    void Update()
    {
        MakeMoney();
        _gameManager.Shoppable(_unitACost, _unitBCost, _unitCCost, _upgradeCost, _money, _count, _evolution.Length);
    }

    void LateUpdate()
    {
        _gameManager.MoneyUI(_money, _evolution[_count].MaxMoney);
    }

    public void MakeMoney()
    {
        if(_evolution[_count].MaxMoney <= _money)
            _money = _evolution[_count].MaxMoney;
        _money += _evolution[_count].MakeMoneySpeed * Time.deltaTime;
    }

    public void AddMoney(float money)
    {
        if(_evolution[_count].MaxMoney <= _money)
            _money = _evolution[_count].MaxMoney;
        _money += money;
    }

    public float SetMoney()
    {
        return _money;
    }

    public void TakeDamage(int damage)
    {
        _hp -= damage;
    }

    public void Upgrade()
    {
        if(_count != _evolution.Length)
        {
            _money -= _upgradeCost;
            _count++;
            _upgradeCost = _evolution[_count].UpgradeCost;
            _evolution[_count].Background.SetActive(true);
            _evolution[_count-1].Background.SetActive(false);
            _evolution[_count].UnitUI.SetActive(true);
            _evolution[_count-1].UnitUI.SetActive(false);
            _evolution[_count].Level.SetActive(true);
            _evolution[_count-1].Level.SetActive(false);

            _gameManager.TowerUpgrade(_count);
        }

        _unitACost = _evolution[_count].UnitA.GetComponent<PlayerObject>().SetCost();
        _unitBCost = _evolution[_count].UnitB.GetComponent<PlayerObject>().SetCost();
        _unitCCost = _evolution[_count].UnitC.GetComponent<PlayerObject>().SetCost();
    }

    public void BuyUnitA()
    {
        GameObject unitAPrefab;
        GameObject unitInstance;
        float unitACost;

        unitAPrefab = _evolution[_count].UnitA;
        unitACost = _evolution[_count].UnitA.GetComponent<PlayerObject>().SetCost();

        _money -= unitACost;
        unitInstance = Instantiate(unitAPrefab);
        unitInstance.transform.position = _unitSpawnPos.position;

        _unitList.Add(unitInstance);
        
    }

    public void BuyUnitB()
    {
        GameObject unitBPrefab;
        GameObject unitInstance;
        float unitBCost;

        unitBPrefab = _evolution[_count].UnitB;
        unitBCost = _evolution[_count].UnitB.GetComponent<PlayerObject>().SetCost();
      
        _money -= unitBCost;
        unitInstance = Instantiate(unitBPrefab);
        unitInstance.transform.position = _unitSpawnPos.position;

        _unitList.Add(unitInstance);
        
    }

    public void BuyUnitC()
    {
        GameObject unitCPrefab;
        GameObject unitInstance;
        float unitCCost;

        unitCPrefab = _evolution[_count].UnitC;
        unitCCost = _evolution[_count].UnitC.GetComponent<PlayerObject>().SetCost();
        
         _money -= unitCCost;
        unitInstance = Instantiate(unitCPrefab);
        unitInstance.transform.position = _unitSpawnPos.position;

        _unitList.Add(unitInstance);
    }

    public void DestroyTower()
    {
        _sprite.sprite = _destroyTower;
        _destroyEffeect.SetActive(true);
        _gameManager.GameOver("Lose");
    }

    public void StopGame()
    {
        enabled = false;
    }

    public void ResumeGame()
    {
        enabled = true;
    }
}