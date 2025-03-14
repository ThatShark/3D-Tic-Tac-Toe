using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public Button startButton;
    public Button stasticButton;
    public Button quitButton;

    void Start()
    {
        startButton.onClick.AddListener(startButtonOnClick);
        stasticButton.onClick.AddListener(stasticButtonOnClick);
        quitButton.onClick.AddListener(quitButtonOnClick);
    }


    void startButtonOnClick() {
        Debug.Log("Start Button Clicked");
    }

    void stasticButtonOnClick() {
        Debug.Log("Stastic Button Clicked");
    }
    void quitButtonOnClick() {
        Debug.Log("Quit Button Clicked");
    }
    void Update()
    {
        
    }
}
