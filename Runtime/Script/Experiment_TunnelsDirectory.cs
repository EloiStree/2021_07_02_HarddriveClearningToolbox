using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

public class Experiment_TunnelsDirectory : MonoBehaviour
{
    public FolderTunnel [] m_tunnels;
    public DefaultWindowedFilesMover m_defaultMover= new DefaultWindowedFilesMover();


    [ContextMenu("Test Moving Files")]
    public void TestMovingFiles()
    {
        for (int i = 0; i < m_tunnels.Length; i++)
        {
            FolderTunnelUtility.PushEntryToExit(m_tunnels[i], m_defaultMover);

        }
    }
    [ContextMenu("Create Tunnel")]
    public void CreateEntry()
    {
        for (int i = 0; i < m_tunnels.Length; i++)
        {
            FolderTunnelUtility.CreateEntryDirectory(m_tunnels[i]);

        }
    }
    [ContextMenu("Close Tunnel")]
    public void CloseEntry()
    {
        for (int i = 0; i < m_tunnels.Length; i++)
        {
            FolderTunnelUtility.DeleteEntryDirectory(m_tunnels[i]);

        }
    }
    [ContextMenu("Show Entries of Tunnel")]
    public void ShowEntries()
    {
        for (int i = 0; i < m_tunnels.Length; i++)
        {

            m_tunnels[i].OpenEntryAndExitDirectory();
        }
    }
}


public interface HowToMoveFilesAndFolder {

    public void Move(string elementToMovePath, string folderDestinationPath );

}

public class DefaultWindowedFilesMover : HowToMoveFilesAndFolder
{
    public float m_timeBeforeExit = 2;
    public bool m_requestAdminPower;

    public MoveType m_whatToUseToMove = MoveType.Batch_Robocopy;
    public enum MoveType { Batch_Move, Batch_Robocopy, App_TeraCopy };
    public static string GetCommandToMove(string from, string to, MoveType whatToUse)
    {
        if (whatToUse == MoveType.Batch_Robocopy) {
            if (File.Exists(from))
                return "robocopy  \"" + from + "\" \"" + to + "\" /S /E /Move";


            if (Directory.Exists(from)) {

                int indexToCut = from.Replace("\\", "/").LastIndexOf("/");
                if (indexToCut >= 0) {
                    string nameFolder = from.Substring(indexToCut);
                    return "robocopy  \"" + from + "\" \"" + to + nameFolder + "\" /S  /Move";
                
                }

            
            }

        }
        if (whatToUse == MoveType.App_TeraCopy)
            return "TeraCopy move \"" + from + "\" \"" + to + "\"  ";


        return "move /-Y \"" + from + "\" \"" + to + "\"  ";
    }
    public void Move(string elementToMovePath, string folderDestinationPath)
    {
        MoveWithWindowProcess(elementToMovePath, folderDestinationPath, m_timeBeforeExit, m_whatToUseToMove);
    }
    public static void MoveWithWindowProcess( string what, string to, float timeDisplayingMessage= 1,MoveType moveType = MoveType.Batch_Robocopy)
    {
        what = what.Trim();

        if (!Directory.Exists(what) && !File.Exists(what))
           return;

        if (!Directory.Exists(to))
            Directory.CreateDirectory(to);

        string cmdToMove = GetCommandToMove(what, to, moveType);
        string[] cmds = new string[]{
             "echo \" Start moving stuffs...\" "
            ,"echo \"CMD| "+cmdToMove+"\""
            , cmdToMove
            , "echo \" ... Stop moving stuffs \"  "
            //, "if not exist \""+what+"\" echo \"Has been moved\" "
            //, "if exist \""+what+"\" echo \" Should have moved \" "
            //, "if not exist \""+to+"\" echo \" Had not been move \" "
            //, "if exist \""+to+"\" echo \"Is Arrived at destination\" " 
            , "TIMEOUT /T " +timeDisplayingMessage+" "
            , "EXIT  "
        };
        string folderPath = Directory.Exists(what)? what: Directory.GetDirectoryRoot(what);

        //UnityEngine.Debug.Log("--ttti->" + what);
        MoveWithWindowProcess(cmds, folderPath);
    }
    public static void MoveWithWindowProcess(IEnumerable<string> cmds, string workingDirectory = "", bool requestAdminPower=false)
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
        processStartInfo.WorkingDirectory = workingDirectory;
        processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
        processStartInfo.Arguments = "/K " + string.Join("&", cmds.ToList());
        if(requestAdminPower)
        processStartInfo.Verb = "runas";
        Process process = Process.Start(processStartInfo);

    }
}


public class FolderTunnelUtility {

    public static DefaultWindowedFilesMover GetDefaultMoverLogic() { return m_defaultMover; }
    static DefaultWindowedFilesMover m_defaultMover = new DefaultWindowedFilesMover();
    public static void PushEntryToExit( FolderTunnel tunnel, HowToMoveFilesAndFolder mover)
    {

        string destination = tunnel.m_exit.GetPath();
        tunnel.GetFilesAndFoldersList(out string[] files, out string[] directories);
        for (int i = 0; i < files.Length; i++)
        {
            mover.Move(files[i], destination);
        }
        for (int i = 0; i < directories.Length; i++)
        {
            mover.Move(directories[i], destination);
        }
    }

    public static void CreateEntryDirectory(FolderTunnel tunnel) {
        Directory.CreateDirectory(tunnel.m_entry.GetPath());
    }
    public static void DeleteEntryDirectory(FolderTunnel tunnel ) {
        
            Directory.Delete(tunnel.m_entry.GetPath());
        
    }
    public static bool IsEntryEmpty(FolderTunnel tunnel) {
        return tunnel.IsEntryEmpty();
    }

}

[System.Serializable]
public class FolderTunnel {
    public string m_alias="Unnamed";
    public FolderPath m_entry;
    public FolderPath m_exit;


   

    public void GetFilesAndFoldersList(out string[] files, out string [] directories) {

        files = Directory.GetFiles(m_entry.GetPath());
        directories = Directory.GetDirectories(m_entry.GetPath());
    }

    public void OpenEntryAndExitDirectory()
    {
        Application.OpenURL(m_entry.GetPath());
        Application.OpenURL(m_exit.GetPath());
    }
    public void OpenEntryDirectory()
    {
        Application.OpenURL(m_entry.GetPath());
    }
    public void OpenExitDirectory()
    {
        Application.OpenURL(m_exit.GetPath());
    }

    public bool IsEntryEmpty()
    {
        return Directory.GetFiles(m_entry.GetPath()).Length == 0 && Directory.GetDirectories(m_entry.GetPath()).Length ==0;
    }
    public uint HasElementsInEntry() {
        return (uint) ( Directory.GetFiles(m_entry.GetPath()).Length + Directory.GetDirectories(m_entry.GetPath()).Length ) ; 
    }

    public string GetAlias()
    {
        return m_alias;
    }

    public string GetEntryPath()
    {
        return m_entry.GetPath();
    }

    public string GetExitPath()
    {
        return m_exit.GetPath();
    }

    public void SetExitPath(string text)
    {
        m_exit.m_path = text;
    }

    public void SetEntryPath(string text)
    {
        m_entry.m_path = text;
    }

    public void SetAlias(string text)
    {

        m_alias=text;
    }
}

[System.Serializable]
public class FolderPath {
    public string m_path;

    public bool Exists() { return Directory.Exists(m_path); }
    public string GetPath()
    {
        return m_path;
    }

    public string GetEndNameOfPath()
    {
        if (File.Exists(m_path))
            return Path.GetFileNameWithoutExtension(m_path);
        else
        {

            int indexOfNameStart = m_path.Replace("\\", "/").LastIndexOf("/");
            if (indexOfNameStart < 0 || indexOfNameStart+1>=m_path.Length)
                return "";
            else  return m_path.Substring(indexOfNameStart+1);
        }
    }
}
