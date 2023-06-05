using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyProximityUnit : MonoBehaviour
{
    [SerializeField] int _hp;
    [SerializeField] int _damage;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _attackSpeed;
    [SerializeField] float _attackArea;

    public Animator _animator;
    public SortingGroup _sort;
    public SoundManager _soundManager;
    public AudioClip _attackSound;

    Enemy _enemy;
    CapsuleCollider2D _collider;
    Transform[] _scanPlayer;

    bool _isMove;
    bool _isDie;
    bool _isAttack;

    void Start()
    {
        _isMove = true;
        _isDie = false; 
        _collider = GetComponent<CapsuleCollider2D>();
        _sort = GetComponentInChildren<SortingGroup>();
        GetComponent<EnemyObject>().GetHp(_hp);
        _enemy = GameObject.FindWithTag("EnemyTower").GetComponent<Enemy>();
        _soundManager = GameObject.FindWithTag("SoundManager").GetComponentInChildren<SoundManager>();
    }

    void FixedUpdate()
    {
        Scan();
    }

    void Update()
    {
        Move(); 
    }

    public void Move()
    {   
        if(_isMove && !_isDie)
        {
            float move = _moveSpeed * Time.deltaTime;
            _animator.SetBool("isMove", true);
            transform.Translate(-move, 0, 0);
        }
    }

    public void Die()
    {
        _animator.SetTrigger("doDie");
        StopCoroutine(AttackRoutine());
        // _scanPlayer = null;
        _collider.enabled = false;
        _sort.sortingOrder = 1;
        _isMove = false;
        _isDie = true;
        _enemy._unitList.Remove(this.gameObject);
        Invoke("Disappear", 1f);
    }

    public void Disappear()
    {
        Destroy(this.gameObject);
    }

    public void Scan()
    {
        if(!_isDie)
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.left, _attackArea, LayerMask.GetMask("Player"));
            int length = hit.Length;
            _scanPlayer = new Transform[length];
            for(int i=0; i<hit.Length; i++)
            {
                if(hit[i].transform != null)
                {
                    _isMove = false;
                    _animator.SetBool("isMove", false);
                    _scanPlayer[i] = hit[i].transform;
                    if(!_isAttack && !_isDie)
                    {
                        StartCoroutine(AttackRoutine());
                    }
                }  
            }
            if(length == 0)
                _isMove = true;
        }
    }
    
    IEnumerator AttackRoutine()
    {
        _isAttack = true;
            
        _animator.SetTrigger("doAttack");
        yield return new WaitForSeconds(0.5f);
        _soundManager.GetComponent<SoundManager>().PlaySound(_attackSound);

        for(int i=0; i<_scanPlayer.Length; i++)
        {
            _scanPlayer[i].gameObject.GetComponent<PlayerObject>().TakeDamage(_damage);
        }
        
        float time = Random.Range(0.5f, 1.5f);
        yield return new WaitForSeconds(time + _attackSpeed);  
        _isAttack = false;
    }
}