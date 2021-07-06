using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PageFormatQuickScript : MonoBehaviour
{
    public GameObject[] m_target;
    public int m_index;


    public void Next() {

        m_index++;
        if (m_index < 0)
            m_index = m_target.Length - 1;
        if (m_index >= m_target.Length)
            m_index = 0;
        
        for (int i = 0; i < m_target.Length; i++)
        {
            m_target[i].SetActive(i == m_index);
        }

    }
}
