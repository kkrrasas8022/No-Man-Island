using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class CuttedBambooCtrl : InteractableObject
{
    bool isBuildReady;
    public bool isFixed;
    bool isTrigger;
    [SerializeField] int power;
    [SerializeField] private InputActionProperty leftTriggerAction;
    [SerializeField] private InputActionProperty rightTriggerAction;

    [SerializeField] GameObject contackPoints;

    public override void OnEnable()
    {
        //���� �̺�Ʈ ���
        leftTriggerAction.action.performed += LeftTriggerEnter;
        rightTriggerAction.action.performed += RightTriggerEnter;

        leftTriggerAction.action.canceled += LeftTriggerExit;
        rightTriggerAction.action.canceled += RightTriggerExit;
    }

    public override void OnDisable()
    {
        leftTriggerAction.action.performed -= LeftTriggerEnter;
        rightTriggerAction.action.performed -= RightTriggerEnter;

        leftTriggerAction.action.canceled -= LeftTriggerExit;
        rightTriggerAction.action.canceled -= RightTriggerExit;
    }

    public override void TakeDamage(int dmg)
    {
        //�Ǽ� �غ� �Ϸ��
        if (isBuildReady)
        {
            //Fix();
            pv.RPC(nameof(Fix), RpcTarget.AllViaServer);
        }
    }

    //���� ��Ű�� �Լ�
    [PunRPC]
    void Fix()
    {
        //���� �ȹޱ�
        rig.isKinematic = true;
        isFixed = true;
        //�׷� ��ȣ�ۿ� ���̾� �������� ���� �� ������ ����
        inter.interactionLayers = 1 << InteractionLayerMask.NameToLayer("Fixed");
    }

    //Ʈ���� �Է� üũ �Լ���
    void LeftTriggerEnter(InputAction.CallbackContext context)
    {
        if (grabCount > 0)
        {
            isTrigger = true;
        }
    }
    void RightTriggerEnter(InputAction.CallbackContext context)
    {
        if (grabCount > 0)
        {
            isTrigger = true;
        }
    }
    void LeftTriggerExit(InputAction.CallbackContext context)
    {
        if (grabCount > 0)
        {
            isTrigger = false;
        }
    }
    void RightTriggerExit(InputAction.CallbackContext context)
    {
        if (grabCount > 0)
        {
            isTrigger = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(pv.IsMine)
        {
            if(rig != null)
            {
                //�ٴڿ� ���� �� �ֵ��� ���ִ� �ڵ�
                if (collision.gameObject.CompareTag("Land"))
                {
                    //�浹�� �ӵ��� ���� �̻�, Ʈ���Ÿ� ���� ����
                    if (rig.linearVelocity.magnitude > 0.7f && isTrigger)
                    {
                        //Fix();
                        pv.RPC(nameof(Fix), RpcTarget.AllViaServer);
                    }
                }

                if (rig.linearVelocity.magnitude > 0.7f && grabCount > 0)
                {
                    if (collision.gameObject.GetComponentInParent<AnimalClass>() != null)
                    {
                        collision.gameObject.GetComponentInParent<AnimalClass>().CallDamageRPC(power);
                    }
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //�Ǽ� ���� ��ü�� ������
        if (collision.gameObject.CompareTag("CanBuild"))
        {
            //�ݸ��� ���� ������ �� ���� Ʈ���� �ݸ����� ����
            contackPoints.SetActive(true);
            ContactPoint contactPoint = collision.GetContact(0);
            contackPoints.transform.position = contactPoint.point;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        //�Ǽ� ���� ��ü�� ��������
        if (collision.gameObject.CompareTag("CanBuild"))
        {
            //���� �ݸ��� ��� ��Ȱ��ȭ, �Ǽ� �غ� ����
            contackPoints.SetActive(false);
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
