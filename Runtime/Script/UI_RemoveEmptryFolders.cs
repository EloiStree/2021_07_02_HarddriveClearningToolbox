using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_RemoveEmptryFolders : MonoBehaviour
{

    public InputField m_targetPath;

    public void RemoveEmptyFolders() {

       string pathTarget = m_targetPath.text.Trim() ;
       string [] folderPaths = Directory.GetDirectories(pathTarget, "*", SearchOption.AllDirectories);

        folderPaths = folderPaths.OrderByDescending(k=>k.Count(f => f == '/' || f == '\\'  )).ToArray();
        for (int i = 0; i < folderPaths.Length; i++)
        {
            if (IsDirectoryEmpty(folderPaths[i]))
            {
                try
                {

                    Directory.Delete(folderPaths[i]);
                }
                catch (Exception e) { 
                
                }
            
            }
        }
    
    }

    public bool IsDirectoryEmpty(string path)
    {
        return !Directory.EnumerateFileSystemEntries(path).Any();
    }
}
