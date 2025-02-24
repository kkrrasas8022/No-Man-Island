using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class FireWoodCtrl : PhotonGrabObject
{
    [SerializeField] float burningTime;
    [SerializeField] GameObject fireEffect;

    //��ȭ
    [PunRPC]
    public void FlameOn()
    {
        //�� ����Ʈ �ѱ�
        fireEffect.SetActive(true);
        //���� �ð����� ��Ÿ�� �������
        Destroy(gameObject, burningTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Fire"))
        {
            FlameOn();
            pv.RPC(nameof(FlameOn), RpcTarget.AllViaServer);
        }
    }
}
