using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Photon.Pun;
using Photon;
using Photon.Realtime;
using System.Collections;
using Unity.VisualScripting;

public class PhotonGrabObject : MonoBehaviourPunCallbacks
{
    protected Rigidbody rig;
    [SerializeField] protected XRGrabInteractable inter;
    protected int grabCount;
    [SerializeField] protected PhotonView pv;
    int grabLayer;
    int normalLayer;

    protected virtual void Start()
    {
        grabCount = 0;
        grabLayer = LayerMask.NameToLayer("GrabObject");
        normalLayer = LayerMask.NameToLayer("Default");

        rig = GetComponent<Rigidbody>();

        if (inter != null)
        {
            inter.selectEntered.AddListener((args) =>
            {
                //������Ʈ�� PhotonView���� Ownership Transfer�� Takeover�� �����ϸ�
                //������(��Ʈ�ѷ� ����)�� ������ ������ �� �ֵ��� �Ѵ�
                //TransferOwnership(Player) -> ���� PhotonView�� �������� Player�� �ٲٴ� �Լ�
                pv.TransferOwnership(PhotonNetwork.LocalPlayer);
                grabCount++;
                pv.RPC(nameof(Griped), RpcTarget.AllViaServer, grabCount);
            });

            inter.selectExited.AddListener((args) =>
            {
                if(gameObject.activeSelf == true)
                {
                    grabCount--;
                    pv.RPC(nameof(Griped), RpcTarget.AllViaServer, grabCount);
                }
            });
        }
    }

    protected virtual void GripEvent()
    {

    }

    [PunRPC]
    public void Griped(int count)
    {
        if(count > 0)
        {
            rig.useGravity = false;
        }
        else
        {
            rig.useGravity = true;
        }
        //�׸� ���¸� �߷� ����
        //�׸� ���°� �ƴϸ� �߷� ŰŰ
        //rig.useGravity = !isGriped;

        if (count > 0)
        {
            gameObject.layer = grabLayer;
        }
        else
        {
            gameObject.layer = normalLayer;
        }
    }
}