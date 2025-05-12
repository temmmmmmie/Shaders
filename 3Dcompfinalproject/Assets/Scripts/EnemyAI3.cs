using System.Collections;
using UnityEngine;

public class EnemyAI3 : MonoBehaviour
{
    public enum State //상태를 담아두는 열거형
    {
        PATROL,
        TRACE,
        ATTACK,
        STOP,
        RUN
    }

    public State state = State.PATROL; //초기값은 순찰
    public float attackDist = 5; //ATTACK 상태로 변경되는 범위
    public float traceDist = 10; //TRACE 상태로 변경되는 범위
    public bool stop; //정지
    public GameObject player; //플레이어
    public float dist; //플레이어와의 거리

    Transform playerTr; //플레이어 트랜스폼
    Transform enemyTr; //교수님 트랜스폼
    WaitForSeconds ws; //대기

    MoveAgent3 moveAgent; //무브에이젼트 스크립트
    public static bool gameover; //게임오버인가?

    private void Awake()
    {
        if (player != null) //플레이어가 존재한다면
            playerTr = player.transform; //플레이어 트랜스폼 지정

        enemyTr = GetComponent<Transform>(); //교수님 트랜스폼 지정
        ws = new WaitForSeconds(0.3f); //대기 지정

        moveAgent = GetComponent<MoveAgent3>(); //무브 에이전트 지정
        gameover = false; //게임오버 초기화
    }

    private void Update()
    {
        if(dist < 2f && !PlayerCtrl2.instance.godmode) //거리가 완전 가깝고 무적모드가 아니라면
        {
            gameover = true; //게임오버
            state = State.ATTACK; //상태를 정지상태로
        }
        if(PlayerCtrl2.instance.final) state = State.RUN; //마지막 스테이지라면 상태를 추격으로
        if(stop) state = State.STOP; //정지가 트루라면 상태를 정지로
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState()); //상태지정코루틴 실행
        StartCoroutine(Action()); //액션 코루틴 실행
    }

    IEnumerator CheckState() //상태지정 코루틴
    {
        while(!stop) //멈춤 상태가 아닐동안
        {
            if (gameover || PlayerCtrl2.instance.final || stop) //게임오버, 멈춤상태, 마지막스테이지라면
                yield break; //While문 정지
            dist = Vector3.Distance(playerTr.position, enemyTr.position); //교수님과 플레이어간 거리 계산
            if (dist < attackDist) //거리가 공격범위안에 있다면
                state = State.ATTACK; //상태는 공격
            else if (dist < traceDist) //거리가 추격범위안에 있다면
                state = State.TRACE; //상태는 추격
            else //아예 멀리 떨어져 있다면 
                state = State.PATROL; //상태는 순찰

            yield return ws; //대기
        }
    }
    public void actionon() //액션 코루틴 실행함수
    {
        StartCoroutine(Action());
    }
    IEnumerator Action() //액션 코루틴
    {

        while(!stop) //멈춤 상태가 아닌동안
        { 
            yield return ws; //대기
            switch (state) //상태가 어떤가에 따라
            {
                case State.PATROL: //순찰 상태라면
                    moveAgent.SetPatrolling(true); //순찰함수 실행
                    break;

                case State.TRACE: //추격 상태라면
                    moveAgent.SetTraceTarget(playerTr.position); //추격함수 실행
                    break;

                case State.ATTACK: //공격 상태라면
                    moveAgent.Stop(); //정지
                    break;

                case State.STOP: //정지 상태라면
                    moveAgent.Stop(); //정지
                    break;
                case State.RUN: //달리기 상태라면
                    moveAgent.traceSpeed = 5; //속도 최대로
                    moveAgent.SetTraceTarget(playerTr.position); //추격
                    break;
            }
        }
    }
}

