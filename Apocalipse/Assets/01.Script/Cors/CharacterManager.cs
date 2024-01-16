using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.TextCore.Text;

public class CharacterManager : BaseManager
{
    [SerializeField]
    private BaseCharacter _player;
    public BaseCharacter Player => _player;

    public override void Init(GameManager gameManager)
    {
        base.Init(gameManager);
        _player.Init(this);// map,sound, Item, Character은 base를 상속 받을 것.이유는 GameManager를 상속할 것이고 GameManager에 접근하기 위해.상호 참조/
    }
}