using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //1.만약 부딪힌 물체가 Bullet 이거나 Enemy 이라면
        if (other.gameObject.name.Contains("Bullet") ||
        other.gameObject.name.Contains("Enemy"))
        {
            //2.부딪힌 물체를 비활성화
            other.gameObject.SetActive(false);
        }

    }
}
