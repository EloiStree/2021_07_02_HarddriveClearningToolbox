using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Dropper : MonoBehaviour ,I_UI_Droppable
{
    public Dropper m_dropper;
    public InputField m_alias;
    public InputField m_destinationPath;
    public InputField m_iconPathOrUrl;
    public RawImage m_iconDisplayer;
    public DropperUnityEvent m_requestDrop;

    void Start()
    {
        Update_Data2UI();
    }

    public void SetWith(Dropper data)
    {
        m_dropper = data;
        Update_Data2UI();
    }

    public void TriggerRequestDrop() {
        m_requestDrop.Invoke(m_dropper);
    }


    public void Update_UI2Data()
    {
        m_dropper.m_alias = m_alias.text;
        m_dropper.m_destinationPath = m_destinationPath.text;
        m_dropper.m_iconPathOrUrl = m_iconPathOrUrl.text;

    }
    public void Update_Data2UI()
    {
        m_alias.text = m_dropper.m_alias;
        m_destinationPath.text = m_dropper.m_destinationPath;
        m_iconPathOrUrl.text = m_dropper.m_iconPathOrUrl;

      //  StartCoroutine(LoadIcon(m_dropper.m_iconPathOrUrl));

    }
    public void OpenDirectory() {
        Application.OpenURL(m_dropper.m_destinationPath);
    }

    public IEnumerator LoadIcon(string url) {

        WWW www = new WWW(url);
        yield return www;
        m_iconDisplayer.texture = www.texture;

    }

    [System.Serializable]
    public class DropperUnityEvent: UnityEvent<Dropper>{}


    #region SAVE DATA BETWEEN SESSION
    public QuickPlayerPrefsSave m_playerPrefs;

    [ContextMenu("Reset Unique ID")]
    public void ResetId()
    {
        m_playerPrefs.GenerateRandomId();
    }
    public void OnEnable()
    {
        m_playerPrefs.GetStoredObject(out Dropper obj, out bool found);
        if (found)
        {
            m_dropper = obj;
        }
        Update_Data2UI();
    }

    private void OnDisable()
    {
        m_playerPrefs.SetStoredObject(m_dropper);
        Update_Data2UI();
    }
    private void OnApplicationQuit()
    {
        m_playerPrefs.SetStoredObject(m_dropper);
    }

    private void Reset()
    {
        m_playerPrefs.GenerateRandomId();
    }

    public void GetDroppablePath(out string path)
    {
        path = m_dropper.m_destinationPath;
    }
    #endregion
}


public class DropperUtility {

    public static DefaultWindowedFilesMover GetDefaultMoverLogic() { return m_defaultMover; }
    static DefaultWindowedFilesMover m_defaultMover = new DefaultWindowedFilesMover();

    public static void MoveSelectionToDropperDestination(Dropper dropper, string[] selectedPath, HowToMoveFilesAndFolder mover = null)
    {
        if (mover == null)
            mover = m_defaultMover;

        // UnityEngine.Debug.Log("--d->" + string.Join("\n", selectedPath));
        string destination = dropper.m_destinationPath;
        MoveSelectionToDropperDestination(destination, selectedPath, mover);

    }
        public static void MoveSelectionToDropperDestination(string destination, string [] selectedPath, HowToMoveFilesAndFolder mover=null)
    {
        destination = destination.Replace("/", "\\");
        for (int i = 0; i < selectedPath.Length; i++)
        {
           selectedPath[i] = selectedPath[i].Replace("/", "\\");
        }
        if (mover == null)
            mover = m_defaultMover;


        string[] paths = selectedPath;
        for (int i = 0; i < paths.Length; i++)
        {
            paths[i] = paths[i].Trim();
            if (!string.IsNullOrEmpty(paths[i]) && (File.Exists(paths[i]) || Directory.Exists(paths[i])))
            {

                UnityEngine.Debug.Log("--i->"+ paths[i]);
                mover.Move(paths[i].Trim(), destination);
            }
        }
        
    }
}

[System.Serializable]
public class Dropper {
    public string m_alias;
    public string m_iconPathOrUrl;
    public string m_destinationPath;

   
}

public class DropperEvent {
    public Dropper m_dropSelected;
}
public delegate void DropperFunctionCall(Dropper dropper);

public class DropperGlobalListener {

    private static DropperFunctionCall m_outsideUnitySelectionToDestination;
    public static void TakeInChargeOutsideUnitySelection(DropperFunctionCall howToMoveAndWhat)
    {
        m_outsideUnitySelectionToDestination = howToMoveAndWhat;

    }
    public static void MoveSelectionToDropperDestination(Dropper dropper)
    {
        if(m_outsideUnitySelectionToDestination!=null)
            m_outsideUnitySelectionToDestination(dropper);
    }
    public static bool IsMoveOutsideUnitySelectionTakeInCharge() { return m_outsideUnitySelectionToDestination != null; }

}