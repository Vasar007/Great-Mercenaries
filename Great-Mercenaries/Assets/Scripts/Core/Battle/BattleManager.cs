using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using GreatMercenaries.Assets.Scripts.Core.Cards;
using GreatMercenaries.Assets.Scripts.UI;

namespace GreatMercenaries.Assets.Scripts.Core.Battle
{
    public class BattleManager : MonoBehaviour
    {
        public enum BattleResult
        {
            NotEnd,
            PlayerWon,
            EnemyWon
        }

        public GameObject playerCard;
        public GameObject enemyCard;

        private bool _isNeedToFight;
        private bool _hasPlayerCard;
        private bool _hasEnemyCard;
        private bool _needToDelay = true;


        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            if (LoadingManager.currentAppState == LoadingManager.AppState.Loading) return;

            if (!_needToDelay)
            {
                _needToDelay = true;

                Thread.Sleep(1000);
                Destroy(playerCard);
                Destroy(enemyCard);
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

        public BattleResult MakeBattle(bool isPlayerStep)
        {
            if (!_isNeedToFight)
            {
                throw new ArgumentNullException();
            }

            var player = playerCard.GetComponent<Card>();
            var enemy = enemyCard.GetComponent<Card>();

            // True if player won, false otherwise.
            return isPlayerStep ? ProcessFight(player, enemy) : ProcessFight(enemy, player);
        }

        private BattleResult ProcessFight(Card card1, Card card2)
        {
            if (card1.IsAlive())
            {
                card2.ReceiveDamage(card1.damage);
                if (card2.IsAlive())
                {
                    card1.ReceiveDamage(card2.damage);
                }

                Thread.Sleep(500);
                Debug.Log("In action: " + card1.healthPoints + " vs. " + card2.healthPoints);
            }

            _isNeedToFight = card1.IsAlive() && card2.IsAlive();
            return _isNeedToFight ? BattleResult.NotEnd
                                  : card1.IsAlive() ? BattleResult.PlayerWon : BattleResult.EnemyWon;
        }

        public void SetCardForBattle(GameObject card, bool isPlayer)
        {
            if (isPlayer)
            {
                playerCard = card;
                _hasPlayerCard = true;
            }
            else
            {
                enemyCard = card;
                _hasEnemyCard = true;
            }

            if (_hasPlayerCard && _hasEnemyCard)
            {
                _isNeedToFight = true;
            }
        }

        public void DestroyCards()
        {
            StartCoroutine(MakeDelay(1.0f));

            _hasPlayerCard = false;
            _hasEnemyCard = false;
        }
    }
}
