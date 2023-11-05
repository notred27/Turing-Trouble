using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Levels", menuName = "ScriptableObjects/Level")]
public class Level : ScriptableObject
{
    public string LevelName;

    [Header("Tape Containers")]

    public List<Tile> TopTape;
    public List<Tile> MidTape;
    public List<Tile> BotTape;


    [Header("Store Variables")]

    public List<DragAndDrop> SeeItemList;
    public List<int> SeeCostList;

    public List<DragAndDrop> DoItemList;
    public List<int> DoCostList;

    
    [Header("Limits")]
    public int Budget;
    public int MaxMoves;
    public int MaxBlocks;

}


