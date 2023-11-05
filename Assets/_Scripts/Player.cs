using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Player : ObjectWrapper
{
    public static Player Instance;
    [SerializeField] private State PlayerState;

    [Header("Tweening Vars")]
    private float _tweenDuration = 0.7f;
    [SerializeField] private Ease _ease;

    public bool isRunning = false;

    private void Awake()
    {
        //Stop any duplicates
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            PlayerState = State.normal;

        }
    }

    public IEnumerator Move(Dictionary<See, Do> transitionTable)
    {
        Tile currentTile = GetCurrentTile();

        if(!transitionTable.ContainsKey(currentTile.TileInteractable.SeeType)) {
            isRunning = false;
            yield break;
        }
      
        //See what we are currently on, if see matches what we are on
        if(currentTile.TileInteractable != null) 
        {
            
            switch (transitionTable[currentTile.TileInteractable.SeeType]) 
            {
                case Do.UP:  yield return StartCoroutine(Up()); break;
                case Do.DOWN: yield return StartCoroutine(Down()); break;
                case Do.LEFT: yield return StartCoroutine(Left()); break;
                case Do.RIGHT: yield return StartCoroutine(Right()); break;
                default: isRunning = false; break;
            }

            Tile newTile = GetCurrentTile();
            if(newTile.TileInteractable != null)
            {
                newTile.TileInteractable.gameObject.SetActive(true);
                newTile.TileInteractable.OnInteract(PlayerState);  //Call an interaction with the tile you land on (if its not null)
               
            }
        } 
    }
    private IEnumerator Up()
    {
        switch(TapeLayer)
        {
            case TapeLayer.TOP: isRunning = false; break;
            case TapeLayer.MID: yield return StartCoroutine(MoveTo(GameManager.Instance.TopTape[index])); TapeLayer = TapeLayer.TOP; break;
            case TapeLayer.BOT: yield return StartCoroutine(MoveTo(GameManager.Instance.MidTape[index])); TapeLayer = TapeLayer.MID; break;
        }
    }

    private IEnumerator Down()
    {
        switch (TapeLayer)
        {
            case TapeLayer.TOP: yield return StartCoroutine(MoveTo(GameManager.Instance.MidTape[index])); TapeLayer = TapeLayer.MID; break;
            case TapeLayer.MID: yield return StartCoroutine(MoveTo(GameManager.Instance.BotTape[index])); TapeLayer = TapeLayer.BOT; break;
            case TapeLayer.BOT: isRunning = false; break;
        }
    }

    private IEnumerator Right()
    {
        switch (TapeLayer)
        {
            case TapeLayer.TOP:
                if (index + 1 <= GameManager.Instance.TopTape.Count)
                {
                    yield return StartCoroutine(MoveTo(GameManager.Instance.TopTape[index + 1]));
                    index++;
                }
                else
                {
                    isRunning = false;
                }
                break;
            case TapeLayer.MID:
                if (index + 1 <= GameManager.Instance.MidTape.Count)
                {
                    yield return StartCoroutine(MoveTo(GameManager.Instance.MidTape[index + 1]));
                    index++;
                }
                else
                {
                    isRunning = false;
                }
                break;
            case TapeLayer.BOT:
                if (index + 1 <= GameManager.Instance.BotTape.Count)
                {
                    yield return StartCoroutine(MoveTo(GameManager.Instance.BotTape[index + 1]));
                    index++;
                    
                }
                else
                {
                    isRunning = false;
                }
                break;
        }
    }

    private IEnumerator Left()
    {
        //Checking that index is valid
        if(index - 1 < 0) { isRunning = false; yield break; }

        switch (TapeLayer)
        {
            case TapeLayer.TOP:
                    yield return StartCoroutine(MoveTo(GameManager.Instance.TopTape[index - 1]));
                    index--;
                break;
            case TapeLayer.MID:
                    yield return StartCoroutine(MoveTo(GameManager.Instance.MidTape[index - 1]));
                    index--;
                break;
            case TapeLayer.BOT:
                    yield return StartCoroutine(MoveTo(GameManager.Instance.BotTape[index - 1]));
                    index--;
                break;
        }
    }

    /// <summary>
    /// Moves to the tile and waits for completion
    /// </summary>
    /// <param name="tile">The location we want to move to</param>
    public IEnumerator MoveTo(Tile tile)  //TODO update this to return false in some cases??
    {
        //Clear our current tile
        Tile curr = GetCurrentTile();
        curr.TilePlayer = null;

        //Set the move to tile
        tile.TilePlayer = this;

        //Tween ourselves there
        Tween move = transform.DOMove(tile.transform.position + new Vector3(0, 1, 0), _tweenDuration).SetEase(_ease);
        yield return move.WaitForCompletion();
    }

    public Tile GetCurrentTile()
    {
        switch (TapeLayer)
        {
            case TapeLayer.TOP: return GameManager.Instance.TopTape[index]; 
            case TapeLayer.MID: return GameManager.Instance.MidTape[index];
            case TapeLayer.BOT: return GameManager.Instance.BotTape[index];
            default: Debug.Log("Failed to find cur tile in Player");  return null;
        }
    }
    
    public void ChangeState(State newState)
    {
        PlayerState = newState;
    }
}




public enum State
{
    normal = 0,
    fight = 1,
    mobility = 2,
}


