using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        IsPlaying,
        GameOver,
        ActionBattle
    }

    public int raundsLimit = 6;
    public int neededToWin = 4;
    public bool isPlayerTurn;
    public int raundNumber = 1;
    public GameState currentGameState;

    public BattleManager battleManager;

    public bool urgentNotify;

    private int _numberOfMadeSteps;
    private int _wonsRaunds;
    private BattleManager.BattleResult _fightResult;
    private bool _processingFight;
    private bool _fightEnded;


    private void Start()
    {
        // Define who make first step.
        //IsPlayerTurn = Random.Range(0, 2) == 1;
        _numberOfMadeSteps = 0;

        // Start game phase.
        currentGameState = GameState.IsPlaying;
    }
    
    private void Update()
    {
        if (LoadingManager.currentAppState == LoadingManager.AppState.Loading) return;

        if (_processingFight)
        {
            _fightResult = battleManager.MakeBattle(isPlayerTurn);

            if (_fightResult != 0)
            {
                _processingFight = false;
                _fightEnded = true;
            }
        }

        if (_fightEnded)
        {
            _fightEnded = false;
            Debug.Log((_fightResult == BattleManager.BattleResult.PlayerWon ? "Player" : "Bot") + " won round!");
            _wonsRaunds += _fightResult == BattleManager.BattleResult.PlayerWon ? 1 : 0;

            battleManager.DestroyCards();
            ++raundNumber;
            currentGameState = GameState.IsPlaying;

            if (raundNumber > raundsLimit)
            {
                // If number of rounds greater than raunds limit game is over.
                currentGameState = GameState.GameOver;
                Debug.Log("Game over!");
            }
        }
    }

    public void MakeStep()
    {
        // Check if game is not over.
        if (currentGameState == GameState.GameOver) return;

        // Switch flag and increase step counter.
        isPlayerTurn ^= true;
        ++_numberOfMadeSteps;
        urgentNotify = false;

        // If counter is odd then round is not end.
        if (_numberOfMadeSteps % 2 != 0) return;

        // Else make fight and increase round cunter.
        if (raundNumber <= raundsLimit)
        {
            currentGameState = GameState.ActionBattle;
            _processingFight = true;
        }
    }

    public bool IsPlayerWon()
    {
        return raundsLimit - _wonsRaunds < neededToWin;
    }

    public static IEnumerator MakeDelay(float timeDelay)
    {
        float time = 0.0f;
        while (time < timeDelay)
        {
            time += Time.deltaTime;
            yield return null;
        }
    }
}
