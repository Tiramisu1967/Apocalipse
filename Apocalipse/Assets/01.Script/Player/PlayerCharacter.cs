using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;
using UnityEngine.TextCore.Text;
using TMPro;

public class PlayerCharacter : BaseCharacter
{
    public Transform[] AddOnPos;
    public GameObject Add;
    #region Movement
    private Vector2 _moveInput;
    public float MoveSpeed;
    #endregion

    #region Skills
    [HideInInspector] public Dictionary<EnumTypes.PlayerSkill, BaseSkill> Skills;
    [SerializeField] private GameObject[] _skillPrefabs;
    #endregion

    #region Invincibility
    private bool invincibility;
    private Coroutine invincibilityCoroutine;
    private const double InvincibilityDurationInSeconds = 3; // ���� ���� �ð� (��)
    public bool Invincibility
    {
        get { return invincibility; }
        set { invincibility = value; }
    }
    #endregion

    #region Weapon
    public int CurrentWeaponLevel = 0;
    public int MaxWeaponLevel = 3;
    #endregion

    public override void Init(CharacterManager characterManager)
    {
        base.Init(characterManager);// map,sound, Item, Character�� base�� ��� ���� ��.������ GameManager�� ����� ���̰� GameManager�� �����ϱ� ����.��ȣ ����/
        InitializeSkills();
        if (GameInstance.instance.CurrentAddOnCount < 2)
        {
            for (int i = 1; i <= GameInstance.instance.CurrentAddOnCount; i++)
            {
                AddOnItem.SpawnAddOn(AddOnPos[GameInstance.instance.CurrentAddOnCount - 1].transform.position, AddOnItem.Add);
            }
        }
        


    }

    public void DeadProcess()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        UpdateMovement();
        UpdateSkillInput();

    }

    private void UpdateMovement()
    {
        _moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.Translate(new Vector3(_moveInput.x, _moveInput.y, 0f) * (MoveSpeed * Time.deltaTime));

        // ī�޶��� ���� �ϴ���(0, 0, 0.0)�̸�, ���� �����(1.0 , 1.0)�̴�.
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        if (pos.y < 0f) pos.y = 0f;
        if (pos.y > 1f) pos.y = 1f;
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    /*if���� ����Ͽ� zŰ�� �Է� �޾� �Էµ� ������ �ش��ϴ� Ŭ������ Activate �Լ��� ȣ���ϴ� ����� ���� �Լ� ActivateSkill �Լ��� primary ������ ������ �Է��ϰ� ������
    �̿� ���� if���� ����Ͽ� xŰ�� cŰ�� �Է� �޾� ActivateSkill �Լ���  ���� Repir, Bomb ������ ������ �Է��ϰ� �ִ�.*/
    private void UpdateSkillInput()
    {
        if (Input.GetKey(KeyCode.Z)) ActivateSkill(EnumTypes.PlayerSkill.Primary);
        if (Input.GetKeyUp(KeyCode.X)) ActivateSkill(EnumTypes.PlayerSkill.Repair);
        if (Input.GetKeyUp(KeyCode.C)) ActivateSkill(EnumTypes.PlayerSkill.Bomb);
        if (Input.GetKeyUp(KeyCode.V)) ActivateSkill(EnumTypes.PlayerSkill.freeze);
        if (Input.GetKeyUp(KeyCode.B)) ActivateSkill(EnumTypes.PlayerSkill.Protact);
        if (Input.GetKeyUp(KeyCode.N)) ActivateSkill(EnumTypes.PlayerSkill.Recovery);
    }

    /*
    Skills��� Dictionary�� �����ϰ�, EnumTypes.PlayerSkill�� Ű�� �Ͽ� BaseSkill�� ������ �ϴ� ���ο� Dictionary�� �Ҵ��Ѵ�.
    for���� ���� �ݺ��ϸ鼭 AddSkill �޼��带 ȣ���ϰ� ��ų�� Skills Dictionary�� �߰��Ѵ�. 
    ���� CurrentWeaponLevel�� GameInstance.instance.CurrentPlayerWeaponLevel���� �Ѵ�
    */

    private void InitializeSkills()
    {
        Skills = new Dictionary<EnumTypes.PlayerSkill, BaseSkill>();//���� �Ͽ��⿡ ä���ִ� ��. �����Ҵ翡 �ش�

        for (int i = 0; i < _skillPrefabs.Length; i++)
        {
            AddSkill((EnumTypes.PlayerSkill)i, _skillPrefabs[i]);
        }

        CurrentWeaponLevel = GameInstance.instance.CurrentPlayerWeaponLevel;
        Debug.Log(CurrentWeaponLevel);
    }


    /*
     InitializeSkills �Լ��� for���� ���Ͽ� �ݺ������� ȣ��ǰ� ������ skillobject��� ������Ʈ�� �����ϰ� �ִ�. 
    ���� if���� ���Ͽ� skillobject�� null ���� �ƴ� ��� BaseSkill�� Init �Լ��� CharacterManager�� �����Ͽ� ȣ���ϰ� Skills�� BaseSkill ������Ʈ�� �߰��ϰ� �ִ�.
     */
    private void AddSkill(EnumTypes.PlayerSkill skillType, GameObject prefab)
    {
        GameObject skillObject = Instantiate(prefab, transform.position, Quaternion.identity);
        skillObject.transform.parent = this.transform;

        if (skillObject != null)
        {
            BaseSkill skillComponent = skillObject.GetComponent<BaseSkill>();
            skillComponent.Init(CharacterManager);
            Skills.Add(skillType, skillComponent);
        }
    }


    /*
     UpdateSkillInput �Լ����� Ű�� �Է¹޾� skil�� ���� ������ ������ ���� �ް� ������, �� ������ ������ �ش��ϴ� PlayerSkill�� ���Ǵ� Ŭ�����鿡 Activate�� ȣ���ϰ� �ִ�. 
    �̶� Activate�� �� Skill�� ����ϴ� Ŭ������ �θ� Ŭ������ BaseSkill���� Virtual ���� �Լ��� ����Ǿ� BaseSkill���� skill�� cooldowntime�� üũ�ϰ� �ִ�.
     */
    private void ActivateSkill(EnumTypes.PlayerSkill skillType)
    {
        if (Skills.ContainsKey(skillType))
        {
            Debug.Log("!");
            if (Skills[skillType].IsAvailable())
            {
                CurrentWeaponLevel = GameInstance.instance.CurrentPlayerWeaponLevel;
                Skills[skillType].Activate();
                
            }
            //else
            //{
            //    if (skillType != EnumTypes.PlayerSkill.Primary)
            //        GetComponent<PlayerUI>().NoticeSkillCooldown(skillType);
            //}
        }
    }
    public void SetInvincibility(bool invin)
    {
        if (invin)
        {
            if (invincibilityCoroutine != null)// null�� ��� ����
            {
                StopCoroutine(invincibilityCoroutine);
            }

            invincibilityCoroutine = StartCoroutine(InvincibilityCoroutine());// �ش� �Լ��� ����
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        Invincibility = true;// Invincibility�� ture�� ����
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();//sprite ����

        // ���� ���� �ð� (��)
        float invincibilityDuration = 3f;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);

        // ������ ������ ������ ���
        yield return new WaitForSeconds(invincibilityDuration);

        // Ÿ�̸Ӱ� ����Ǹ� ������ ��Ȱ��ȭ
        Invincibility = false;
        spriteRenderer.color = new Color(1, 1, 1, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            collision.gameObject.GetComponent<BaseItem>().OnGetItem(CharacterManager);
            Destroy(collision.gameObject);

        }
    }

}