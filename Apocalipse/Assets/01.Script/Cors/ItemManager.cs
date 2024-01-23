using EnumTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//직렬화 하고 있다.
public class Item//클래스 선언, Item이라는 공개 클래스를 선언하고 있다.
{
    public EnumTypes.ItemName Name;//EnumTypes에 ItemName이라는 열거형 변수를 선언하였다.
    public GameObject Prefab;//게임 오브젝트 Prefab를 선언하였다.
}

public class BaseItem : MonoBehaviour
{
    protected void Update()
    {
        transform.Translate(new Vector3(0, -0.005f, 0f));//프레임마다 반복적으로 -0.005만큼 transform.Translate를 사용하여 내려가고 있다.
    }

    public virtual void OnGetItem(CharacterManager characterManager) { }//?
}

public class ItemManager : MonoBehaviour
{
    public List<Item> Items = new List<Item>();//item list를 선언, 동적할당
    public void SpawnItem(EnumTypes.ItemName name, Vector3 position)
    {
        Item foundItem = Items.Find(item => item.Name == name);//열거형 변수를 통하여 Item 명 정의, prefab 받기

        if (foundItem != null)
        {
            GameObject itemPrefab = foundItem.Prefab;//foundItem에 정의한 내용를 itemPrefab에 다시 한번 정의하고 있다.
            GameObject inst = Instantiate(itemPrefab, position, Quaternion.identity);//위 함수들에 실행 결과에 따라 생성될 prefab,위치에 PreFab 막고 있다.
            inst.GetComponent<Animator>().SetInteger("ItemIndex", (int)name);
        }
    }

    public void SpawnRandomItem(int min, int max, Vector3 position)
    {
        if (Random.Range(0, 3) == 0)//0~2 사이에 랜덤한 값 하나를 정하고 그 값이 0일 경우 다음의 함수를 실행
        {
            SpawnItem(EnumTypes.ItemName.Refuel, position);//
            return;
        }

        if (Random.Range(min, max) == min)//min과 max 값 사이에서 랜덤한 값을 min과 비교하여 같을 겨우 아래를 실행
        {
            int randomInt = Random.Range(0, 4);//0~3 사이에서 랜덤한 값 하나를 뽑아 randomInt 룰 정의
            EnumTypes.ItemName itemName = (EnumTypes.ItemName)randomInt;// 열거형 변수 모음에서 해당 값에 열거형 변수를 명을 가지고 오고 있다.
            SpawnItem(itemName, position);// 가지고 온 열거셩 변수를 스폰 하고 있다.
        }
    }

    public void SpawnRandomItem(Vector3 position)
    {
        int randomInt = Random.Range(0, 4);//램
        EnumTypes.ItemName itemName = (EnumTypes.ItemName)randomInt;
        SpawnItem(itemName, position);
    }
}