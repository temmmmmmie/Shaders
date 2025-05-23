using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playermove : MonoBehaviour
{
    // 이동 속도 변수
    public float moveSpeed = 7f;
    // 캐릭터 콘트롤러 변수
    CharacterController cc;
    // 중력 변수
    float gravity = -20f;
    // 수직 속력 변수
    public float yVelocity = 0;
    // 점프력 변수
    public float jumpPower = 10f;
    // 점프 상태 변수
    public bool isJumping = false;

    public Vector3 dir;
    // 플레이어 체력 변수
    public int hp = 20;

    // 최대 체력 변수
    int maxHp = 20;
    // hp 슬라이더 변수
    public Slider hpSlider;

    // Hit 효과 오브젝트
    public GameObject hitEffect;
    // 애니메이터 변수
    Animator anim;
    private void Start()
    {
        // 캐릭터 콘트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();
        // 애니메이터 받아오기
        anim = GetComponentInChildren<Animator>();

    }
    void Update()
    {
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        // 키보드 [W], [A], [S], [D] 버튼을 입력하면 캐릭터를 그 방향으로 이동시키고 싶다
        // 1. 사용자의 입력을 받는다.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 2. 이동 방향을 설정한다.
        dir = new Vector3(h, 0, v);
        dir = dir.normalized;
        // 이동 블랜딩 트리를 호출하고 벡터의 크기 값을 넘겨준다.
        if(dir.magnitude > 0.5f)
        {
            anim.SetTrigger("Run");
        }
        // 2-1. 메인 카메라를 기준으로 방향을 변환한다.
        dir = Camera.main.transform.TransformDirection(dir);
        // 2-2. 캐릭터 수직 속도에 중력 값을 적용한다.
        yVelocity += gravity * Time.deltaTime;
        // 2-2. 만일, 점프 중이었고, 다시 바닥에 착지했다면...
        if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            // 점프 전 상태로 초기화한다.
            isJumping = false;
            // 캐릭터 수직 속도를 0으로 만든다.
            yVelocity = 0;
        }
        // 2-3. 만일, 키보드 [Spacebar] 키를 입력했고, 점프를 하지 않은 상태라면...
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            // 캐릭터 수직 속도에 점프력을 적용하고 점프 상태로 변경한다.
            yVelocity = jumpPower;
            isJumping = true;
        }
        dir.y = yVelocity;
        // 3. 이동 속도에 맞춰 이동한다.
        cc.Move(dir * moveSpeed * Time.deltaTime);
        // 4. 현재 플레이어 hp(%)를 hp 슬라이더의 value에 반영한다.
        hpSlider.value = (float)hp / (float)maxHp;


    }
    // 플레이어의 피격 함수
    public void DamageAction(int damage)
    {
        // 에너미의 공격력만큼 플레이어의 체력을 깎는다.
        hp -= damage;
        // 만일, 플레이어의 체력이 0보다 크면 피격 효과를 출력한다.
        if (hp > 0)
        {
            // 피격 이펙트 코루틴을 시작한다.
            StartCoroutine(PlayHitEffect());
        }

    }

    // 피격 효과 코루틴 함수
    IEnumerator PlayHitEffect()
    {
        // 1. 피격 UI를 활성화한다.
        hitEffect.SetActive(true);
        // 2. 0.3초간 대기한다.
        yield return new WaitForSeconds(0.3f);
        // 3. 피격 UI를 비활성화한다.
        hitEffect.SetActive(false);
    }


}
