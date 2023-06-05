using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] int _damage;
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _attackArea;

    public BulletType _bulletType;

    public SoundManager _soundManager;
    public AudioClip _attackSound;

    Transform _scanEnemy;

    bool _isMove;
    bool _isHit;

    void Start()
    {
        _isMove = true;
        _isHit = false;
        _soundManager = GameObject.FindWithTag("SoundManager").GetComponentInChildren<SoundManager>();
    }

    void FixedUpdate() 
    {
        Attack();
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        if(_isMove)
        {
            float move = _bulletSpeed * Time.deltaTime;
            transform.Translate(move, 0, 0);
        }
        
    }

    public void Attack()
    {
        if(_isHit == false)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, _attackArea, LayerMask.GetMask("Enemy"));
            if(hit.transform != null)
            {
                _scanEnemy = hit.transform;
                _isMove = false;
                _isHit = true;
                _scanEnemy.gameObject.GetComponent<EnemyObject>().TakeDamage(_damage);
                
                if(_bulletType == BulletType.Magic)
                    Invoke("Disappear", 3f);
                else
                {
                    _soundManager.GetComponent<SoundManager>().PlaySound(_attackSound);
                    Disappear();
                }
            }     
        }
    }

    public void Disappear()
    {
        Destroy(this.gameObject);
    }

    public enum BulletType
    {
        Arrow,
        Magic,
        Fireball
    }
}