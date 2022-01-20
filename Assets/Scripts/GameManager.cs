using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {
    public LanguageManager languageManager;
    public PlayerManager playerManager;
    public ChangeSceneManager changeSceneManager;
    public DialogueManager dialogueManager;
    public options option;
    public AudioClip son_menu;
    public AudioClip son_jeu;
    private string currentScene = "other_scene";
    private bool beingTranslated = false;

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        if (languageManager == null) {
            languageManager = new LanguageManager();
        }

        if (playerManager == null) {
            playerManager = new PlayerManager();
        }

        if (changeSceneManager == null) {
            changeSceneManager = new ChangeSceneManager();
        }

        if (changeSceneManager == null) {
            option = new options();
        }

        if (dialogueManager == null) {
            dialogueManager = new DialogueManager();
        }

        StartCoroutine(changeSceneManager.goToMenu());
    }

    void Update(){
        if (!beingTranslated) {
            StartCoroutine(LoadTranslations());
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private IEnumerator LoadTranslations() {
        beingTranslated = true;
        LoadLanguage();
        yield return new WaitForSeconds(0.5f);
        beingTranslated = false;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        AudioSource audio = GameObject.FindWithTag("musique").GetComponent<AudioSource>();
        if(scene.name.Contains("game_start") && this.currentScene != "game_start") {
            option.loadOptionInGame();
            playerManager.getInventory().loadInventory();
            this.currentScene = "game_start";
            audio.clip = son_jeu;
            audio.volume = PlayerPrefs.GetFloat("MusiqueVolume", 1);
            audio.Play();
            dialogueManager.resetIdDialogue();
            dialogueManager.StartDialogue();
        } else if(scene.name.Contains("options") && this.currentScene != "options") {
            option.setValue();
            this.currentScene = "options";
        } else if(scene.name.Contains("menu") && this.currentScene.Contains("game")) {
            // Si on quitte le jeu pour aller vers le menu
            Destroy(getOption().getCanvasInterface());
            Destroy(this.playerManager.getInventory().getInventoryInterface());
            Destroy(GameObject.FindWithTag("Player"));
            Destroy(GameObject.FindWithTag("canvas_pick_item"));
            this.playerManager.getInventory().clearInventaireClefs();
            this.playerManager.getInventory().clearInventaireObjets();
            this.playerManager.getInventory().clearInventaireJournaux();
            this.currentScene = "menu";
            audio.clip = son_menu;
            audio.volume = PlayerPrefs.GetFloat("MusiqueVolume", 1);
            audio.Play();
        } else if(scene.name.Contains("menu") && this.currentScene != "menu" && this.currentScene != "options") {
            // Si on arrive dans le menu et qu'on etait pas déjà dans le menu ou les options 
            // comme ça on relance pas la musique a chaque fois qu'on change de scene
            audio.clip = son_menu;
            audio.volume = PlayerPrefs.GetFloat("MusiqueVolume", 1);
            audio.Play();
            this.currentScene = "menu";
        } else if(scene.name.Contains("menu") && this.currentScene != "menu"){
            // Si on arrive dans le menu et qu'on etait pas déjà dans le menu
            this.currentScene = "menu";
        } else if(!((scene.name.Contains("game") || scene.name.Contains("options") || scene.name.Contains("menu")))) {
            //Si on est ni dans une scene de jeu ni dans les options ni dans le menu
            this.currentScene = "other scene";
        }
    }

    public string getCurrentScene(){
        return this.currentScene;
    }

    public options getOption(){
        return this.option;
    }

    public PlayerManager getPlayerManager(){
        return this.playerManager;
    }

    public DialogueManager getDialogueManager(){
        return this.dialogueManager;
    }

    public void LoadLanguage() {
        languageManager.LoadLanguage(PlayerPrefs.GetString("lang", "français"));
    }
}
