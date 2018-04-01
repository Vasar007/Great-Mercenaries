using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    // Time to wait before exit the level.
    public float exitDelay = 5.0f;

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private string _gameOverTextTag = "GameOverText";

    // Reference to the animator component.
    private Animator _animator;
    // Timer to count up to exit the level.
    private float _exitTimer;

    private bool _firstStep = true;
    private bool _needToDelay = true;

    private void Awake()
    {
        // Set up the reference.
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_gameManager.currentGameState != GameManager.GameState.GameOver) return;

        // Additional delay to avoid quick first bot step if bot makes step first.
        if (_firstStep)
        {
            StartCoroutine(MakeDelay(0.5f));
            _firstStep = false;
            return;
        }

        if (_needToDelay) return;

        if (_gameManager.currentGameState == GameManager.GameState.GameOver)
        {
            // If game is over.
            GameObject.FindGameObjectWithTag(_gameOverTextTag).GetComponent<Text>().text =
                _gameManager.IsPlayerWon() ? "You won!" : "You lose!";

            // Tell the animator the game is over.
            _animator.SetTrigger("GameOver");

            // Increment a timer to count up to exit.
            _exitTimer += Time.deltaTime;

            // If it reaches the exit delay.
            if (_exitTimer >= exitDelay)
            {
                // Then exit to main menu.
                UnityEngine.SceneManagement.SceneManager.LoadScene(
                    PlayerPrefs.GetString("LastScene"));
            }
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
}
