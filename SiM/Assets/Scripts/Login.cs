using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour {

    public UnityEngine.UI.InputField nameField;
    public UnityEngine.UI.InputField idField;

    public UnityEngine.UI.Text errorText;

    public void OnLogin()
    {
        if (!string.IsNullOrEmpty(nameField.text) && !string.IsNullOrEmpty(idField.text))
        {
            int id;
            if (int.TryParse(idField.text, out id))
            {
                ScoreManager.NewPlayer(nameField.text.ToUpper(), id);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
            }
            else
                errorText.text = "Erro: Numero USP invalido";
        }
        else
            errorText.text = "Erro: Campo vazio";
    }
}
