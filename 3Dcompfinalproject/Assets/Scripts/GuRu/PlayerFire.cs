using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    //총알 생산할 공장
    public GameObject bulletFactory;
    //총구
    public GameObject firePosition;
    public int poolSize = 10;
    // 오브젝트풀 배열
    public GameObject[] bulletObjectPool;
    void Start()
    {
        // 2. 탄창을 총알 담을 수 있는 크기로 만들어 준다.
        bulletObjectPool = new GameObject[poolSize];
        // 3. 탄창에 넣을 총알 개수 만큼 반복하여
        for (int i = 0; i < poolSize; i++)
        {
            // 4. 총알공장에서 총알 생성한다.
            GameObject bullet = Instantiate(bulletFactory);
            // 5. 총알을 오브젝트풀에 넣고싶다.
            bulletObjectPool[i] = bullet;
            // 비활성화 시키자.
            bullet.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //1.발사 버튼을 눌렀으니까
        if (Input.GetButtonDown("Fire1"))
        {
            //2.탄창 안에 있는 총알들 중에서
            for (int i = 0; i < poolSize; i++)
            {
                //3.비활성화 된 총알을
                // - 만약 총알이 비활성화 되었다면
                GameObject bullet = bulletObjectPool[i];
                if (bullet.activeSelf == false)
                {
                    //4.총알을 발사하고 싶다.(활성화시킨다.)
                    bullet.SetActive(true);
                    // 총알을 위치 시키기
                    bullet.transform.position = transform.position;
                    //총알 발사 하였기 때문에 비활성화 총알 검색 중단
                    break;
                }
            }
        }
    }

}
