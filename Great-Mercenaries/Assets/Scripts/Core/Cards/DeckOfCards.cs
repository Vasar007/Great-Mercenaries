using System.Collections.Generic;
using UnityEngine;
using GreatMercenaries.Assets.Scripts.UI;

namespace GreatMercenaries.Assets.Scripts.Core.Cards
{
    public class DeckOfCards : MonoBehaviour
    {
        public List<GameObject> deck = new List<GameObject>();

        public string panelTag = "Tag";

        public List<GameObject> Cards { get { return _cards; } }
        private List<GameObject> _cards = new List<GameObject>();

        public List<GameObject> Hand { get { return _hand; } }
        private List<GameObject> _hand = new List<GameObject>();

        private int _cardsDealt;
        private bool _showReset;

        private Transform _tabletop;


        private void Start()
        {
            _tabletop = GameObject.FindGameObjectWithTag(panelTag).transform;

            for (int i = 0; i < deck.Count; ++i)
            {
                string message = panelTag + "CardStats" + i;

                var card = deck[i].GetComponent<Card>();
                var cardText = deck[i].GetComponentInChildren<TextBinding>();

                cardText.CreateListener(message);
                card.ActionBroadcast(message);

                Debug.Log("Created message: " + message);
            }

            ResetDeck();
        }

        //private void OnGUI()
        //{
        //    if (!_showReset)
        //    {
        //        // Deal button.
        //        if (GUI.Button(new Rect(10, 10, 200, 40), "Deal"))
        //        {
        //            MoveDealtCard();
        //        }
        //    }
        //    else
        //    {
        //        // Reset button.
        //        if (GUI.Button(new Rect(10, 10, 200, 40), "Reset"))
        //        {
        //            ResetDeck();
        //        }
        //    }
        //    // GameOver button.
        //    if (GUI.Button(new Rect(Screen.width - 210, 10, 200, 40), "GameOver"))
        //    {
        //        GameOver();
        //    }
        //}

        public void ResetDeck()
        {
            _cardsDealt = 0;
            foreach (var card in _hand)
            {
                Destroy(card);
            }
            _hand.Clear();
            _cards.Clear();
            _cards.AddRange(deck);
            _showReset = false;
        }

        private GameObject DealCard()
        {
            if (_cards.Count == 0)
            {
                _showReset = true;
                return null;

                // Alternatively to auto reset the deck:
                //ResetDeck();
            }

            //int index = Random.Range(0, _cards.Count - 1);

            int index = _cards.Count - 1;
            GameObject dealCard = Instantiate(_cards[index]);

            string message = panelTag + "CardStats" + index;
            Debug.Log(message + " Size: " + _cards.Count);
            dealCard.GetComponentInChildren<TextBinding>().CreateListener(message);
            dealCard.GetComponent<Card>().ActionBroadcast(message);

            _cards.RemoveAt(index);

            if (_cards.Count == 0)
            {
                _showReset = true;
            }

            return dealCard;
        }

        private void GameOver()
        {
            var temp = _showReset;
            ResetDeck();
            _showReset = temp;
        }

        public void MoveDealtCard()
        {
            GameObject newCard = DealCard();
            // Check card is null or not.
            if (newCard == null)
            {
                Debug.Log("Out of Cards");
                _showReset = true;
                return;
            }

            // Place card on tabletop.
            newCard.transform.SetParent(_tabletop);
            newCard.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);

            Debug.Log("Deal card to " + _tabletop.name);
            Debug.Log("Listen: " + newCard.GetComponent<Card>().GetValueMessage() + " " + newCard.GetComponentInChildren<TextBinding>().GetValueMessage());

            // Add card to hand.
            _hand.Add(newCard);
            ++_cardsDealt;
        }
    }
}
