using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] int _damage;
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _attackArea;

    public BulletType _bulletType;

    public SoundManager _soundManager;
    public AudioClip _attackSound;

    Transform _scanPlayer;

    bool _isMove;
    bool _isHit;

    void Start()
    {
        _soundManager = GameObject.FindWithTag("SoundManager").GetComponentInChildren<SoundManager>();
    }

    void OnEnable()
    {
        _isMove = true;
        _isHit = false;
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
            transform.Translate(-move, 0, 0);
        }
        
    }

    public void Attack()
    {
        if(_isHit == false)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, _attackArea, LayerMask.GetMask("Player"));
            if(hit.transform != null)
            {
                _scanPlayer = hit.transform;
                _isMove = false;
                _isHit = true;
                _scanPlayer.gameObject.GetComponent<PlayerObject>().TakeDamage(_damage);
                
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
