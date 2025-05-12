using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    //필요속성 : 이동속도
    public float speed = 5;
    // 방향을 전역변수로 만들어서 Start와 Update에서 사용
    Vector3 dir;
    public GameObject explosionFactory;

    void Start()
    {

        // 0부터 9(10-1) 까지 값중에 하나를 랜덤으로 가져와서
        int randValue = UnityEngine.Random.Range(0, 10);
        // 만약 3보다 작으면 플레이어방향
        if (randValue < 3)
        {
            // 플레이어를 찾아서 target으로 하고싶다.
            GameObject target = GameObject.Find("Player");
            // 방향을 구하고싶다. target - me
            dir = target.transform.position - transform.position;
            // 방향의 크기를 1로 하고 싶다.
            dir.Normalize();
        }
        // 그렇지 않으면 아래방향으로 정하고 싶다.
        else
        {
            dir = Vector3.down;
        }
    }

    void Update()
    {
        // 1. 방향을 구한다.
        // 2. 이동하고 싶다. 공식 P = P0 + vt
        transform.position += dir * speed * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision other)
    {
        // 에너미를 잡을 때마다 현재 점수 표시하고 싶다.
        Scoreman.Instance.SetScore(Scoreman.Instance.GetScore() + 1);

        GameObject explosion = Instantiate(explosionFactory);
        explosion.transform.position = transform.position;
        //1.만약 부딪힌 물체가 Bullet 이라면
        if (other.gameObject.name.Contains("Bullet"))
        {
            //2.부딪힌 물체를 비활성화
            other.gameObject.SetActive(false);
        }
        //3.그렇지 않으면 제거
        else
        {
            Destroy(other.gameObject);
        }
        //Destroy(gameObject);
        gameObject.SetActive(false);




    }
}
