using System.Collections;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    IEnumerator BurnDmg()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
        }
    }

    //플레이어가 가까이 오면 피해주기
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerState ps = other.gameObject.GetComponentInChildren<PlayerState>();
            //ps.TakeDamage(5);
        }
    }

    //플레이어가 근처에 있으면 체온 올려주기
    //플레이어에서 불을 감지or 불에서 플레이어 감지
    //불에서 플레이어를 감지해서 체온을 올리면 모든 불마다 올려서 중복으로 올라감
    //플레이어에서 불을 감지하는걸로
}
