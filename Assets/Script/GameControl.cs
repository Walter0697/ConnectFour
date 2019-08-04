using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public string gamestate = "menu";
    public GameObject menuPanel;
    public GameObject endgamePanel;
    private GeneratePiece generator;

    public Text removeButtonText;
    public Text winningText;
    private KeyCode restartCode = KeyCode.R;

    // Start is called before the first frame update
    void Start()
    {
        generator = GetComponent<GeneratePiece>();
        endgamePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gamestate == "endgame")
        {
            if (Input.GetKeyDown(restartCode))
            {
                generator.clearBoard();
                gamestate = "menu";
                endgamePanel.SetActive(false);
                menuPanel.SetActive(true);
            }
        }
    }

    private void toggleMenuPanel(bool active)
    {
        menuPanel.SetActive(active);
    }

    private void toggleEndPanel(bool active)
    {
        endgamePanel.SetActive(active);
    }

    public void startGame()
    {
        toggleMenuPanel(false);
        gamestate = "gameplay";
    }

    public void endGame(int winner)
    {
        toggleEndPanel(true);
        gamestate = "endgame";
        if (winner == 1)
        {
            winningText.text = "you win!";
        }
        else if (winner == 2)
        {
            winningText.text = "computer wins!";
        }
        else
        {
            winningText.text = "It is a tie!";
        }
    }

    public void removeButton()
    {
        generator.toggleRemove();
        if (generator.allowRemove)
        {
            removeButtonText.text = "allow remove";
        }
        else
        {
            removeButtonText.text = "not allow remove";
        }
    }

    public void endGame()
    {
        toggleMenuPanel(true);
        gamestate = "menu";
    }
}
