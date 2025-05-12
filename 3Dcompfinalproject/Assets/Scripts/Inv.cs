using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inv : MonoBehaviour
{
    public GameObject inv; //인벤토리 ui
    public static bool isinv; //인벤토리를 열고 있는가?
    public int[] contents; //인벤토리의 상태
    [Space(20)]
    [Header("Data")]
    public TextMeshProUGUI itemname; //아이템 이름 텍스트
    public TextMeshProUGUI itemdesc; //아이템 설명 텍스트
    public GameObject usebtn; //아이템 사용 버튼
    public Image inspecter; //아이템 사진
    public Image[] subnails; //아이템의 사진을 표시하는 image 컴포넌트를 담아두는 배열
    public Sprite[] itemimgs; //아이템의 사진들을 담아두는 배열
    int curitem; //현재 아이템의 종류
    int curitemidx; //현재 마우스로 가리킨 곳의 인덱스
    // Start is called before the first frame update
    void Start()
    {
        isinv = false; //스테틱 초기화
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) //E키를 누르고 있다면
        {
            if(inv.activeSelf == false) //인벤토리를 닫고 있다면
            {
                PlayerCtrl2.instance.sfx.play(1); //효과음 재생
                isinv = true; //인벤토리를 여는중
                inv.SetActive(true); //인벤토리 ui 활성화
                showinv(); //인벤토리의 아이템을 보여주는 함수
            }
            else //인벤토리를 열고 있다면
            {
                PlayerCtrl2.instance.sfx.play(1); //효과음 재생
                isinv = false; //인벤토리를 닫음
                inv.SetActive(false); //인벤토리 ui 비활성화
            }
        }
    }

    public bool itemadd(int itemcode) //아이템 추가 함수
    {
        for(int i = 0; i < contents.Length; i++) //인벤토리의 길이까지 실행
        {
            if (contents[i] == 0) //만약 비어있는 칸이 있다면
            {
                if(itemcode == 2) PlayerCtrl2.instance.book++; //책이라면 책 변수 +1
                contents[i] = itemcode; //그 곳에 아이템 저장
                return false; //인벤토리에 공간이 있음
            }
        }
        return true; //인벤토리에 공간이 없음
    }

    public void showinv() //인벤토리를 보여주는 함수
    {
        for (int i = 0; i < contents.Length; i++) //인벤토리의 길이까지 실행
        {
            if (contents[i] == 0) //만약 i번째 칸이 비어있다면
            {
                subnails[i].sprite = itemimgs[0]; //이미지를 공백으로 설정
                subnails[i].color = Color.clear; //이미지를 투명하게 설정
            }
            else //i번째 칸이 비어있지 않다면
            {
                subnails[i].sprite = itemimgs[contents[i]]; //이미지를 i번째 이미지로 설정 (스프라이트의 순서와 아이템코드의 순서와 같음)
                subnails[i].color = Color.white; //이미지를 보이게 설정
            }
        }
    }

    public void inspect(int i) //인벤토리를 클릭했을 때의 함수
    {
        print("wut");
        curitemidx = i; //현재 클릭한 칸의 인덱스
        switch(contents[i]) //어떤 아이템 코드인가?
        {
            case 0: //만약 아이템 코드가 0이라면
                itemname.text = ""; //아이템 이름 공백
                itemdesc.text = ""; //아이템 설명 공백
                inspecter.color = Color.clear; //사진 투명하게
                usebtn.SetActive(false); //사용버튼 비활성화
                break;
            case 1: //만약 아이템 코드가 1이라면
                itemname.text = "점심"; //아이템 이름 설정
                itemdesc.text = "맛있는 점심이다."; //아이템 설명 설정
                inspecter.sprite = itemimgs[1]; //아이템 사진 설정
                inspecter.color = Color.white; //사진 보이게
                usebtn.SetActive(true); //사용버튼 활성화
                curitem = 1; //현재 아이템 코드는 1
                break;
            case 2: //만약 아이템 코드가 2이라면
                itemname.text = "과제"; //아이템 이름 설정
                itemdesc.text = "살려주세요"; //아이템 설명 설정
                inspecter.sprite = itemimgs[2]; //아이템 사진 설정
                inspecter.color = Color.white; //사진 보이게
                usebtn.SetActive(false); //사용버튼 비활성화
                curitem = 2; //현재 아이템 코드는 2
                break;
            case 3: //만약 아이템 코드가 3이라면
                itemname.text = "배터리"; //아이템 이름 설정
                itemdesc.text = "사람한테 꽂을순 없을까"; //아이템 설명 설정
                inspecter.sprite = itemimgs[3]; //아이템 사진 설정
                inspecter.color = Color.white; //사진 보이게
                usebtn.SetActive(true); //사용버튼 활성화
                curitem = 3; //현재 아이템 코드는 3
                break;
            case 4: //만약 아이템 코드가 4이라면
                itemname.text = "물병"; //아이템 이름 설정
                itemdesc.text = "시원한 물이다!"; //아이템 설명 설정
                inspecter.sprite = itemimgs[4]; //아이템 사진 설정
                inspecter.color = Color.white; //사진 보이게
                usebtn.SetActive(true); //사용버튼 활성화
                curitem = 4; //현재 아이템 코드는 4
                break;
            case 5: //만약 아이템 코드가 5이라면
                itemname.text = "교실 열쇠"; //아이템 이름 설정
                itemdesc.text = "잠긴 문을 열 수 있다"; //아이템 설명 설정
                inspecter.sprite = itemimgs[5]; //아이템 사진 설정
                inspecter.color = Color.white; //사진 보이게
                usebtn.SetActive(false); //사용버튼 비활성화
                curitem = 5; //현재 아이템 코드는 5
                break;
        }
    }

    public void Useitem() //아이템 사용 함수
    {
        switch(curitem) //만약 현재 아이템의 코드가
        {
            case 1: //1이라면
                PlayerCtrl2.instance.eatlunch = true; //점심을 먹은 상태
                PlayerCtrl2.instance.Stamina = 40; //스테미너를 최대로
                StartCoroutine( duration(10, 0)); //지속시간 코루틴
                break;
            case 3: //3이라면
                PlayerCtrl2.instance.sfx.play(0); //효과음 재생
                PlayerCtrl2.instance.energy += 20; //배터리를 20만큼 충전
                if(PlayerCtrl2.instance.energy > 100) //만약 100보다 커진다면
                {
                    PlayerCtrl2.instance.energy = 100; //배터리는 100(오버플로우 방지)
                }
                break;
            case 4: //4라면
                PlayerCtrl2.instance.moveSpeed = 5; //이동속도 증가
                StartCoroutine(duration(10, 1)); //지속시간 코루틴
                break;
        }
        contents[curitemidx] = 0; //사용한 아이템 삭제
        resetinv(); //인벤토리 재정렬
    }

    public void resetinv() //인벤토리 재정렬 함수
    {
        contents = Array.FindAll(contents, num => num != 0).ToArray(); //0인 요소들을 전부 찾아서 그것들을 제외한 배열 생성
        Array.Resize(ref contents, 12); //다시 배열의 길이를 12로 설정
        showinv(); //아이템 보여주기
        itemname.text = ""; //아이템 이름 초기화
        itemdesc.text = ""; //아이템 설명 초기화
        inspecter.color = Color.clear; //아이템 사진 초기화
        usebtn.SetActive(false); //사용버튼 초기화
    }

    IEnumerator duration(int i, int efftype) //지속시간 코루틴
    {
        yield return new WaitForSeconds(i); //i초만큼 지속됨
        if (efftype == 0) //만약 점심효과라면
        {
            PlayerCtrl2.instance.eatlunch = false; //점심효과 제거
        }
        else if (efftype == 1) //만약 물 효과라면
        {
            PlayerCtrl2.instance.moveSpeed = 3; //이동속도 초기화
        }

    }
}
