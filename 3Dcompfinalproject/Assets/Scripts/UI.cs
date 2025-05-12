using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public GameObject info; //튜토리얼 패널
    public AudioSource vfx; //효과음
    public void gamestart() //게임시작 함수
    {
        vfx.Play(); //효과음 재생
        SceneManager.LoadScene("PlayScene"); //게임시작
    }
    public void exit() //게임종료
    {
        vfx.Play(); //효과음 재생
        Application.Quit(); //게임종료
    }

    public void showinfo() //튜토리얼 출력 함수
    {
        vfx.Play(); //효과음 재생
        info.SetActive(true); //튜토리얼 출력
    }

    public void totitle() //타이틀로 돌아가는 함수
    {
        vfx.Play(); //효과음 재생
        SceneManager.LoadScene("Startscene"); //타이틀로 돌아가기
    }
}
