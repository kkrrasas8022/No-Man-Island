using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StoneCtrl : PhotonGrabObject
{
    [SerializeField] int power;

    private void OnCollisionEnter(Collision collision)
    {
        //충돌시 속도가 일정 이상일 때 & 플레이어가 잡고있을 때
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

            //돌끼리 부딪히면
            if (collision.gameObject.CompareTag("Stone"))
            {
                int count = 0;
                //주변에 장작 체크
                Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);

                //주변의 장작 갯수 체크
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("FireWood"))
                    {
                        count++;
                    }
                }

                //장작의 갯수가 4개 이상이면 점화
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
