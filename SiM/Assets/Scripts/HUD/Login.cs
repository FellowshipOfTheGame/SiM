using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class Login : MonoBehaviour
{
    public InputField nameField;
    public InputField idField;
    public Text errorText;

    public string menuScene;

    public void OnLogin()
    {
        if(!string.IsNullOrEmpty(nameField.text) && !string.IsNullOrEmpty(idField.text))
        {
            int id;
            if (int.TryParse(idField.text, out id))
            {
                ScoreManager.NewPlayer(nameField.text, id);
                SceneManager.LoadScene(menuScene);
            }
            else
                errorText.text = "Erro: Numero USP invalido";
        }
        else
            errorText.text = "Erro: Campo vazio";
    }
}
