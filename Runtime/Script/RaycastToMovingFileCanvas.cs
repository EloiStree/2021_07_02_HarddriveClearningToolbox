using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RaycastToMovingFileCanvas : MonoBehaviour
{

    [SerializeField] GraphicRaycaster m_raycaster;
    PointerEventData m_pointerEventData;
    [SerializeField] EventSystem m_eventSystem;

   

    [SerializeField] RectTransform m_canvasRect;

    public RectTransform m_debugDrop;

    public List<GameObject> m_pointedAt;
    public List<MonoBehaviour> m_droppableFound;


    void Update()
    {
        
        Vector3 v3 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        m_debugDrop.anchorMin = new Vector2(v3.x, v3.y);
        m_debugDrop.anchorMax = new Vector2(v3.x, v3.y);
    }

    public void RaycastForDropperUI()
    {
        m_pointerEventData = new PointerEventData(m_eventSystem);
        m_pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();

        m_raycaster.Raycast(m_pointerEventData, results);
        m_pointedAt = results.Select(k => k.gameObject).ToList();

        m_droppableFound.Clear();
        for (int i = 0; i < m_pointedAt.Count; i++)
        {
            m_droppableFound.AddRange(m_pointedAt[i].GetComponents<MonoBehaviour>().Where(k => k is I_UI_Droppable));
        }
    }
    public void RaycastForDropperUI(out I_UI_Droppable[] found)
    {
        RaycastForDropperUI();
        found = m_droppableFound.Select(k=>k as I_UI_Droppable).ToArray();
    }

}
