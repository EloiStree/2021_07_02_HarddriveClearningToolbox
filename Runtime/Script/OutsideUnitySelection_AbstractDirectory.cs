using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class OutsideUnitySelection_AbstractDirectory : OutsideUnitySelectionFileAndFolderBehaviour
{
    public string m_directoryTarget;
    public string[] m_selectedPaths;
    public float m_refreshTimer=0.1f;

    public override void GetSelectedElements(out string[] filesAndFolders)
    {
        filesAndFolders = m_selectedPaths;
    }

    public override bool IsSelectingSomething()
    {
        return m_selectedPaths.Length != 0;
    }

    void Start()
    {
        InvokeRepeating("Refresh", 0, m_refreshTimer);   
    }

    void Refresh()
    {
        List<string> selection = new List<string>();
        selection.AddRange(Directory.GetFiles(m_directoryTarget));
        selection.AddRange(Directory.GetDirectories(m_directoryTarget));
        m_selectedPaths = selection.ToArray();
    }
}



public interface OutsideUnitySelectionFileAndFolder {

    public void GetSelectedElements(out string[] filesAndFolders);
    public bool IsSelectingSomething();
}
public abstract class OutsideUnitySelectionFileAndFolderBehaviour : MonoBehaviour, OutsideUnitySelectionFileAndFolder
{
    public abstract void GetSelectedElements(out string[] filesAndFolders);
    public abstract bool IsSelectingSomething();
}