using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDebugDisplay : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool Enabled;

    [Header("References")]
    [SerializeField] private Canvas debugCanvas;

    [SerializeField] private UIDebugDisplayLine MPosScreen; 
    [SerializeField] private UIDebugDisplayLine MPosWorld; 
    [SerializeField] private UIDebugDisplayLine MPosCell; 
    [SerializeField] private UIDebugDisplayLine PlayerPos; 


    private void Update()
    {
        ToggleDisplay();
    }

    private void ToggleDisplay()
    {
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
    public void SetPlayerPos(Vector3 pos) => PlayerPos.Set(pos.ToString());
}
