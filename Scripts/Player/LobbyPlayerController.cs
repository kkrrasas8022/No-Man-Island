using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class LobbyPlayerController : MonoBehaviourPunCallbacks
{

    [SerializeField] private Animator anim;
    [SerializeField] private InputActionProperty moveAction;

    private Vector2 inputVec;

    private int hasstickX = Animator.StringToHash("stickX");//stick�� hash���� �����ͼ� �����صδ� ����
    private int hasstickY = Animator.StringToHash("stickY");


    private void Update()
    { 
        //�̵��� ���Ͱ��� �޾ƿͼ� �ִϸ��̼ǿ� ��������
        inputVec = moveAction.action.ReadValue<Vector2>();

        //ī�޶� �ޱ��� ���󰡵��� ��
        Vector3 temp = Camera.main.transform.eulerAngles;
        temp.x = temp.z = 0;
        this.transform.eulerAngles = temp;


        //�� ĳ������ �ִϸ��̼��� ������
        anim.SetFloat(hasstickX, inputVec.x);
        anim.SetFloat(hasstickY, inputVec.y);
    }
}
