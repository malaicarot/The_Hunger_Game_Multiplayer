using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class PlayerSync : MonoBehaviourPun, IPunObservable
{
    public MonoBehaviour[] localScripts; // cac script cuc bo
    public GameObject[] localObjects;    // cac doi tuong cuc bo
    Rigidbody2D playerRb;               // Thanh phan Rigidbody cua doi tuong

    /**************************************************************/

    Vector3 latestPos;                  // Vi tri moi nhat nhan duoc tu nha mang
    Quaternion latestRot;               // Goc quay moi nhat
    Vector2 latestVelocity;             // Van toc moi nhat
    float latestAngularVelocity;         // Van toc goc moi nhat

    /**************************************************************/
    float currentTime = 0.0f;              // Thoi gian hien tai
    double currentPacketTime = 0;       // Thoi gian ghi o goi tin hien tai
    double lastPacketTime = 0;          // Thoi gian ghi o goi tin truoc do

    /**************************************************************/
    Vector3 positionAtLastPacket = Vector3.zero;           // Vi tri tai thoi diem goi tin cuoi cung duoc nhan
    Quaternion rotationAtLastPacket = Quaternion.identity; // Goc quay tai thoi diem goi tin cuoi cung duoc nhan
    Vector2 velocityAtLastPacket = Vector2.zero;           // Van toc tai thoi diem goi tin cuoi cung duoc nhan
    float angularVelocityAtLastPacket = 0.0f;                  // Van toc goc tai thoi diem goi tin cuoi cung duoc nhan

    void Start()
    {

        playerRb = GetComponent<Rigidbody2D>();
        // playerRb.isKinematic = !photonView.IsMine; // Nếu không phải của người chơi cục bộ, đặt kinematic để không bị ảnh hưởng bởi vật lý


        for (int i = 0; i < localScripts.Length; i++)
        {
            localScripts[i].enabled = photonView.IsMine; 
        }

        for (int i = 0; i < localObjects.Length; i++)
        {
            localObjects[i].SetActive(photonView.IsMine); 
        }
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            transform.position = Vector3.Lerp(positionAtLastPacket, latestPos, (float)(currentTime / timeToReachGoal));
            transform.rotation = Quaternion.Lerp(rotationAtLastPacket, latestRot, (float)(currentTime / timeToReachGoal));
            playerRb.velocity = Vector2.Lerp(velocityAtLastPacket, latestVelocity, (float)(currentTime / timeToReachGoal));
            playerRb.angularVelocity = Mathf.Lerp(angularVelocityAtLastPacket, latestAngularVelocity, (float)(currentTime / timeToReachGoal));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(playerRb.velocity);
            stream.SendNext(playerRb.angularVelocity);
        }
        else
        {
            latestPos = (Vector3)stream.ReceiveNext();
            latestRot = (Quaternion)stream.ReceiveNext();
            latestVelocity = (Vector2)stream.ReceiveNext();
            latestAngularVelocity = (float)stream.ReceiveNext();

            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            positionAtLastPacket = transform.position;
            rotationAtLastPacket = transform.rotation;
            velocityAtLastPacket = playerRb.velocity;
            angularVelocityAtLastPacket = playerRb.angularVelocity;
        }
    }
}