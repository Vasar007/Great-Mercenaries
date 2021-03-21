using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public DeckOfCards playerDeck;

    //public string playerTabletopTag = "PlayerTabletop";

    [SerializeField]
    private GameManager _gameManager;

    //private Transform _playerTabletop;
    private bool _stepInProcess;


    private void Start()
    {
        // Find bot tabletop for moving card on it.
        //_playerTabletop = GameObject.FindGameObjectWithTag(playerTabletopTag).transform;

        Invoke("DealCards", 0.5f);
    }

    private void Update()
    {
        // Check if game scene is loading or not.
        if (LoadingManager.currentAppState == LoadingManager.AppState.Loading) return;

        // Check whose turn and if game is not over.
        if (!_gameManager.isPlayerTurn
            || _gameManager.currentGameState != GameManager.GameState.IsPlaying) return;

        if (!_stepInProcess)
        {
            _stepInProcess = true;
            ChangeAbilityToDrag(true);
        }

        if (CheckTransition())
        {
            _stepInProcess = false;
        }

        if (!_stepInProcess)
        {
            ChangeAbilityToDrag(false);

            _gameManager.MakeStep();
        }
    }

    private void DealCards()
    {
        for (int i = 0; i < _gameManager.raundsLimit; ++i)
        {
            playerDeck.MoveDealtCard();
        }
    }

    private void ChangeAbilityToDrag(bool flag)
    {
        foreach (var card in playerDeck.Hand)
        {
            if (card != null)
            {
                card.GetComponent<Draggable>().isDragable = flag;
            }
        }
    }

    private bool CheckTransition()
    {
        return _gameManager.urgentNotify;
    }
}
