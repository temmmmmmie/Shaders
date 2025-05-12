using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent3 : MonoBehaviour
{
    public List<Transform> wayPoints; //웨이포인트들을 담아두는 리스트
    public int nextIdx; //다음 웨이포인트

    public float patrolSpeed = 1.5f; //순찰속도
    public float traceSpeed = 2.5f; //추격속도

    NavMeshAgent agent; //네브 매쉬 에이전트

    bool patrolling; //순찰하는 중인가?
    Vector3 traceTarget; //목표의 위치값

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //네브 매쉬 에이전트 설정
        agent.speed = patrolSpeed; //속도를 순찰속도로 초기화
        wayPoints.RemoveAt(0); //웨이포인트 초기화
        nextIdx = Random.Range(0, wayPoints.Count); //다음 순찰지점을 랜덤으로 지정

        SetPatrolling(true); //순찰
    }
    public void SetPatrolling(bool patrol) //순찰함수
    {
        patrolling = patrol; //순찰하는 중
        agent.speed = patrolSpeed; //이동속도를 순찰속도로 설정
        agent.angularSpeed = 120; //이동속도를 순찰속도로 설정

        MoveWayPoint(); //다음 웨이포인트로 이동
    }

    public void SetTraceTarget(Vector3 pos) //추격 함수
    {
        traceTarget = pos; //추격 대상 설정
        agent.speed = traceSpeed; //속도를 추격속도로
        agent.angularSpeed = 360; //속도를 추격속도로

        TraceTarget(traceTarget); //추격대상 설정
    }

    void MoveWayPoint() //웨이포인트 이동 함수
    {
        if (agent.isPathStale) //이미 이동중이라면
            return; //리턴

        agent.destination = wayPoints[nextIdx].position; //목적지를 다음 웨이포인트로
        agent.isStopped = false; //움직이는중
    }

    void TraceTarget(Vector3 pos) //추격함수
    {
        if (agent.isPathStale) //이미 이동중이라면
            return; //리턴

        agent.destination = pos; //목적지를 추격대상으로
        agent.isStopped = false; //움직이는중
    }

    public void Stop() //정지함수
    {
        agent.isStopped = true; //정지
        agent.velocity = Vector3.zero; //정지
        patrolling = false; //순찰하고 있지 않음
    }

    // Update is called once per frame
    void Update()
    {
        if (!patrolling) //순찰중이 아니라면
            return; //리턴

        if (agent.velocity.magnitude > 0.2f && //웨이포인트에 어느정도 가까워졌다면
            agent.remainingDistance < 0.5f)
        {
            nextIdx = Random.Range(0, wayPoints.Count); //다음 인덱스 지정
            MoveWayPoint(); //이동
        }
    }
}
