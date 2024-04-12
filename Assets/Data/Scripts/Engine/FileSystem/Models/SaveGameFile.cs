using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveGameFile
{
    public int SavePosition { get; set; }
    public string Chapter { get; set; }
    public int Checkpoint { get; set; }
    public DateTime LastSaveDate { get; set; }

    public SaveGameFile(int savePosition)
    {
        SavePosition = savePosition;
        Chapter = "Prologue";
        Checkpoint = 0;
        LastSaveDate = DateTime.Now;
    }
}
