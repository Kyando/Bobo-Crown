using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitGame : MonoBehaviour
{
    public Button button;
    void Start()
    {
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TaskOnClick();
        }
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene("CG1");
    }
}
