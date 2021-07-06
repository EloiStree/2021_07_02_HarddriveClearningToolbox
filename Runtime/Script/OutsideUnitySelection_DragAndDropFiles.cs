using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideUnitySelection_DragAndDropFiles : OutsideUnitySelectionFileAndFolderBehaviour
{

    public FileDragAndDrop m_listener;
    public override void GetSelectedElements(out string[] filesAndFolders)
    {
        filesAndFolders= m_listener.m_filesDropped;
    }

    public override bool IsSelectingSomething()
    {
        return m_listener.m_filesDropped.Length > 0;
    }



}
