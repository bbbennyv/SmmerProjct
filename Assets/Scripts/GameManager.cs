using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI Ref")]
    [SerializeField] private GameObject startUI;
    /*[SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject upgradeUI;*/

    private State currentState;

    //public GameObject PauseUI => pauseUI;
    //public GameObject GamePlayUI => gameUI;
    public GameObject StartUI => startUI;
   // public GameObject UpgradeUI => upgradeUI;

    private GameplayState gameplayState = new GameplayState();
    private PauseState pauseState = new PauseState();
    private StartState startState = new StartState();
    private UpgradeState upgradeTState = new UpgradeState();

    public static GameManager Instance;

    public bool IsGameplay => currentState is GameplayState;

    //public bool IsPaused => currentState is PauseState;
    //public State CurrentState => currentState;

    private float startTimer = 3;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetState(startState);
    }



    private void Update()
    {
        if (PlayerInputManager.instance.gameStartable)
        {

            startTimer -= Time.deltaTime;
        }

        if (startTimer <= 0)
        {
            SetState(gameplayState);
            Debug.Log("GAMEPLAY");
        }

        if(currentState == gameplayState)
        {
            Debug.Log("WON");
        }

    }

    public void SetState(State newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;
        currentState.Enter(this);
        Debug.Log($"current state -{currentState.ToString()}");
    }


    //if need be these are here
    /*public void GoToGameplay()
    {
        SetState(gameplayState);
    }

    public void GoToPause()
    {
        SetState(pauseState);
    }

  */

    public void TogglePause()
    {
        if (currentState is PauseState)
            SetState(gameplayState);
        else if (currentState is GameplayState)
            SetState(pauseState);
    }

    public void GoToGameplay()
    {
        SetState(gameplayState);
    }


}
