using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField] RectTransform roomContent;
    [SerializeField] RoomEntry roomEntryPrefab;

    Dictionary<string, RoomInfo> roomDictionary;

    private void Awake()
    {
        roomDictionary = new Dictionary<string, RoomInfo>();
    }

    private void OnDisable()
    {
        roomDictionary.Clear();
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        // Clear room list
        for (int i = 0; i < roomContent.childCount; i++)
        {
            Destroy(roomContent.GetChild(i).gameObject);
        }

        // Update room info
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            { // ���̿������������� , ��������, ���� ����� ����
                if (roomDictionary.ContainsKey(info.Name))
                {
                    roomDictionary.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            // ���� ��������� <-- ����� �׳� �̸����־������̸�
            if (roomDictionary.ContainsKey(info.Name))
            {
                roomDictionary[info.Name] = info;
            }
            // Add new room info to cache
            else
            {//���ο��
                roomDictionary.Add(info.Name, info);
            }
        }

        // Create room list // UI�� �߰�
        foreach (RoomInfo info in roomDictionary.Values)
        {
            RoomEntry entry = Instantiate(roomEntryPrefab, roomContent);
            entry.SetRoomInfo(info);
        }
    }

    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }
}
