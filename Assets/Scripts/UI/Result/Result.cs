using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    [SerializeField]
    private GameObject[] buttons;

    public void Lose()
    {
        buttons[0].SetActive(GameManager.Instance.IsResurrection);
        buttons[1].SetActive(true);
        buttons[2].SetActive(true);
    }

    public void Win()
    {
        buttons[0].SetActive(false);
        buttons[1].SetActive(true);
        buttons[2].SetActive(false);
    }

    void Awake()
    {
        UIManager.Instance.ResultUI = this;
    }
}
