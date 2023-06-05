using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] float _speed;
    public GameObject _mainCamera;
    
    void Start()
    {
        
    }

    void LateUpdate()
    {
        float cameraMoveSpeed = _mainCamera.GetComponent<MainCamera>().ReturnMoveSpeed();
        if(cameraMoveSpeed!=0)
        {
            float move = _speed * cameraMoveSpeed * Time.deltaTime;
            transform.Translate(move, 0, 0);     
        }
    }
        
}
