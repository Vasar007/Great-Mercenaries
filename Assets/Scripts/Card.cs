using UnityEngine;

public class Card : MonoBehaviour
{
    public int healthPoints;
    public int damage;

    [SerializeField]
    private string _valueMessage;

    private bool _hasAnyChages;


    // Use this for initialization
    private void Start()
    {
        //ActionBroadcast();
    }

    // Update is called once per frame
    private void Update()
    {
        if (LoadingManager.currentAppState == LoadingManager.AppState.Loading) return;

        if (_hasAnyChages)
        {
            ActionBroadcast(_valueMessage);
            _hasAnyChages = false;
        }
    }

    public void ReceiveDamege(int receivedDamage)
    {
        if (IsAlive())
        {
            healthPoints -= damage;

            if (healthPoints <= 0)
            {
                healthPoints = 0;
            }

            _hasAnyChages = true;
        }
    }

    public bool IsAlive()
    {
        return healthPoints > 0;
    }

    public void ActionBroadcast(string message)
    {
        _valueMessage = message;
        Messenger.Broadcast<MonoBehaviour, string>(_valueMessage, this,
                                                   damage + "/" + healthPoints);
    }

    public void SetValueMessage(string message)
    {
        _valueMessage = message;
    }

    public string GetValueMessage()
    {
        return _valueMessage;
    }
}
