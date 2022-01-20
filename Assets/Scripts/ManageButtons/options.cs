using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class options : MonoBehaviour {
    public GameObject optionInterface;
    public GameObject canvasInterface;
    private bool inOption;
    private bool starting;
    private List<string> languages;

    void Update() {
        if(starting) {
            setValue();
            starting = false;
        }
        // Si on presse la touche echap + on est dans une scene de jeu
        if(Input.GetKeyDown(KeyCode.Escape) && FindObjectOfType<GameManager>().getCurrentScene().StartsWith("game") == true) {   
            // Si le menu des options est fermé
            if(this.inOption == false) {
                //On ferme l'inventaire si il est ouvert
                if(GameObject.FindWithTag("inventory_interface") != null){
                    FindObjectOfType<Inventory>().onIsClose();
                }
                //On ouvre le menu d'option
                openOption();
            } else {
                // Sinon, on le cache
                closeOption(); 
            }
        }
    }

    // Fonction permettant d'afficher le menu des options en jeu
    public void openOption() {
        // On rend le menu visible
        this.canvasInterface.SetActive(true);
        // On rend le curseur visible
        Cursor.visible = true;
        // On débloque le curseur
        Cursor.lockState = CursorLockMode.None;
        GameObject varGameObject = GameObject.Find("MainCamera");
        // On empeche la camera de suivre le mouvement de la souris
        varGameObject.GetComponent<mouseLook>().enabled = false;
        this.inOption = true;
        // On actualise les valeurs
        setValue();
    }

    // Fonction permettant de fermer le menu des options en jeu
    public void closeOption() {
        // On rend le curseur invisible
        Cursor.visible = false;
        // On bloque le curseur
        Cursor.lockState = CursorLockMode.Locked;
        GameObject varGameObject = GameObject.Find("MainCamera");
        // On permet à la camera de suivre le mouvement de la souris
        varGameObject.GetComponent<mouseLook>().enabled = true;
        // Dans le cas où on revient au jeu depuis les options, il faut modifier l'objet option du gameManager
        FindObjectOfType<GameManager>().getOption().setInOption(false);
        // On rend le menu invisible
        GameObject option = GameObject.FindWithTag("option_interface");
        option.SetActive(false);
    }

    public void setInOption(bool state) {
        this.inOption = state;
    }

    // Fonction permettant d'actualiser les valeurs du menu des options
    public void setValue() {
        languages = new List<string>();
        languages.Add("Français");
        var info = new DirectoryInfo("Assets/Resources/Languages/");
        foreach (FileInfo file in info.GetFiles()) {
            string name = file.Name;
            if (name.EndsWith(".po.txt")) {
                int index = name.IndexOf(".po.txt");
                name = name.Substring(0, index);
                name = char.ToUpper(name[0]) + name.Substring(1);
                languages.Add(name);
            }
        }
        GameObject.FindWithTag("musique_slider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusiqueVolume", 1);
        GameObject.FindWithTag("bruitage_slider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("BruitageVolume", 1);
        GameObject.FindWithTag("sensi_slider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("Sensibility", 200) / 400;
        GameObject.FindWithTag("langage_dropdown").GetComponent<Dropdown>().ClearOptions();
        GameObject.FindWithTag("langage_dropdown").GetComponent<Dropdown>().AddOptions(languages);
        Dropdown dropdown = GameObject.FindWithTag("langage_dropdown").GetComponent<Dropdown>();
        dropdown.value = dropdown.options.FindIndex((i) => {
            string language = PlayerPrefs.GetString("lang", "français");
            return i.text.Equals(char.ToUpper(language[0]) + language.Substring(1));
        });
        GameObject.FindWithTag("input_field_interaction").GetComponent<InputField>().text = PlayerPrefs.GetString("Interaction", "E");
        GameObject.FindWithTag("input_field_inventory").GetComponent<InputField>().text = PlayerPrefs.GetString("Inventory", "I");
        GameObject.FindWithTag("input_field_forward").GetComponent<InputField>().text = PlayerPrefs.GetString("Forward", "Z");
        GameObject.FindWithTag("input_field_backward").GetComponent<InputField>().text = PlayerPrefs.GetString("Backward", "S");
        GameObject.FindWithTag("input_field_left").GetComponent<InputField>().text = PlayerPrefs.GetString("Left", "Q");
        GameObject.FindWithTag("input_field_right").GetComponent<InputField>().text = PlayerPrefs.GetString("Right", "D");
    }
    
    // Fonction permettant de changer le volume de la musique
    public void changeMusiqueVolume() {
        /* AudioSource son = GameObject.FindWithTag("test_musique").GetComponent<AudioSource>(); */
        float volume = gameObject.GetComponent<Slider>().value;
        /* son.volume = volume; */
        PlayerPrefs.SetFloat("MusiqueVolume", volume);
        //On modifie le volume de la musique
        AudioSource audio = GameObject.FindWithTag("musique").GetComponent<AudioSource>();
        audio.volume = PlayerPrefs.GetFloat("MusiqueVolume", 1);
    }

    // Fonction permettant de changer le volume des bruitages
    public void changeBruitageVolume() {
        AudioSource son = GameObject.FindWithTag("test_bruitage").GetComponent<AudioSource>();
        float volume = gameObject.GetComponent<Slider>().value;
        son.volume = volume;
        PlayerPrefs.SetFloat("BruitageVolume", volume);
    }

    // Fonction permettant de changer la sensibilité de la souris
    public void changeSensibility() {
        float sensibility = gameObject.GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("Sensibility", sensibility * 400);
    }

    // Fonction permettant de changer le langage
    public void changeLanguage() {
        Dropdown dropdown = gameObject.GetComponent<Dropdown>();
        PlayerPrefs.SetString("lang", dropdown.options[dropdown.value].text.ToLower());
        FindObjectOfType<GameManager>().LoadLanguage();
    }

    // Fonction permettant de changer la touche d'interaction
    public void changeInputInteraction() {
        string inputInteraction = gameObject.GetComponent<InputField>().text.ToUpper();
        if(inputInteraction == "" || inputInteraction == " ") {
            inputInteraction = "E";
        }
        changeInput("Interaction", inputInteraction);
    }

    // Fonction permettant de changer la touche d'inventaire
    public void changeInputInventory() {
        string inputInventory = gameObject.GetComponent<InputField>().text.ToUpper();
        if(inputInventory == "" || inputInventory == " ") {
            inputInventory = "I";
        }
        changeInput("Inventory", inputInventory);
    }

    // Fonction permettant de changer la touche pour avancer
    public void changeInputForward() {
        string inputForward = gameObject.GetComponent<InputField>().text.ToUpper();
        if(inputForward == "" || inputForward == " ") {
            inputForward = "Z";
        }
        changeInput("Forward", inputForward);
    }

    // Fonction permettant de changer la touche pour reculer
    public void changeInputBackward() {
        string inputBackward = gameObject.GetComponent<InputField>().text.ToUpper();
        if(inputBackward == "" || inputBackward == " ") {
            inputBackward = "S";
        }
        changeInput("Backward", inputBackward);
    }

    // Fonction permettant de changer la touche pour aller à gauche
    public void changeInputLeft() {
        string inputLeft = gameObject.GetComponent<InputField>().text.ToUpper();
        if(inputLeft == "" || inputLeft == " ") {
            inputLeft = "Q";
        }
        changeInput("Left", inputLeft);
    }

    // Fonction permettant de changer la touche pour aller à droite
    public void changeInputRight() {
        string inputRight = gameObject.GetComponent<InputField>().text.ToUpper();
        if(inputRight == "" || inputRight == " ") {
            inputRight = "D";
        }
        changeInput("Right", inputRight);
    }

    // Vérifie la validité des changements d'input et change (ou non) la valeur.
    // Elle indique aussi si la touche est déjà utilisée pour une autre action en la mettant en rouge.
    // string = l'action à changer
    // key = la touche à affilier à l'action
    public void changeInput(string input, string key) {
        bool validity = checkInputAvailable(input,key);
        if(validity == true) {
            PlayerPrefs.SetString(input, key);
            GameObject.FindWithTag("text_input_" + input).GetComponent<Text>().color = Color.black;
        } else {
            GameObject.FindWithTag("text_input_" + input).GetComponent<Text>().color = Color.red;
        }
    }

    // Vérifie la validité des changements d'input
    // input = l'action à changer
    // key = la touche à affilier à l'action
    public bool checkInputAvailable(string input, string key) {
        bool checkInputAvailable = true;
        // La touche "O" est utilisée exclusivement pour les options
        if(key != "O") {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("Inventory", PlayerPrefs.GetString("Inventory", "I"));
            dictionary.Add("Interaction", PlayerPrefs.GetString("Interaction", "E"));
            dictionary.Add("Forward", PlayerPrefs.GetString("Forward", "Z"));
            dictionary.Add("Backward", PlayerPrefs.GetString("Backward", "S"));
            dictionary.Add("Right", PlayerPrefs.GetString("Right", "D"));
            dictionary.Add("Left", PlayerPrefs.GetString("Left", "Q"));  

            foreach(KeyValuePair<string,string> elem in dictionary) {
                if(elem.Value == key) {
                    if(elem.Key != input) {
                        checkInputAvailable = false;
                    }
                }
            }
        } else {
            checkInputAvailable = false;
        }
        return checkInputAvailable;
    }

    public void loadOptionInGame() {
        // On instancie le menu des options (c'est un canvas)
        this.canvasInterface = Instantiate(this.optionInterface, new Vector3(541.875f, 261.875f , 0), Quaternion.identity) as GameObject;
        // On le rend invisible
        this.canvasInterface.SetActive(false);
    }

    public bool getInOption() {
        return this.inOption;
    }

    public GameObject getCanvasInterface() {
        return this.canvasInterface;
    }
}