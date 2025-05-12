using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioClip[] clips; //이 소스가 재생할 클립들을 담아두는 배열
    public AudioSource source; //어떤 소스로 재생할 것인가?

    public void step1() //발소리1
    {
        source.clip = clips[0]; //발소리 1 클립을 재생
        source.Play();
    }
    public void step2() //발소리2
    {
        source.clip = clips[1]; //발소리 2 클립을 재생
        source.Play();
    }
    public void play(int i) //재생함수
    {
        source.clip = clips[i]; //i번째에 있는 클립을 재생
        source.Play();
    }
}
