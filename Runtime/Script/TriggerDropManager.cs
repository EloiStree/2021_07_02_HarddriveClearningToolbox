using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDropManager : MonoBehaviour
{
    public DropperRequestManagerMono m_selectedFilesAndFolders;
    public RaycastToMovingFileCanvas m_selectedDropper;


    public void TriggerDropFromRaycastOnUI() {

        m_selectedDropper.RaycastForDropperUI( out I_UI_Droppable [] found);

        if (found.Length == 1) {

            found[0].GetDroppablePath(out string path);
            m_selectedFilesAndFolders.DropRequest(path);
        
        }
    
    }
 
}
