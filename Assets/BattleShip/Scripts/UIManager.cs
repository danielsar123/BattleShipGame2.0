using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public delegate void ChangeShip(int id);
    public static event ChangeShip OnChangeShip;

    public delegate void ChangeOrientation();
    public static event ChangeOrientation OnChangeOrientation;

    public void OnShipButtonClick(int id)
    {
        OnChangeShip?.Invoke(id);
    }

    public void OnOrientationClick()
    {
        OnChangeOrientation?.Invoke();
    }
}
