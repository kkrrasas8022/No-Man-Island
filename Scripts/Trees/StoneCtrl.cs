using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StoneCtrl : PhotonGrabObject
{
    [SerializeField] int power;

    private void OnCollisionEnter(Collision collision)
    {
        //�浹�� �ӵ��� ���� �̻��� �� & �÷��̾ ������� ��
        if(rig.linearVelocity.magnitude > 0.7f && grabCount > 0)
        {
            if(collision.gameObject.GetComponent<InteractableObject>() != null)
            {
                collision.gameObject.GetComponent<InteractableObject>().TakeDamage(power);
            }

            if (collision.gameObject.GetComponentInParent<AnimalClass>() != null)
            {
                collision.gameObject.GetComponentInParent<AnimalClass>().CallDamageRPC(power);
            }

            //������ �ε�����
            if (collision.gameObject.CompareTag("Stone"))
            {
                int count = 0;
                //�ֺ��� ���� üũ
                Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);

                //�ֺ��� ���� ���� üũ
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("FireWood"))
                    {
                        count++;
                    }
                }

                //������ ������ 4�� �̻��̸� ��ȭ
                if( count >= 4)
                {
                    foreach (Collider collider in colliders)
                    {
                        if (collider.CompareTag("FireWood"))
                        {
                            collider.GetComponent<PhotonView>().RPC("FlameOn", RpcTarget.AllViaServer);
                        }
                    }
                }
            }
        }
    }
}
