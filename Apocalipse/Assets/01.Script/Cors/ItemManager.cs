using EnumTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//����ȭ �ϰ� �ִ�.
public class Item//Ŭ���� ����, Item�̶�� ���� Ŭ������ �����ϰ� �ִ�.
{
    public EnumTypes.ItemName Name;//EnumTypes�� ItemName�̶�� ������ ������ �����Ͽ���.
    public GameObject Prefab;//���� ������Ʈ Prefab�� �����Ͽ���.
}

public class BaseItem : MonoBehaviour
{
    protected void Update()
    {
        transform.Translate(new Vector3(0, -0.005f, 0f));//�����Ӹ��� �ݺ������� -0.005��ŭ transform.Translate�� ����Ͽ� �������� �ִ�.
    }

    public virtual void OnGetItem(CharacterManager characterManager) { }//?
}

public class ItemManager : MonoBehaviour
{
    public List<Item> Items = new List<Item>();//item list�� ����, �����Ҵ�
    public void SpawnItem(EnumTypes.ItemName name, Vector3 position)
    {
        Item foundItem = Items.Find(item => item.Name == name);//������ ������ ���Ͽ� Item �� ����, prefab �ޱ�

        if (foundItem != null)
        {
            GameObject itemPrefab = foundItem.Prefab;//foundItem�� ������ ���븦 itemPrefab�� �ٽ� �ѹ� �����ϰ� �ִ�.
            GameObject inst = Instantiate(itemPrefab, position, Quaternion.identity);//�� �Լ��鿡 ���� ����� ���� ������ prefab,��ġ�� PreFab ���� �ִ�.
            inst.GetComponent<Animator>().SetInteger("ItemIndex", (int)name);
        }
    }

    public void SpawnRandomItem(int min, int max, Vector3 position)
    {
        if (Random.Range(0, 3) == 0)//0~2 ���̿� ������ �� �ϳ��� ���ϰ� �� ���� 0�� ��� ������ �Լ��� ����
        {
            SpawnItem(EnumTypes.ItemName.Refuel, position);//
            return;
        }

        if (Random.Range(min, max) == min)//min�� max �� ���̿��� ������ ���� min�� ���Ͽ� ���� �ܿ� �Ʒ��� ����
        {
            int randomInt = Random.Range(0, 4);//0~3 ���̿��� ������ �� �ϳ��� �̾� randomInt �� ����
            EnumTypes.ItemName itemName = (EnumTypes.ItemName)randomInt;// ������ ���� �������� �ش� ���� ������ ������ ���� ������ ���� �ִ�.
            SpawnItem(itemName, position);// ������ �� ���ż� ������ ���� �ϰ� �ִ�.
        }
    }

    public void SpawnRandomItem(Vector3 position)
    {
        int randomInt = Random.Range(0, 4);//��
        EnumTypes.ItemName itemName = (EnumTypes.ItemName)randomInt;
        SpawnItem(itemName, position);
    }
}