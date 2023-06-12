using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyEvolution
{
    public float EvolutionInterval;
    public GameObject Level;
    public GameObject UnitA;
    public GameObject UnitB;
    public GameObject UnitC;
}

public class Enemy : MonoBehaviour
{
    public System.Action<float> OnUpdateHealthRatio;

    public int _hp;
    [SerializeField] float _unitSpawnIntervalMin;
    [SerializeField] float _unitSpawnIntervalMax;

    public GameManager _gameManager;
    public SoundManager _soundManager;

    public AudioClip _upgradeSound;
    public AudioClip _buySound;

    public Transform _unitSpawnPos;
    public Sprite _destroyTower;
    public GameObject _destroyEffeect;
    public List<GameObject> _unitList;


    [SerializeField] EnemyEvolution[] _evolution;

    SpriteRenderer _sprite;

    public UpgradeBar _upgradeBar;

    bool _isDestroy;
    int _count;
    float _curTime;

    void Awake()
    {
        OnUpdateHealthRatio += GetComponentInChildren<HpBar>().SetFillAmount;
    }   

    void Start()
    {
        _isDestroy = false;
        _count = 0;
        GetComponent<EnemyObject>().GetHp(_hp);
        _evolution[_count].Level.SetActive(true);
        _gameManager.GetComponent<GameManager>();
        _sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(EnemyUnitSpawnRoutine());
    }

    void Update()
    {
        _curTime += Time.deltaTime;
    }

    void LateUpdate()
    {
        Upgrade();
    }

    public void UnitSpawn(GameObject unitPrefab)
    {
        GameObject unitInstance;
        unitInstance = Instantiate(unitPrefab);
        unitInstance.transform.position = _unitSpawnPos.position;

        _soundManager.GetComponent<SoundManager>().PlaySound(_buySound);

        _unitList.Add(unitInstance);
    }

    public void Upgrade()
    {
        if(_count + 1 == _evolution.Length)
        {
            _upgradeBar.GetComponent<UpgradeBar>().SetFillAmount(1);
        }
        else if(_curTime < _evolution[_count].EvolutionInterval && _count + 1 < _evolution.Length)
        {
            if(_curTime == 0)
            {
                _upgradeBar.GetComponent<UpgradeBar>().SetFillAmount(0);
            }
            _upgradeBar.GetComponent<UpgradeBar>().SetFillAmount(_curTime / _evolution[_count].EvolutionInterval);
        }
        else
        {
            _curTime = 0;
            _count++;
            _evolution[_count].Level.SetActive(true);
            _evolution[_count-1].Level.SetActive(false);
            _soundManager.GetComponent<SoundManager>().PlaySound(_upgradeSound);
        }

    }

    public void DestroyTower()
    {
        _isDestroy = true;
        _sprite.sprite = _destroyTower;
        _destroyEffeect.SetActive(true);
        _gameManager.GameOver("Win");
    }

    public void StopGame()
    {
        StopAllCoroutines();
        enabled = false;
        Debug.Log(1);
    }

    public void ResumeGame()
    {
        StartCoroutine(EnemyUnitSpawnRoutine());
        enabled = true;
    }

    IEnumerator EnemyUnitSpawnRoutine()
    {
        GameObject unitPrefab;

        while(!_isDestroy)
        {
            float time = Random.Range(_unitSpawnIntervalMin, _unitSpawnIntervalMax);
            yield return new WaitForSeconds(time);

            int randomSpawn = Random.Range(0, 7);
            if(randomSpawn >= 0 && randomSpawn <= 3)
            {
                unitPrefab = _evolution[_count].UnitA;
                UnitSpawn(unitPrefab);
            }
            else if(randomSpawn <= 5)
            {
                unitPrefab = _evolution[_count].UnitB;
                UnitSpawn(unitPrefab);
            }
            else
            {
                unitPrefab = _evolution[_count].UnitC;
                UnitSpawn(unitPrefab);
            }
        }
    }
}