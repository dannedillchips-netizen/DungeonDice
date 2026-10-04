using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

        public string SceneName;
        
        public void NextScene()
        {
            SceneManager.LoadScene(SceneName);
        }
}
