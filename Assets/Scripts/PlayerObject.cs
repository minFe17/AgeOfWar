using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerObject : MonoBehaviour
{
    [SerializeField] float _cost;
    
    public GameObject _unit;
    public Sprite _tower;
    public Sprite _hitTower;

    Player _player;

    int _curHp;

    public ObjectType _objectType;

    SpriteRenderer _sprite;

    public void Start()
    {
        _unit = this.gameObject;
        _player = GameObject.FindWithTag("PlayerTower").GetComponent<Player>();
    }

    public void TakeDamage(int damage)
    {
        _curHp -= damage;
        if(_curHp <= 0)
        {
            _curHp = 0;

            if(_objectType == ObjectType.Tower)
                GetComponent<Player>().DestroyTower();
            else if(_objectType == ObjectType.ProximityUnit)
                GetComponent<PlayerProximityUnit>().Die();
            else if(_objectType == ObjectType.RangeUnit)
                GetComponent<PlayerRangeUnit>().Die();
        }
        else if(_objectType == ObjectType.Tower)
        {
            _player.OnUpdateHealthRatio?.Invoke(_curHp / (float)_player._hp);
            _sprite = GetComponent<SpriteRenderer>();
            StartCoroutine(TowerHitRoutine(_sprite));
        }
        
    }

    public void GetHp(int hp)
    {
        _curHp = hp;
    }

    public float SetCost()
    {
        return _cost;
    }


    public void StopGame()
    {
        if(_objectType == ObjectType.ProximityUnit)
        {
            _unit.GetComponent<PlayerProximityUnit>().enabled = false;
            _unit.GetComponentInChildren<Animator>().enabled = false;
            enabled = false;
        }
        else if(_objectType == ObjectType.RangeUnit)
        {
            _unit.GetComponent<PlayerRangeUnit>().enabled = false;
            _unit.GetComponentInChildren<Animator>().enabled = false;
            enabled = false;
        }
        else if(_objectType == ObjectType.Bullet)
        {
            _unit.GetComponent<PlayerBullet>().enabled = false;
            enabled = false;
        }
    }

    public void ResumeGame()
    {
        if(_objectType == ObjectType.ProximityUnit)
        {
            _unit.GetComponent<PlayerProximityUnit>().enabled = true;
            _unit.GetComponentInChildren<Animator>().enabled = true;
            enabled = true;
        }
        else if(_objectType == ObjectType.RangeUnit)
        {
            _unit.GetComponent<PlayerRangeUnit>().enabled = true;
            _unit.GetComponentInChildren<Animator>().enabled = true;
            enabled = true;
        }
        else if(_objectType == ObjectType.Bullet)
        {
            _unit.GetComponent<PlayerBullet>().enabled = true;
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