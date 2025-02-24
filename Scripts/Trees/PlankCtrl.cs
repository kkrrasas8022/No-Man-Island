using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;

public class PlankCtrl : InteractableObject
{
    bool isBuildReady;
    bool isFixed;
    [SerializeField] GameObject[] contactPoints;

    public override void TakeDamage(int dmg)
    {
        //�Ǽ� �غ� �Ϸ� �� ������ ������ ����(�Ǽ�)
        if (isBuildReady)
        {
            Fix();
            pv.RPC(nameof(Fix), RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    void Fix()
    {
        rig.isKinematic = true;
        isFixed = true;

        //�׷� ��ȣ�ۿ� ���̾� �������� ���� �� ������ ����
        inter.interactionLayers = 1 << InteractionLayerMask.NameToLayer("Fixed");
    }

    private void OnCollisionStay(Collision collision)
    {
        //�Ǽ� ���� ��ü�� ������
        if (collision.gameObject.CompareTag("CanBuild"))
        {
            ContactPoint contactPoint;
            int contactCount = collision.GetContacts(collision.contacts);
            for (int i = 0; i < contactCount; i++)
            {
                //�ݸ��� ���� ������ �� ���� Ʈ���� �ݸ����� ����
                contactPoint = collision.GetContact(i);
                contactPoints[i].SetActive(true);
                contactPoints[i].transform.position = contactPoint.point;

                //���� ���� ���� ����Ʈ ���� ��ŭ��
                if(i == contactPoints.Length - 1)
                {
                    break;
                }
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        //�Ǽ� ���� ��ü�� ��������
        if (collision.gameObject.CompareTag("CanBuild"))
        {
            for(int i = 0; i < contactPoints.Length; i++)
            {
                //���� �ݸ��� ��� ��Ȱ��ȭ
                contactPoints[i].SetActive(false);
            }
            //�Ǽ� �غ� ����
            isBuildReady = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //�ݸ��� ���� ������ ���� ������ �Ǽ��غ�
        if (other.CompareTag("Stone"))
        {
            isBuildReady = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //�ݸ��� ���� �������� ���� ������ �Ǽ��غ� ����
        if (other.CompareTag("Stone"))
        {
            isBuildReady = false;
        }
    }
}
