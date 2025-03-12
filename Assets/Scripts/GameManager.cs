using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Broadcast Events")]
    [SerializeField] private VoidEventSO gameStartEventSO;

    [Header("ListenEvents")]
    [SerializeField] private VoidEventSO gameOverEventSO;

    public UnityEvent onGameEnd;


    private void Awake()
    {
        gameStartEventSO.RaiseEvent();
    }

    private void OnEnable()
    {
        gameOverEventSO.OnEventRaised += EndGame;
    }

    private void OnDisable()
    {
        gameOverEventSO.OnEventRaised -= EndGame;
    }

    private void EndGame()
    {
        onGameEnd?.Invoke();
    }
}
