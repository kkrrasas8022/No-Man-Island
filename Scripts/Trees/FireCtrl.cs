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

    //�÷��̾ ������ ���� �����ֱ�
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerState ps = other.gameObject.GetComponentInChildren<PlayerState>();
            //ps.TakeDamage(5);
        }
    }

    //�÷��̾ ��ó�� ������ ü�� �÷��ֱ�
    //�÷��̾�� ���� ����or �ҿ��� �÷��̾� ����
    //�ҿ��� �÷��̾ �����ؼ� ü���� �ø��� ��� �Ҹ��� �÷��� �ߺ����� �ö�
    //�÷��̾�� ���� �����ϴ°ɷ�
}
