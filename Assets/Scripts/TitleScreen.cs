using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private UIOption playOption;
    [SerializeField] private UIOption quitOption;

    private void Start()
    {
        playOption.OnConfirm = () => SceneManager.LoadScene("SampleScene");
        quitOption.OnConfirm = () => Application.Quit();
    }
}
