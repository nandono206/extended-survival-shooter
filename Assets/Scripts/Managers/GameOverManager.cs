using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;       
    public float restartDelay = 5f;  
    public Button retryButton;        


    Animator anim;                          
    float restartTimer;                    


    void Awake()
    {
        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("GameOver");

            restartTimer += Time.deltaTime;

            if (restartTimer >= restartDelay)
            {
                // If more than 5 seconds, redirect to main menu
                SceneManager.LoadScene("Main_Menu", LoadSceneMode.Single);
            }
        }
    }
}