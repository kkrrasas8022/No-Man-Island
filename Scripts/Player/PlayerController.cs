using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PlayerController : MonoBehaviourPunCallbacks
{

    [SerializeField] private Animator anim;
    [SerializeField] private InputActionProperty moveAction;

    private Vector2 inputVec;
    private Quaternion rotQ;
    private Vector3 rotV;
    [SerializeField] private PhotonView pv;
    [SerializeField] private CharacterController cctr;
    private Rigidbody rb;
    [SerializeField] private GameObject[] models;

    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject statusUI;
    [SerializeField] private GameObject statusUISencer;
    [SerializeField] private GameObject mouth;



    private int hasstickX = Animator.StringToHash("stickX");//stick�� hash���� �����ͼ� �����صδ� ����
    private int hasstickY = Animator.StringToHash("stickY");



    private void Start()
    {
        //�� ��ü�� ��쿡 ���� ���ְ� 
        if (pv.IsMine)
        {
            Transform tr = GameObject.Find("XR Origin (VR)").transform;
            Vector3 temp = transform.position;
            this.transform.root.parent = tr;
            transform.position = Vector3.zero;
            tr.position=temp;
            //���� XR Origin�� ī�޶� ������ ������ �ű�
            leftHand.transform.parent=tr.GetChild(0).GetChild(1);
            statusUISencer.transform.parent=tr.GetChild(0).GetChild(1);
            rightHand.transform.parent=tr.GetChild(0).GetChild(2);
            statusUI.transform.parent = tr.GetChild(0);
            mouth.transform.parent = Camera.main.transform;
            mouth.transform.localPosition = new Vector3(0, -0.06f, 0.05f);

            leftHand.transform.localPosition = Vector3.zero;
            rightHand.transform.localPosition = Vector3.zero;
            leftHand.transform.localRotation = Quaternion.identity;
            rightHand.transform.localRotation = Quaternion.identity;
            statusUISencer.transform.localPosition = Vector3.right * 0.2f;

            //�� ���̸� �������ͽ� ������ ��� ��
            if(SceneManager.GetActiveScene().name== "3_RoomScene")
            {
                GetComponent<PlayerState>().enabled = false;
                statusUISencer.gameObject.SetActive(false);
                statusUI.gameObject.SetActive(false);
            }    



            models[0].SetActive(false);
            models[1].SetActive(false);
        }
        //�� ��ü�� �ƴ� ���
        else
        {
            statusUISencer.gameObject.SetActive(false);
            statusUI.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        //�� ��ü�� �ƴϸ� �۵������ʴ´�
        if (!pv.IsMine)
            return;

        //�̵��� ���Ͱ��� �޾ƿͼ� �ִϸ��̼ǿ� ��������
        inputVec = moveAction.action.ReadValue<Vector2>();

        //ī�޶� �ޱ��� ���󰡵��� ��
        Vector3 temp = Camera.main.transform.eulerAngles;
        temp.x = temp.z=0;
        this.transform.eulerAngles = temp;
        

        //�� ĳ������ �ִϸ��̼��� ������
        anim.SetFloat(hasstickX, inputVec.x);
        anim.SetFloat(hasstickY, inputVec.y);
    }
}
