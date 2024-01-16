/*
 해당 클래스는 CharacterManager에서 Player 를 위해 2번 참조되었으며 PlayerCharacter에서 부모클래스로 참조되어 총 3번 참조가 되었으며, CharacterManager => _characterManager는 해당 클래스를 참조하는 
 다른 클래스들의 공통으로 사용되는 변수를 미리 선언하여 다른 클래스에서 계속해서 재선언하지 않아도 되도록 되어있다.
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.TextCore.Text;

public class BaseCharacter : MonoBehaviour
{
    private CharacterManager _characterManager;
    public CharacterManager CharacterManager => _characterManager;
    /*
      public CharacterManager CharacterManager
      {
        get { return _characterManager; }
      }
     */

    //public으로 선언하여 외부에서도 접근할 수 있도록 하였으며 virtual 가상 함수로 선언되어 해당 클래스를 상속 받은 다른 클래스에서도 Init를 재정의하여 다르게 사용할 수 있는 Init 함수를 작성하였다.
    //characterManager를 입력 받으며 입력된 charactherManager를 _characterManager에 저장한다. 
    public virtual void Init(CharacterManager characterManager)
    {
        _characterManager = characterManager;//플레이어가 여럿일 수 있기에//상호참조
    }
}