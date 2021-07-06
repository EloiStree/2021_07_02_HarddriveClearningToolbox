using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuickPlayerPrefsSave
{
    public string m_id;
    public void GenerateRandomId()
    {
        m_id = "QuickSPrefsSave_" + GetRandomNumber() + "_" + GetRandomNumber() + "_" + GetRandomNumber();
    }
    public uint GetRandomNumber() {
        return (uint)(UnityEngine.Random.value * 10000000);
    }

    public void SaveText(string text)
    {
        PlayerPrefs.SetString(m_id, text);
    }
    public void GetText(out string text)
    {
        text = PlayerPrefs.GetString(m_id, "");
    }


    public void GetStoredObject<T>(out T stored, out bool found) where T : class
    {

        GetText(out string text);
        if (text.Length < 1)
        {
            stored = null;
            found = false;
        }
        else
        {
            stored = JsonUtility.FromJson<T>(text);
            found = true;
        }
    }
    public void SetStoredObject<T>(T toStore)
    {

        SaveText(JsonUtility.ToJson(toStore));
    }
}
