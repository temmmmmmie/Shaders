using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemymanager : MonoBehaviour
{
    public int poolSize = 10;
    //오브젝트풀 배열
    public GameObject[] enemyObjectPool;
    //SpawnPoint 들
    public Transform[] spawnPoints;
    // 최소시간
    float minTime = 1;
    // 최대시간
    float maxTime = 5;
    // 현재시간
    float currentTime;
    // 일정시간
    public float creatTime = 1;
    // 적 공장
    public GameObject enemyFactory;

    void Start()
    {
        // 태어날 때 적 생성시간을 설정하고
        creatTime = UnityEngine.Random.Range(minTime, maxTime);
        //2. 오브젝트풀을 에너미들을 담을 수 있는 크기로 만들어 준다.
        enemyObjectPool = new GameObject[poolSize];
        //3. 오브젝트풀에 넣을 에너미 개수 만큼 반복하여
        for (int i = 0; i < poolSize; i++)
        {
            //4. 에너미공장에서 에너미를 생성한다.
            GameObject enemy = Instantiate(enemyFactory);
            //5. 에너미를 오브젝트풀에 넣고싶다.
            enemyObjectPool[i] = enemy;
            // 비활성화 시키자.
            enemy.SetActive(false);
        }
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        //1.생성 시간이 되었으니까
        if (currentTime > creatTime)
        {
            //2.에너미풀 안에 있는 에너미들 중에서
            for (int i = 0; i < poolSize; i++)
            {
                //3.비활성화 된 에너미를
                // - 만약 에너미가 비활성화 되었다면
                GameObject enemy = enemyObjectPool[i];
                if (enemy.activeSelf == false)
                {
                    //4.에너미를 활성화 하고 싶다.
                    enemy.SetActive(true);
                    // 랜덤으로 인덱스 선택
                    int index = Random.Range(0, spawnPoints.Length);
                    // 에너미 위치 시키기
                    enemy.transform.position = spawnPoints[index].position;

                }
            }
            creatTime = Random.Range(1.0f, 5.0f);
            currentTime = 0;
        }
    }

}
