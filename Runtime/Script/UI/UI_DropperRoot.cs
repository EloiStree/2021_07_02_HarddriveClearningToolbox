using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface DropperDestination
{
    public void GetDestinationPath(out string path);
}
public interface DropperBasicInformation
{
    public void GetIconPath(out string pathOrUrl);
    public void GetAliasName(out string name);
}

public interface DropperInfo: DropperDestination, DropperBasicInformation { }

public interface UI2D_DropperRootInterface: DropperInfo { 


}

public class UI_DropperRoot : MonoBehaviour, DropperInfo
{
    public string m_aliasName;
    public string m_iconPath;
    public string m_destinationPath;

    public void GetAliasName(out string aliasName)
    {
        aliasName = m_aliasName;
    }

    public void GetIconPath(out string pathOrUrlOfIcon)
    {
        pathOrUrlOfIcon = m_iconPath;
    }

    public void GetDestinationPath(out string destinationPath)
    {
        destinationPath = m_destinationPath;
    }

   
}
