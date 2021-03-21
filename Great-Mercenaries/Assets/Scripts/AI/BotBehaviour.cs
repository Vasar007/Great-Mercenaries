using System.Collections;
using UnityEngine;
using GreatMercenaries.Assets.Scripts.Core;
using GreatMercenaries.Assets.Scripts.Core.Cards;
using GreatMercenaries.Assets.Scripts.UI;

namespace GreatMercenaries.Assets.Scripts.AI
{
    public class BotBehaviour : MonoBehaviour
    {
        public DeckOfCards botDeck;
        public string botTabletopTag = "EnemyTabletop";

        [SerializeField]
        private GameManager _gameManager;

        private Transform _botTabletop;
        private GameObject _placeholder;
        private Vector3 _goal;
        private bool _stepInProcess;
        private int _usedCard;
        private bool _firstStep = true;
        private bool _needToDelay = true;


        private void Start()
        {
            // Find bot tabletop for moving card on it.
            _botTabletop = GameObject.FindGameObjectWithTag(botTabletopTag).transform;

            Invoke("DealCards", 0.5f);

            // Define used card.
            _usedCard = 0;
            //_usedCard = Random.Range(0, BotHand.Count);
        }
    
        private void Update()
        {
            // Check if game scene is loading or not.
            if (LoadingManager.currentAppState == LoadingManager.AppState.Loading) return;

            // Additional delay to avoid quick first bot step if bot makes step first.
            if (_firstStep)
            {
                StartCoroutine(MakeDelay(0.5f));
                _firstStep = false;
                return;
            }

            // Additional delay flag.
            if (_needToDelay) return;

            // Check whose turn and if game is not over.
            if (_gameManager.isPlayerTurn 
                || _gameManager.currentGameState == GameManager.GameState.GameOver) return;

            // If bot has already started own turn.
            if (!_stepInProcess)
            {
                _stepInProcess = true;

                // Calculate new position of card to move.
                _placeholder = new GameObject();
                _placeholder.transform.SetParent(_botTabletop);

                // Goal vector for moving.
                _goal = new Vector3(_placeholder.transform.position.x,
                                    _placeholder.transform.position.y + 1.4f,
                                    _botTabletop.transform.position.z);

                botDeck.Hand[_usedCard].transform.SetParent(_botTabletop.parent);
            }
       
            // Make movement between card start position and new calculated position.
            botDeck.Hand[_usedCard].transform.position =
                Vector3.MoveTowards(botDeck.Hand[_usedCard].transform.position, _goal, 0.5f);

            // If card moving was ended
            if ((botDeck.Hand[_usedCard].transform.position - _goal).sqrMagnitude < 0.05f)
            {
                Debug.Log("Hey, I made my step!");
                _stepInProcess = false;

                // Make final actions.
                botDeck.Hand[_usedCard].transform.SetParent(_botTabletop);
                _gameManager.battleManager.SetCardForBattle(botDeck.Hand[_usedCard], false);
                ++_usedCard;
                if (_usedCard == botDeck.Hand.Count)
                {
                    _usedCard = 0;
                }

                // Destroy temporary object.
                Destroy(_placeholder);

                // Send message to manager that bot made step.
                _gameManager.MakeStep();
            }
        }

        private IEnumerator MakeDelay(float timeDelay)
        {
            float time = 0.0f;
            while (time < timeDelay)
            {
                time += Time.deltaTime;
                yield return null;
            }

            _needToDelay = false;
        }

        private void DealCards()
        {
            for (int i = 0; i < _gameManager.raundsLimit; ++i)
            {
                botDeck.MoveDealtCard();
            }
        }

        public void MoveCardToTabletop()
        {
        
        }
    }
}
