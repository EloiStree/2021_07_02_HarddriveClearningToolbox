using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FolderTunnel : MonoBehaviour, I_UI_Droppable
{
    public FolderTunnel m_tunnelInfo;
    public InputField m_alias;
    public InputField m_entryPath;
    public InputField m_exitPath;

    void Start()
    {
        Update_Data2UI();
    }

    public void SetWith(FolderTunnel data) {
        m_tunnelInfo = data;
        Update_Data2UI();
    }

    public void PushEntryToExit() {
        FolderTunnelUtility.PushEntryToExit(m_tunnelInfo, FolderTunnelUtility.GetDefaultMoverLogic());
    }
    public void OpenEntryDirectory() { m_tunnelInfo.OpenEntryDirectory(); }
    public void OpenExitDirectory() { m_tunnelInfo.OpenExitDirectory(); }
    public void OpenEntryAndExitDirectoy() { m_tunnelInfo.OpenEntryAndExitDirectory(); }


    public void CreateEntryDirectory() { FolderTunnelUtility.CreateEntryDirectory(m_tunnelInfo); }
    public void DeleteEntryDirectory() { FolderTunnelUtility.DeleteEntryDirectory(m_tunnelInfo); }

    public void Update_UI2Data()
    {
        m_tunnelInfo.SetAlias(m_alias.text);
        m_tunnelInfo.SetEntryPath(m_entryPath.text);
        m_tunnelInfo.SetExitPath(m_exitPath.text);
    }
    public void Update_Data2UI()
    {
        m_alias.text = m_tunnelInfo.GetAlias();
        m_entryPath.text = m_tunnelInfo.GetEntryPath();
        m_exitPath.text=  m_tunnelInfo.GetExitPath();
    }
    #region SAVE DATA BETWEEN SESSION
    public QuickPlayerPrefsSave m_playerPrefs;

    [ContextMenu("Reset Unique ID")]
    public void ResetId()
    {
        m_playerPrefs.GenerateRandomId();
    }
    public void OnEnable()
    {
        m_playerPrefs.GetStoredObject(out FolderTunnel obj, out bool found);
        if (found)
        {
            m_tunnelInfo = obj;
        }
        Update_Data2UI();
    }

    private void OnDisable()
    {
        m_playerPrefs.SetStoredObject(m_tunnelInfo);
        Update_Data2UI();
    }
    private void OnApplicationQuit()
    {
        m_playerPrefs.SetStoredObject(m_tunnelInfo);
    }

    private void Reset()
    {
        m_playerPrefs.GenerateRandomId();
    }

    public void GetDroppablePath(out string path)
    {
        path = m_tunnelInfo.m_exit.GetPath();
    }
    #endregion

}
