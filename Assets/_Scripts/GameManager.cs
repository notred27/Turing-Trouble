using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Dictionary<See, Do> _transitionTable = new();

    [Header("Visuals")]
    [SerializeField] private TextMeshProUGUI _maxBlocksTMP;
    [SerializeField] private TextMeshProUGUI _maxMovesTMP;
    [SerializeField] private TextMeshProUGUI _numMovesTMP;
    [SerializeField] private TextMeshProUGUI _levelNameTMP;

    [Header("See Do Blocks")]
    public List<SeeDo> Blocks = new();
    [SerializeField] private Button _runButton;

    [Header("Store")]
    [SerializeField] private TextMeshProUGUI _coinText;
    private int _numCoins = 0;

    [Header("Tape")]
    private Level Level;
    [SerializeField] private Transform _topPos;
    [SerializeField] private Transform _midPos;
    [SerializeField] private Transform _botPos;
    private readonly float _offset = 1.5f;


    //Local level tapes
    public List<Tile> TopTape;
    public List<Tile> MidTape;
    public List<Tile> BotTape;

    public int MaxMoves;
    public int MaxBlocks;

    private void Awake()
    {
        //Stop any duplicates
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _runButton.onClick.AddListener(() => StartCoroutine(Run()));
        //Run button not interactable
        _runButton.interactable = false;
    }

    private void Start()
    {
        Level = SceneLoader.Instance.level;
        InitLevel(Level);
        Store.Instance.Init(Level);
        AddToCoins(Level.Budget);
        MaxMoves = Level.MaxMoves;
        MaxBlocks = Level.MaxBlocks;
        _levelNameTMP.text = Level.LevelName;
        _maxBlocksTMP.text = "Max Blocks: " + MaxBlocks;
        _maxMovesTMP.text = "Max Moves: " + MaxMoves;
        _numMovesTMP.text = "Num Moves: 0";
    }

    private IEnumerator Run()
    {

        Player.Instance.isRunning = true;
        _runButton.interactable = false;
        
        //cache vals
        Tile startTile = Player.Instance.GetCurrentTile();
        TapeLayer layer = Player.Instance.TapeLayer;
        int index = Player.Instance.index;

        //Make our transition table from the blocks
        foreach(SeeDo block in Blocks)
        {
            _transitionTable[block.SeeType] = block.DoType;
        }

        //Add transitions on arrows we see
        _transitionTable[See.DOWN] = Do.DOWN;
        _transitionTable[See.UP] = Do.UP;
        _transitionTable[See.LEFT] = Do.LEFT;
        _transitionTable[See.RIGHT] = Do.RIGHT;

        for(int i = 0; i < MaxMoves; i++){
            yield return StartCoroutine(Player.Instance.Move(_transitionTable));
            int sum = i + 1;
            _numMovesTMP.text = "Num Moves: " + sum;


            if(!Player.Instance.isRunning)
            {
                //Check if player is on win
                if(Player.Instance.GetCurrentTile().TileInteractable.SeeType == See.WIN){

                    //Player wins
                    Debug.Log("Player has won");
                    PromptManager.Instance.PopUpText("You Win!");
                } else {

                    //player loses
                    PromptManager.Instance.PopUpText("You Lose! \nTry Again?");

                    //reset player position to the beginning
                    Player.Instance.TapeLayer = layer;
                    Player.Instance.index = index;
                    yield return StartCoroutine(Player.Instance.MoveTo(startTile));
                    CheckRunButton();
                }
                yield break;
            }
             
        }



        Player.Instance.isRunning = false;
    }


    public void CheckRunButton()
    {
        foreach(SeeDo block in Blocks)
        {
            if (!block.IsFilled()) { _runButton.interactable = false; return; }
        }

        _runButton.interactable = true;
    }

    private void StartTape(List<Tile> localList, List<Tile> levelList, Vector3 position, TapeLayer layer)
    {

        for (int i = 0; i < levelList.Count; i++)
        {
            if (levelList[i] != null)
            {
                //Instatiate the tile
                Tile tile = levelList[i];
                localList[i] = Instantiate(tile, position + new Vector3(i * _offset, 0, 0), Quaternion.identity, transform);

                if (localList[i].TileInteractable != null)
                {
                    Interactable interactable = Instantiate(localList[i].TileInteractable, position + new Vector3(i * _offset, 0, 0), Quaternion.identity, localList[i].transform);
                    interactable.TapeLayer = layer;
                    interactable.index = i;
                }
                else
                {
                    Debug.Log("Should have interactable in Tile at " + i + " " + layer);
                }

                //instatite player if there
                if (localList[i].TilePlayer != null)
                {
                    Player player = Instantiate(localList[i].TilePlayer, position + new Vector3(i * _offset, 1, 0), Quaternion.identity, localList[i].transform);
                    player.TapeLayer = layer;
                    player.index = i;
                }
            }
        }
    }

    private void InitLevel(Level level)
    {
        //Set the size of our current tapes
        TopTape = new(level.TopTape);
        MidTape = new(level.MidTape);
        BotTape = new(level.BotTape);

        StartTape(TopTape, level.TopTape, _topPos.position, TapeLayer.TOP);
        StartTape(MidTape, level.MidTape, _midPos.position, TapeLayer.MID);
        StartTape(BotTape, level.BotTape, _botPos.position, TapeLayer.BOT);
    }

    public bool CanBuy(int cost)
    {
        return cost <= _numCoins;
    }

    public int AddToCoins(int num)
    {
        _numCoins += num;
        _coinText.text = _numCoins.ToString(); //update the texts
        return _numCoins;
    }

}

public enum See {
    Player = 0,
    Enemy = 1,
    One = 2, 
    Zero = 3,
    Coin = 4,
    Nothing =5,
    WIN = 6,
    DOWN = 7,
    UP = 8,
    LEFT = 9,
    RIGHT = 10,
}

public enum Do 
{ 
    UP = 0,
    DOWN = 1,
    LEFT = 2,
    RIGHT = 3,
    FIGHT = 4,
    NOTHING = 5,
}

public enum TapeLayer
{
    TOP = 0,
    MID = 1,
    BOT = 2
}