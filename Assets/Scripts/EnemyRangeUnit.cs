using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyRangeUnit : MonoBehaviour
{
    [SerializeField] int _hp;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _attackSpeed;
    [SerializeField] float _attackArea;
    [SerializeField] GameObject _bulletPrefab;

    public Transform _bulletPos; 
    public SortingGroup _sort;
    public Animator _animator;
    public UnitType _unitType;

    public SoundManager _soundManager;
    public AudioClip _attackSound;

    Enemy _enemy;
    Transform _scanPlayer;
    Transform[] _scanPlayers;
    CapsuleCollider2D _collider;

    bool _isMove;
    bool _isDie;
    bool _isAttack;

    void Start()
    {
        _isMove = true;
        _isDie = false;
        _isAttack = false;
        _collider = GetComponent<CapsuleCollider2D>();
        _sort = GetComponentInChildren<SortingGroup>();
        GetComponent<EnemyObject>().GetHp(_hp);
        _enemy = GameObject.FindWithTag("EnemyTower").GetComponent<Enemy>();
        _soundManager = GameObject.FindWithTag("SoundManager").GetComponentInChildren<SoundManager>();
    }

    void Update()
    {
        Move();
    }

    void FixedUpdate()
    {
        if(_unitType == UnitType.Priest)
            PriestUnitScan();
        else
            Scan();
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

    public void Scan()
    {
        if(!_isDie && !_isAttack)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, _attackArea, LayerMask.GetMask("Player"));
            if(hit.transform != null)
            {
                _scanPlayer = hit.transform;
                _isMove = false;
                _animator.SetBool("isMove", false);
                if(!_isAttack && !_isDie)
                {   
                    StartCoroutine(AttackRoutine());
                }
            }
            if(hit.transform == null)
            {
                _isMove = true;
            }
        }
        
    }

    public void PriestUnitScan()
    {
        if(!_isDie)
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.left, _attackArea, LayerMask.GetMask("Player"));
            int length = hit.Length;
            _scanPlayers = new Transform[length];
            for(int i=0; i<hit.Length; i++)
            {
                if(hit[i].transform != null)
                {
                    _scanPlayers[i] = hit[i].transform;
                    _isMove = false;
                    _animator.SetBool("isMove", false);
                    if(!_isAttack && !_isDie)
                    {   
                        StartCoroutine(PriestAttackRoutine()); 
                    }
                }  
            }
            if(length == 0)
                _isMove = true;
        }
    }

    public void Die()
    {
        _animator.SetTrigger("doDie");
        StopCoroutine(AttackRoutine());
        // _scanEnemy = null;
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

    IEnumerator AttackRoutine()
    {
        _isAttack = true;
        GameObject bulletInstance;
        _animator.SetTrigger("doAttack");
        yield return new WaitForSeconds(0.3f);
        _soundManager.GetComponent<SoundManager>().PlaySound(_attackSound);

        bulletInstance = Instantiate(_bulletPrefab);
        bulletInstance.transform.position = _bulletPos.position;

        float time = Random.Range(0.5f, 1.5f);
        yield return new WaitForSeconds(time + _attackSpeed);   

        _isAttack = false;
    }

    IEnumerator PriestAttackRoutine()
    {
        if(_scanPlayers != null)
        {
            _isAttack = true;
            GameObject bulletInstance;

            _animator.SetTrigger("doAttack");
            yield return new WaitForSeconds(0.5f);
            _soundManager.GetComponent<SoundManager>().PlaySound(_attackSound);

            
            for(int i=0; i<_scanPlayers.Length; i++)
            {
                bulletInstance = Instantiate(_bulletPrefab, _scanPlayers[i]);
            }

            float time = Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(time + _attackSpeed);

            _isAttack = false;
        }  
    }

    public enum UnitType
    {
        Archer,
        Wizard,
        Priest,
    }
}

    