using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("interactable obj")]
    public bool is_interactable; //상호작용이 가능한가?
    public string[] description; //설명용 배열
    [Space(10)]
    public bool have_events; //상호작용시 이벤트가 일어나는가?
    public bool requireitem; //이벤트를 위해선 아이템이 필요한가?
    public int requireitemcode; //어떤 아이템이 필요한가
    public Animation assignevent; //어떤 이벤트가 발생하는가
    public string description2; //이벤트 후엔 설명이 어떻게 바뀌는가
    [Space(20)]
    [Header("item")]
    public bool is_item; //아이템인가?
    public string itemname; //아이템의 이름은?
    public int itemcode; //아이템의 코드는?
}
