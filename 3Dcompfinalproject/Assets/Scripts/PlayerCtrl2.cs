using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCtrl2 : MonoBehaviour
{
    public static PlayerCtrl2 instance; //싱글톤 패턴
    [Header("Raycasts")]
    public bool isground; //플레이어가 땅에 있는지 확인하는 변수
    public Vector3 boxSize; //레이캐스트할 박스의 크기
    public float maxDistance; //레이캐스트할 박스의 높이
    public LayerMask groundLayer; //레이캐스트로 감지할 레이어
    [Space(10)]
    public float itemmaxdis; //아이템 레이캐스트의 범위
    public LayerMask itemLayer; //아이템 레이캐스트로 감지할 레이어
    [Space(20)]
    public GameObject professor; //교수님 오브젝트
    public GameObject displateff; //화면효과
    public VolumeProfile volumeProfile; //화면효과2
    public TextMeshProUGUI iteminfo; //화면에 뜨는 아이템 이름 텍스트
    public Image Staminaui; //스테미너 바
    public GameObject cam; //카메라
    public Audio sfx; //효과음
    public AudioSource bgm; //배경음악
    public AudioSource gameoveraudio; //게임오버시 재생할 배경음악
    public Inv inventory; //인벤토리 스크립트
    public Image energybar; //배터리 바
    public GameObject gameover; //게임오버인가?
    public Animation fadeoutanim; //페이드아웃, 페이드인 애니매이션
    [Space(20)]
    public float moveSpeed = 10; //이동속도
    public float rotSpeed = 360; //감도
    CharacterController controller; //플레이어 컨트롤러
    Animator animator; //애니매이터
    Light spotlight; //손전등
    bool crawling; //웅크리는 중인가?
    bool running; //달리는 중인가?
    int desindex; //설명 인덱스
    public float Stamina; //스테미너
    public bool eatlunch; //점심을 먹은 상태인가?
    public bool final; //마지막 스태이지인가?
    float ry; //위아래 인풋 값
    public float energy; //배터리
    public int book; //과제의 갯수
    bool turnon; //손전등을 키고 있는가?
    [Space(20)]
    public bool godmode; //무적모드
    // Start is called before the first frame update
    private void Awake()
    {
        PlayerCtrl2.instance = this; //인스턴스 초기화
    }
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>(); //컨트롤러 설정
        animator = gameObject.GetComponentInChildren<Animator>(); //애니매이터 설정
        spotlight = gameObject.GetComponentInChildren<Light>(); //빛 설정
        turnon = true; //손전등은 true가 기본값
        StartCoroutine(energydrain()); //배터리 코루틴 실행
        StartCoroutine(run()); //스테미너 코루틴 실행
    }
    private void FixedUpdate()
    {
        if (isground == false)//땅에 닿아있지 않으면
        {
            controller.SimpleMove(Vector3.down); //밑으로 힘을 준다, 중력
        }
    }
    bool ah; //게임오버시 업데이트에서 계속 실행하는 것을 방지하기 위한 변수
    // Update is called once per frame
    void Update()
    {
        if(book == 7) //책이 7개가 모이면
        {
            StartCoroutine(fadeout2()); //페이드 아웃 밑 여러 효과들 실행
            final = true; //마지막 스태이지다
            book = 0; //책을 다시 0개로 설정해서 업데이트가 계속 실행되는 것을 방지
        }
        if(EnemyAI3.gameover && ah == false) //게임오버시
        {
            gameover.SetActive(true); //게임오버 영상재생
            bgm.Stop(); //게임 배경음악 정지
            gameoveraudio.Play(); //게임오버 배경음악 재생
            StartCoroutine(fadeout()); //화면 페이드 아웃
            ah = true; //업데이트가 계속 실행되는 것을 방지
        }
        else //게임오버가 아니라면
        {
            isground = Physics.BoxCast(transform.position, boxSize, -transform.up, transform.rotation, maxDistance, groundLayer); //땅에 닿는지 확인하는 레이캐스트

            float h = Input.GetAxis("Horizontal"); //AD 입력 감지
            float v = Input.GetAxis("Vertical"); //WS 입력 감지
            float rx = Input.GetAxis("Mouse X"); //마우스 가로 이동 감지
            ry = Input.GetAxis("Mouse Y"); //마우스 세로 입력 감지

            Vector3 moveDir = gameObject.transform.localRotation *(Vector3.forward * v + Vector3.right * h); //입력을 감지하여 갈 방향을 계산
            if (Stamina > 0 && (h != 0 || v != 0) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Inv.isinv == false && crawling == false) //달리기를 판별
            {
                controller.Move(-1 * moveDir.normalized * moveSpeed * 2 * Time.deltaTime); //이동속도에 2배만큼 이동
                animator.SetBool("Run", true); //뛰는 애니매이션 실행
                running = true; //달리는 중을 표시
            }
            else if (Inv.isinv == false) //걷는중
            {
                controller.Move(-1 * moveDir.normalized * moveSpeed * Time.deltaTime); //이동속도만큼 이동
                animator.SetBool("Run", false); //뛰는 애니매이션 정지
                running = false; //걷는 중을 표시
            }
            transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime * rx, 0)); //가로 방향 회전
            cam.transform.Rotate(new Vector3(-1 * ry, 0, 0)); //세로 방향 카메라 회전
            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) //쉬프트 키를 땟을 때
            {
                StartCoroutine(recover()); //스테미나 회복 코루틴 실행
            }

            if (h != 0 || v != 0) //입력이 들어오고 있다면
            {
                animator.SetBool("Walk", true); //걷는 애니매이션 실행
            }
            else //들어오고 있지 않다면
            {
                animator.SetBool("Walk", false); //걷는 애니매이션 정지
                animator.SetBool("Run", false); //뛰는 애니매이션 정지
                running = false; //달리지 않고 있음
            }

            if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) //컨트롤 키 입력감지
            {
                crawling = true; //웅크리고 있음
                gameObject.transform.localScale = new Vector3(0.6227f, 0.1f, 0.6475f); //플레이어의 크기를 줄임
                moveSpeed = 1; //이동속도 느리게 하기
            }
            if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl)) //컨트롤 키 입력감지
            {
                crawling = false; //웅크리고 있지 않음
                gameObject.transform.localScale = new Vector3(0.6227f, 0.6830349f, 0.6475f); //플레이어의 크기를 다시 늘림
                moveSpeed = 3; //이동속도 정상으로 돌리기
            }

            Staminaui.fillAmount = Stamina / 40; //스테미너 표시 바

            if(Input.GetKeyDown(KeyCode.Q)) //Q키 입력감지
            {
                if(spotlight.intensity == 0 && energy > 0) //배터리가 있고 지금 손전등이 꺼져 있다면
                {
                    sfx.play(0); //효과음 재생
                    spotlight.intensity = 1; //손전등 키기
                    turnon = true; //손전등을 키고 있음
                }
                else //손전등이 켜져 있다면
                {
                    sfx.play(0); //효과음 재생
                    spotlight.intensity = 0;  //손전등 끄기  
                    turnon = false; //손전등을 끄고 있음
                }
            }
            energybar.fillAmount = energy / 100; //배터리 바 표시

            RaycastHit hit; //레이캐스트 출력 변수
            var obj = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit ,itemmaxdis, itemLayer); //아이템 레이캐스트로 감지
            Debug.DrawRay(cam.transform.position, cam.transform.forward * itemmaxdis, Color.red, 0); //디버그용
            if(obj) //만약 아이템이 감지되었다면
            {
                var objinfo = hit.collider.gameObject.GetComponent<Item>(); //그 아이템의 Item 스크립트 추출
                if(objinfo.is_item) //만약 아이템이라면
                {
                    iteminfo.text = objinfo.itemname; //설정창을 아이템의 이름으로 설정
                    if (Input.GetMouseButtonDown(0)) //마우스를 누른다면
                    {
                        sfx.play(1); //효과음 재생
                        var isfull = inventory.itemadd(objinfo.itemcode); //아이템을 인벤토리에 저장
                        if(isfull == false) //인벤토리가 꽉 차있지 않다면
                        {
                            Destroy(hit.collider.gameObject); //아이템 오브젝트를 삭제
                        }
                    }
                }
                else if(objinfo.is_interactable) //만약 상호작용할 수 있다면
                {
                    iteminfo.text = objinfo.description[desindex]; //설명텍스트를 사물에 입력해놓은 설명으로 설정
                    if (Input.GetMouseButtonDown(0)) //마우스를 누른다면
                    {
                        if(objinfo.requireitem) //사물이 아이템을 요구한다면
                        {
                            int findidx; //아이템 감지용 변수
                            findidx = Array.FindIndex(inventory.contents, element => element == objinfo.requireitemcode); //그 아이템이 인벤토리에 존재하는지 검사
                            if (objinfo.have_events && findidx != -1) //아이템이 존재하고 이벤트가 있는 사물이라면
                            {
                                objinfo.assignevent.Play(); //사물에 설정해놓은 이벤트 실행
                                sfx.play(2); //효과음 재생
                                iteminfo.text = objinfo.description2; //설명텍스트를 사물에 입력해놓은 설명으로 설정
                                inventory.contents[findidx] = 0; //아이템 삭제
                                inventory.resetinv(); //인벤토리 재정렬
                                objinfo.is_interactable = false; //상호작용이 끝난 사물은 다시 상호작용 불가로 만들기
                            }
                            else //아이템이 존재하지만 이벤트가 없는 사물이라면
                            {
                                if (desindex >= objinfo.description.Length - 1) //설명의 끝에 이르렀다면
                                {
                                    desindex = 0; //설명 인덱스 초기화
                                }
                                else //아직 중간이라면
                                {
                                    desindex++; //다음 설명으로 넘어가기
                                }
                            }
                        }
                        else //사물이 아이템을 요구하지 않는다면(이 게임에는 엔딩이벤트 밖에 없음)
                        {
                            objinfo.assignevent.Play(); //사물에 설정해놓은 이벤트 실행
                            StartCoroutine(end()); //엔딩 코루틴 실행
                        }

                    }
                }
            }
            else //레이캐스트가 아이템을 벗어난다면
            {
                iteminfo.text = ""; //설명 제거
                desindex = 0; //설명 인덱스 초기화
            }

            if(!final) //마지막 스테이지가 아니라면
            {
                float dist = Vector3.Distance(gameObject.transform.position, professor.transform.position); //나와 교수님 사이의 거리 계산
                if (dist < 7) //만약 거리가 7 이하라면
                {
                    displateff.SetActive(true); //화면 효과 재생
                    bgm.volume = 1 - (dist * 0.1f); //배경음악을 교수님과의 거리와 비례하여 출력
                }
                else //만약 거리가 7 이상이라면
                {
                    displateff.SetActive(false); //화면효과 제거
                    bgm.volume = 0; //배경음악 제거
                }
            }
        }
    }
    IEnumerator end() //엔딩코루틴
    {
        godmode = true; //무적모드 활성화
        StartCoroutine(bgmfadeout()); //배경음악 페이드아웃
        sfx.play(3); //효과음 재생
        displateff.SetActive(false); //화면효과 제거
        yield return new WaitForSeconds(1.5f); //대기
        SceneManager.LoadScene("EndScene"); //엔딩 씬으로 전환
    }
    IEnumerator bgmfadeout() //배경음악 페이드아웃 코루틴
    {
        while(bgm.volume > 0) //배경음악의 볼륨이 0 이상인 동안
        {
            bgm.volume -= 0.01f; //배경음악의 볼륨 낮추기
            yield return new WaitForSeconds(0.01f); //대기
        }
    }
    IEnumerator fadeout() //게임오버 코루틴
    {
        yield return new WaitForSeconds(3); //대기
        fadeoutanim.Play(); //화면 페이드아웃
        yield return new WaitForSeconds(0.5f); //대기
        SceneManager.LoadScene("Startscene"); //메인화면 씬으로 이동
    }

    IEnumerator fadeout2() //마지막 스테이지 코루틴
    {
        EnemyAI3 ai = professor.GetComponent<EnemyAI3>(); //ai스크립트 추출
        fadeoutanim.Play(); //화면 페이드아웃
        ai.stop = true; //ai 정지
        professor.GetComponent<NavMeshAgent>().enabled = false; //navmsh 비활성화
        professor.transform.position = new Vector3(68.04f, 0.7f, -47.88772f); //교수님을 마지막 스테이지로 이동
        gameObject.transform.rotation = Quaternion.Euler(0, -540, 0); //플레이어가 앞을 보게 만듬
        professor.GetComponent<NavMeshAgent>().enabled = true; //navmesh 활성화
        displateff.SetActive(true); //화면효과 활성화
        yield return new WaitForSeconds(1); //대기
        displateff.GetComponent<Volume>().profile = volumeProfile; //화면효과 변경
        bgm.volume = 1; //볼륨을 최대로
        gameObject.transform.position = new Vector3(68.04f, 0.7f, -37.88772f); //플레이어 이동
        ai.stop = false; //ai 활성화
        ai.actionon(); //ai가 플레이어를 쫒아오게 만듬
        fadeoutanim.clip = fadeoutanim.GetClip("fadein"); //애니매이션을 화면 페이드인으로
        fadeoutanim.Play(); //화면 페이드인
    }

    IEnumerator run() //스테미너 소모 코루틴
    {
        while(true) //항상 반복
        {
            if(running && eatlunch == false) //달리고 있고 점심을 먹지 않은 상태라면
            {
                Stamina -= 1f; //스테미너 소모
                yield return new WaitForSeconds(0.1f); //대기
            }
            yield return null; //대기
        }
    }
    IEnumerator recover() //스테미너 회복 코루틴
    {
        while (Stamina < 40) //스테미너가 닳아 있다면
        {
            if(running) //달리고 있다면
            {
                break; //회복정지
            }
            Stamina += 1f; //스테미너 회복
            yield return new WaitForSeconds(0.1f); //대기
        }
    }

    IEnumerator energydrain() //배터리 소모 코루틴
    {
        while(true) //항상 반복
        {
            if(turnon && energy > 0) //손전등을 키고 있고 배터리가 있다면
            {
                energy -= 1f; //배터리 소모
                if(energy <= 0) //배터리가 다 닳았다면
                {
                    sfx.play(0); //효과음 재생
                    spotlight.intensity = 0; //손전등을 강제로 끄기
                    turnon = false; //손전등을 끄고 있음
                }
                yield return new WaitForSeconds(2); //대기
            }
            yield return new WaitForSeconds(1); //대기
        }
    }
    private void OnDrawGizmos() //디버깅 함수
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }
}
