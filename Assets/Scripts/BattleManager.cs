using System;
using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
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

    public bool MakeBattle(bool isPlayerStep)
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

    private bool ProcessFight(Card card1, Card card2)
    {
        while (card1.IsAlive() && card2.IsAlive())
        {
            card2.ReceiveDamege(card1.damage);
            card1.ReceiveDamege(card2.damage);

            Debug.Log("In action: " + card1.healthPoints + " vs. " + card2.healthPoints);
        }

        _isNeedToFight = false;

        return card1.IsAlive();
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
