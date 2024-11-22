using Photon.Pun;
using UnityEngine;
public class WeaponSync : MonoBehaviourPun, IPunObservable
{
    private Vector3 realPosition;
    private Quaternion realRotation;
    void Start()
    {
        if (!photonView.IsMine)
        {
            realPosition = transform.position;
            realRotation = transform.rotation;
        }
    }
    void Update()
    {
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, realPosition, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, Time.deltaTime * 10);
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position); stream.SendNext(transform.rotation);
        }
        else
        {
            realPosition = (Vector3)stream.ReceiveNext();
            realRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}