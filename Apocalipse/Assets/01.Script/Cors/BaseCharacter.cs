/*
 �ش� Ŭ������ CharacterManager���� Player �� ���� 2�� �����Ǿ����� PlayerCharacter���� �θ�Ŭ������ �����Ǿ� �� 3�� ������ �Ǿ�����, CharacterManager => _characterManager�� �ش� Ŭ������ �����ϴ� 
 �ٸ� Ŭ�������� �������� ���Ǵ� ������ �̸� �����Ͽ� �ٸ� Ŭ�������� ����ؼ� �缱������ �ʾƵ� �ǵ��� �Ǿ��ִ�.
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

    //public���� �����Ͽ� �ܺο����� ������ �� �ֵ��� �Ͽ����� virtual ���� �Լ��� ����Ǿ� �ش� Ŭ������ ��� ���� �ٸ� Ŭ���������� Init�� �������Ͽ� �ٸ��� ����� �� �ִ� Init �Լ��� �ۼ��Ͽ���.
    //characterManager�� �Է� ������ �Էµ� charactherManager�� _characterManager�� �����Ѵ�. 
    public virtual void Init(CharacterManager characterManager)
    {
        _characterManager = characterManager;//�÷��̾ ������ �� �ֱ⿡//��ȣ����
    }
}