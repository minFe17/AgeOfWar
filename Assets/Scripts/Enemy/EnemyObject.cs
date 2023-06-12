using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MonoBehaviour
{
    [SerializeField] float _money;

    public GameObject _unit;
    public Sprite _tower;
    public Sprite _hitTower;

    int _curHp;

    Player _player;
    Enemy _enemy;
    public ObjectType _objectType;

    SpriteRenderer _sprite;

    void Start()
    {
        _unit = this.gameObject;
        _player = GameObject.FindWithTag("PlayerTower").GetComponent<Player>();
        _enemy = GameObject.FindWithTag("EnemyTower").GetComponent<Enemy>();
    }

    public void TakeDamage(int damage)
    {
        _curHp -= damage;

        if(_curHp <= 0)
        {
            _curHp = 0;

            if(_objectType == ObjectType.Tower)
            {
                GetComponent<Enemy>().DestroyTower();
            }
            else
            {
                _player.AddMoney(_money);
                _enemy._unitList.Remove(this.gameObject);
                if(_objectType == ObjectType.ProximityUnit)
                    GetComponent<EnemyProximityUnit>().Die();
                else
                    GetComponent<EnemyRangeUnit>().Die();   
            }
        }

        else if(_objectType == ObjectType.Tower)
        {
            _enemy.OnUpdateHealthRatio?.Invoke(_curHp / (float)_enemy._hp);
            _sprite = GetComponent<SpriteRenderer>();
            StartCoroutine(TowerHitRoutine(_sprite));
        }
    }

    public void GetHp(int hp)
    {
        _curHp = hp;
    }

    public void StopGame()
    {
        if(_objectType == ObjectType.ProximityUnit)
        {
            _unit.GetComponent<EnemyProximityUnit>().enabled = false;
            _unit.GetComponentInChildren<Animator>().enabled = false;
            enabled = false;
        }
        else if(_objectType == ObjectType.RangeUnit)
        {
            _unit.GetComponent<EnemyRangeUnit>().enabled = false;
            _unit.GetComponentInChildren<Animator>().enabled = false;
            enabled = false;
        }
        else if(_objectType == ObjectType.Bullet)
        {
            _unit.GetComponent<EnemyBullet>().enabled = false;
            enabled = false;
        }

        _enemy.StopGame();

    }

    public void ResumeGame()
    {
        if(_objectType == ObjectType.ProximityUnit)
        {
            _unit.GetComponent<EnemyProximityUnit>().enabled = true;
            _unit.GetComponentInChildren<Animator>().enabled = true;
            enabled = true;
        }
        else if(_objectType == ObjectType.RangeUnit)
        {
            _unit.GetComponent<EnemyRangeUnit>().enabled = true;
            _unit.GetComponentInChildren<Animator>().enabled = true;
            enabled = true;
        }
        else if(_objectType == ObjectType.Bullet)
        {
            _unit.GetComponent<EnemyBullet>().enabled = true;
            enabled = true;
        }
    }

    public enum ObjectType
    {
        Tower,
        ProximityUnit,
        RangeUnit,
        Bullet
    }

    IEnumerator TowerHitRoutine(SpriteRenderer sprite)
    {
        sprite.sprite = _hitTower;
        yield return new WaitForSeconds(0.1f);
        sprite.sprite = _tower;
    }
}