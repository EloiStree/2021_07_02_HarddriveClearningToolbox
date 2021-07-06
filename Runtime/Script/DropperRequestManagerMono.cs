using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperRequestManagerMono : MonoBehaviour
{

    //Should be general
    public DefaultWindowedFilesMover.MoveType m_moveType = DefaultWindowedFilesMover.MoveType.Batch_Move;
    public OutsideUnitySelectionFileAndFolderBehaviour m_outsideUnitySelection;
    public DefaultWindowedFilesMover m_mover;

    private void Awake()
    {
        m_mover = new DefaultWindowedFilesMover();
        m_mover.m_whatToUseToMove = m_moveType;
    }
    public void DropRequest(Dropper dropDestination)
    {
        m_outsideUnitySelection.GetSelectedElements(out string[] paths);
        UnityEngine.Debug.Log("--->" + string.Join("\n", paths));
        DropperUtility.MoveSelectionToDropperDestination(dropDestination, paths, m_mover);
    }
    public void DropRequest(string dropDestination)
    {
        m_outsideUnitySelection.GetSelectedElements(out string[] paths);
        UnityEngine.Debug.Log("--->" + string.Join("\n", paths));
        DropperUtility.MoveSelectionToDropperDestination(dropDestination, paths, m_mover);
    }


}



