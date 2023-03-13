using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDebugDisplay : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool Enabled;

    [Header("References")]
    [SerializeField] private Canvas debugCanvas;

    [SerializeField] private UIDebugDisplayLine MPosScreen; 
    [SerializeField] private UIDebugDisplayLine MPosWorld; 
    [SerializeField] private UIDebugDisplayLine MPosCell; 
    [SerializeField] private UIDebugDisplayLine PlayerPosWorld; 
    [SerializeField] private UIDebugDisplayLine PlayerPosCell; 

    // private void Update()
    // {
    //     ToggleDisplay();
    // }

    public void ToggleDisplay()
    {
        Enabled = !Enabled;

        if (Enabled)
        {
            debugCanvas.enabled = true;
        }
        else
        {
            debugCanvas.enabled = false;
        }
    }

    // SETTERS
    public void SetMPosScreen(Vector2 screen) => MPosScreen.Set(screen.ToString());
    public void SetMPosWorld(Vector3 world) => MPosWorld.Set(world.ToString());
    public void SetMPosCell(Vector3Int cell) => MPosCell.Set(cell.ToString());
    public void SetPlayerPosWorld(Vector3 pos) => PlayerPosWorld.Set(pos.ToString());
    public void SetPlayerPosCell(Vector3Int pos) => PlayerPosCell.Set(pos.ToString());
}
