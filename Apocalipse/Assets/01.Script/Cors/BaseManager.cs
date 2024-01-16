/*
 *여러 클래스에서 지속적으로 사용할 변수와  가상 함수를 미리 선언하고 있다 
 * 
 * 
 * 
 * 
 * 
 * 
 */




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    protected GameManager  _gameManager;

    public GameManager GameManager { get { return _gameManager; } }
    public virtual void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
