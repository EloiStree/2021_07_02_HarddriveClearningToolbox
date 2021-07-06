using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class OutsideUnitySelection_WindowShell: OutsideUnitySelectionFileAndFolderBehaviour
{
    public TargetMemoryFileWithMutex m_selectedFilesInMemory;
    public string[] m_selectedPaths;
    public float m_refreshTimer = 0.1f;

  
    internal void MoveSelectionToBin()
    {
        throw new NotImplementedException();
    }

   
    void Start()
    {
        m_selectedFilesInMemory = new TargetMemoryFileWithMutex("SelectedFilesAndFolders");
        InvokeRepeating("Refresh", 0.1f, m_refreshTimer);
    }

    void Refresh()
    {
        if (m_selectedFilesInMemory != null) {
            m_selectedFilesInMemory.TextRecovering(out string pathsAsText, false);
            m_selectedPaths = pathsAsText.Split('\n').Where(K=>!string.IsNullOrEmpty(K)).ToArray();
        }
    }

    public override void GetSelectedElements(out string[] filesAndFolders)
    {
        filesAndFolders = m_selectedPaths;
    }

    public override bool IsSelectingSomething()
    {
        return m_selectedPaths.Length != 0;
    }
}
