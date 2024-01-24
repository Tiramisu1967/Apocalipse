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
    private const double InvincibilityDurationInSeconds = 3; // 무적 지속 시간 (초)
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
        base.Init(characterManager);// map,sound, Item, Character은 base를 상속 받을 것.이유는 GameManager를 상속할 것이고 GameManager에 접근하기 위해.상호 참조/
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

        // 카메라의 좌측 하단은(0, 0, 0.0)이며, 우측 상단은(1.0 , 1.0)이다.
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        if (pos.y < 0f) pos.y = 0f;
        if (pos.y > 1f) pos.y = 1f;
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    /*if문을 사용하여 z키를 입력 받아 입력된 변수에 해당하는 클래스에 Activate 함수를 호출하는 사용자 정의 함수 ActivateSkill 함수에 primary 열거형 변수를 입력하고 있으며
    이와 같이 if문을 사용하여 x키와 c키를 입력 받아 ActivateSkill 함수에  각각 Repir, Bomb 열거형 변수를 입력하고 있다.*/
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
    Skills라는 Dictionary를 생성하고, EnumTypes.PlayerSkill을 키로 하여 BaseSkill을 값으로 하는 새로운 Dictionary를 할당한다.
    for문을 따라 반복하면서 AddSkill 메서드를 호출하고 스킬을 Skills Dictionary에 추가한다. 
    또한 CurrentWeaponLevel을 GameInstance.instance.CurrentPlayerWeaponLevel으로 한다
    */

    private void InitializeSkills()
    {
        Skills = new Dictionary<EnumTypes.PlayerSkill, BaseSkill>();//선언만 하였기에 채워주는 것. 동적할당에 해당

        for (int i = 0; i < _skillPrefabs.Length; i++)
        {
            AddSkill((EnumTypes.PlayerSkill)i, _skillPrefabs[i]);
        }

        CurrentWeaponLevel = GameInstance.instance.CurrentPlayerWeaponLevel;
        Debug.Log(CurrentWeaponLevel);
    }


    /*
     InitializeSkills 함수의 for문을 통하여 반복적으로 호출되고 있으며 skillobject라는 오브젝트를 생성하고 있다. 
    또한 if문을 통하여 skillobject가 null 값이 아닐 경우 BaseSkill에 Init 함수에 CharacterManager을 전달하여 호출하고 Skills의 BaseSkill 컴포넌트를 추가하고 있다.
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
     UpdateSkillInput 함수에서 키를 입력받아 skil에 대한 열겨형 변수를 전달 받고 있으며, 각 열거형 변수의 해당하는 PlayerSkill로 사용되는 클래스들에 Activate를 호출하고 있다. 
    이때 Activate는 각 Skill를 담당하는 클래스에 부모 클래스인 BaseSkill에서 Virtual 가상 함수로 선언되어 BaseSkill에서 skill에 cooldowntime을 체크하고 있다.
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
            if (invincibilityCoroutine != null)// null일 경우 멈춤
            {
                StopCoroutine(invincibilityCoroutine);
            }

            invincibilityCoroutine = StartCoroutine(InvincibilityCoroutine());// 해당 함수를 실행
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        Invincibility = true;// Invincibility의 ture로 변경
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();//sprite 참조

        // 무적 지속 시간 (초)
        float invincibilityDuration = 3f;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);

        // 무적이 해제될 때까지 대기
        yield return new WaitForSeconds(invincibilityDuration);

        // 타이머가 만료되면 무적을 비활성화
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