using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DirectoryOpenShortcut : MonoBehaviour, I_UI_Droppable
{

    public DirectoryOpenShortcut m_openShortCut;
    public Text m_label;
    public InputField m_targetPath;
    public InputField m_alias;




    public void Update_Date2UI()
    {
        m_label.text = m_openShortCut.GetAliasOrDirectoryName();
        m_targetPath.text = m_openShortCut.m_targetPath.GetPath();
        if(m_alias)
          m_alias.text = m_openShortCut.m_alias;
    }
    public void Update_UI2Data()
    {

       m_openShortCut.m_targetPath.m_path = m_targetPath.text;
        if (m_alias)
            m_openShortCut.m_alias = m_alias.text;
        Update_Date2UI();

    }

    public void OpenShortcut() {
        Application.OpenURL(m_openShortCut.m_targetPath.GetPath());
    
    }

    #region SAVE DATA BETWEEN SESSION
    public QuickPlayerPrefsSave m_playerPrefs;

    [ContextMenu("Reset Unique ID")]
    public void ResetId() {
        m_playerPrefs.GenerateRandomId();
    }
    public void OnEnable()
    {
        m_playerPrefs.GetStoredObject(out DirectoryOpenShortcut obj, out bool found);
        if (found) {
            m_openShortCut = obj;
        }
        Update_Date2UI();
    }

    private void OnDisable()
    {
        m_playerPrefs.SetStoredObject(m_openShortCut);
        Update_Date2UI();
    }
    private void OnApplicationQuit()
    {
        m_playerPrefs.SetStoredObject(m_openShortCut);
    }

    private void Reset()
    {
        m_playerPrefs.GenerateRandomId();
    }

    public void GetDroppablePath(out string path)
    {
        path = m_openShortCut.m_targetPath.GetPath();
    }
    #endregion
}


[System.Serializable]
public class DirectoryOpenShortcut {

    public string m_alias;
    public FolderPath m_targetPath;

    public string GetAliasOrDirectoryName() {

        if (string.IsNullOrEmpty(m_alias))
        {
            return m_targetPath.GetEndNameOfPath();
        }
        else return m_alias;
    }

}