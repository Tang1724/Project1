using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelFlow
{
    public static string CurrentLevelName => SceneManager.GetActiveScene().name;

    public static void RestartGame()
    {
        ResumeGame();
        SceneManager.LoadScene(CurrentLevelName);
    }

    public static void LoadPreviousLevel()
    {
        ResumeGame();
        SceneManager.LoadScene(Mathf.Max(SceneManager.GetActiveScene().buildIndex - 1, 0));
    }

    public static void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("No more levels!");
            return;
        }

        ResumeGame();
        SceneManager.LoadScene(nextSceneIndex);
    }

    public static void StartGame()
    {
        ResumeGame();
        SceneManager.LoadScene("Test1.1");
        if (AudioManager.instance != null)
            AudioManager.instance.StartMusic();
    }

    public static void LoadMainMenu()
    {
        ResumeGame();
        if (AudioManager.instance != null)
            AudioManager.instance.StopMusic();
        SceneManager.LoadScene("MainScene");
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    private static void ResumeGame()
    {
        if (GameModeManager.Instance != null)
            GameModeManager.Instance.ChangeMode(GameMode.Playing);
    }
}
