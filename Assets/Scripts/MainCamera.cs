using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] float _area;

    float _moveSpeed;
    bool _isMove;

    void Start()
    {
        
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        _moveSpeed = x * 2f;
        float move = _moveSpeed * Time.deltaTime;

        if(transform.position.x > -_area && _moveSpeed < 0)
            _isMove = true;
        else if(transform.position.x < _area && _moveSpeed > 0)
            _isMove = true;
        else if(transform.position.x <= -_area && _moveSpeed < 0)
            _isMove = false;  
        else if(transform.position.x >= _area && _moveSpeed > 0)
            _isMove = false;   
        
        if(_isMove)
        {
            transform.Translate(move, 0, 0);
        }
    }

    public float ReturnMoveSpeed()
    {
        if(_isMove)
            return _moveSpeed;
        else 
            return 0;
    }
}
