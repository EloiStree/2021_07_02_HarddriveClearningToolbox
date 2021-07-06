using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectedFilesAndFolderDebug : MonoBehaviour
{
    public OutsideUnitySelection_WindowShell m_selectedFilesAndFolders;

    public Text m_pathsCount;
    public InputField m_pathsDebug;


    private void Start()
    {
        InvokeRepeating("Refresh", 0.1f, 0.1f);
    }
    public void Refresh() {
        if(m_pathsDebug)
        m_pathsDebug.text = string.Join("\n", m_selectedFilesAndFolders.m_selectedPaths);
        if (m_pathsCount)
            m_pathsCount.text =""+ m_selectedFilesAndFolders.m_selectedPaths.Length;

    }

    public void DeleteSelection() {
        m_selectedFilesAndFolders.MoveSelectionToBin();
    }

}


public interface I_UI_Droppable {

    public void GetDroppablePath(out string path);
}