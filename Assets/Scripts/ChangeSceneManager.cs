using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour {

    public IEnumerator goToMenu() {   
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("menu");
    }
}