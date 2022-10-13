using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

[System.Serializable]
public class ButtonObject
{
    public enum ButtonType
    {
        LoadScene,
        OpenAnotherMenu,
        Quit,
        Resume,
        Empty
    }
    public ButtonType buttonType;
    public string buttonText;
}

[System.Serializable]
public class SettingObject
{
    public enum SettingType
    {
        Volume,
        Resolution,
        WindowMode
    }
    public SettingType settingType;
    public string settingText;
}


public class MenuGenerationToolEditor : EditorWindow
{
    //---STYLE---
    GameObject panelPrefab, buttonPrefab, sliderPrefab, textPrefab, dropdownPrefab, togglePrefab;

    //text style
    TMP_FontAsset textFontAsset;
    int textSize;
    Color textColour;

    //Header style
    TMP_FontAsset headerFontAsset;
    int headerSize;
    Color headerColour;

    //Button style
    TMP_FontAsset buttonFontAsset;
    Color buttonTextColour;
    Vector2 buttonSize;
    Color buttonNormalColour;
    Color buttonHighlightedColour;
    Color buttonPressedColour;
    Color buttonSelectedColour;
    Color buttonDisabledColour;

    //Slider style
    TMP_FontAsset sliderFontAsset;
    Color sliderTextColour;
    Vector2 sliderSize;
    Color sliderBackgroundColour;
    Color sliderFillColour;
    Color handleNormalColour;
    Color handleHighlightedColour;
    Color handlePressedColour;
    Color handleSelectedColour;
    Color handleDisabledColour;
    float handleWidth;

    //Dropdown Style
    TMP_FontAsset dropdownFontAsset;
    Color dropdownTextColour;
    Vector2 dropdownSize;
    Color dropdownNormalColour;
    Color dropdownHighlightedColour;
    Color dropdownPressedColour;
    Color dropdownSelectedColour;
    Color dropdownDisabledColour;

    //Toggle Style
    TMP_FontAsset toggleFontAsset;
    Color toggleTextColour;
    Vector2 toggleSize;
    Color toggleNormalColour;
    Color toggleHighlightedColour;
    Color togglePressedColour;
    Color toggleSelectedColour;
    Color toggleDisabledColour;

    //Unified Colour Data
    Color NormalColour;
    Color HighlightedColour;
    Color PressedColour;
    Color SelectedColour;
    Color DisabledColour;

    //---MENU---    
    public enum MenuType
    {
        HowToUse,
        MainMenu, 
        PauseMenu,
        SettingsMenu
    }
    MenuType menuType;

    public enum MainMenuLayout
    {
        Simple_Left,
        Simple_Center,
        Simple_Right
    }
    MainMenuLayout mainMenuLayout;

    public enum PauseMenuLayout
    {
        SimpleFullscreen_Left,
        SimpleFullscreen_Center,
        SimpleFullscreen_Right
    }
    PauseMenuLayout pauseMenuLayout;

    public enum SettingsMenuLayout
    {
        SimpleListed
    }
    SettingsMenuLayout settingsMenuLayout;

    //---MENU UI---
    GameObject canvas;
    GameObject buttonGo, sliderGo, toggleGo, dropdownGo, textGo;
    public List<ButtonObject> buttonsToCreate = new List<ButtonObject>();
    public List<string> buttonLable = new List<string>();
    public List<string> buttonSceneName = new List<string>();
    public List<GameObject> buttonGOList = new List<GameObject>();

    //---OTHER---
    bool createPanelToOpen;
    bool visibleBGPanel;
    Color bgPanelColour;
    bool setButtonSizeToText;
    bool updateCanvas;
    bool setStyleData;
    bool unifyColourData;
    Vector2 scrollPos;
    TextAnchor childAlignment; // For Verticle & Horizontal Layout group child Alignment
    TextAlignmentOptions textAlignmentOptions;
    bool startingScreen;
    bool loadingScreen;
    GameObject loadingScreenPefab;
    bool showlabel;

    //Pause Menu Variables
    bool isTimePauseable;
    Pause pauseScript;

    //Setting Menu Variables
    GameObject panelToPlaceSettings;
    AudioMixer audioMixer;
    public List<SettingObject> settingObjectToCreate = new List<SettingObject>();
    public List<string> volumeParamaters = new List<string>();
    public List<string> settingVolumeParameterLable = new List<string>();
    

    //For GUI Arrays
    SerializedObject so;
    SerializedProperty buttonProperty;
    SerializedProperty settingProperty;



    [MenuItem("#Custom Window/Menu Generation Tool")]
    static void init()
    {
        MenuGenerationToolEditor window = (MenuGenerationToolEditor)EditorWindow.GetWindow(typeof(MenuGenerationToolEditor));
        window.Show();
    }

    private void OnEnable()
    {
        so = new SerializedObject(this);
        buttonProperty = so.FindProperty("buttonsToCreate");
        settingProperty = so.FindProperty("settingObjectToCreate");

        //Default Values
        bgPanelColour = new Vector4(0, 0, 0, 0.35f);
        setStyleData = true;
        unifyColourData = false;
        createPanelToOpen = true;
        startingScreen = true;
        loadingScreen = true;
        showlabel = false;
    }

    bool foldStyle = false;

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect (5,5,((float)Screen.width - 5f),((float)Screen.height - 30f)));
        GUILayout.BeginVertical(); 
        scrollPos = GUILayout.BeginScrollView(scrollPos,false, false);

        //Step 1 - GameObjects & Prefabs
        canvas = (GameObject)EditorGUILayout.ObjectField("Canvas", canvas, typeof(GameObject), true);
        EditorGUILayout.Space(10);
        panelPrefab = (GameObject)EditorGUILayout.ObjectField("Panel Prefab", panelPrefab, typeof(GameObject), true);
        textPrefab = (GameObject)EditorGUILayout.ObjectField("Text Prefab", textPrefab, typeof(GameObject), true);
        buttonPrefab = (GameObject)EditorGUILayout.ObjectField("Button Prefab", buttonPrefab, typeof(GameObject), true);
        sliderPrefab = (GameObject)EditorGUILayout.ObjectField("Slider Prefab", sliderPrefab, typeof(GameObject), true);
        dropdownPrefab = (GameObject)EditorGUILayout.ObjectField("Dropdown Prefab", dropdownPrefab, typeof(GameObject), true);
        togglePrefab = (GameObject)EditorGUILayout.ObjectField("Toggle Prefab", togglePrefab, typeof(GameObject), true);

        //Step 2 - Style Data
        if (GUILayout.Button("Set default style data"))
        {
            textSize = 27;
            textColour = textPrefab.GetComponent<TextMeshProUGUI>().color; // = 50,50,50,255
            textFontAsset = textPrefab.GetComponent<TextMeshProUGUI>().font;
            headerSize = 50;
            headerColour = textPrefab.GetComponent<TextMeshProUGUI>().color;
            headerFontAsset = textPrefab.GetComponent<TextMeshProUGUI>().font;

            buttonTextColour = buttonPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color;
            buttonSize = buttonPrefab.GetComponent<RectTransform>().sizeDelta;
            buttonNormalColour = buttonPrefab.GetComponent<Button>().colors.normalColor;
            buttonHighlightedColour = buttonPrefab.GetComponent<Button>().colors.highlightedColor;
            buttonPressedColour = buttonPrefab.GetComponent<Button>().colors.pressedColor;
            buttonSelectedColour = buttonPrefab.GetComponent<Button>().colors.selectedColor;
            buttonDisabledColour = buttonPrefab.GetComponent<Button>().colors.disabledColor;
            buttonFontAsset = buttonPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().font;

            sliderSize = sliderPrefab.GetComponent<RectTransform>().sizeDelta;
            sliderBackgroundColour = sliderPrefab.transform.GetChild(0).GetComponent<Image>().color;
            sliderFillColour = sliderPrefab.transform.GetChild(1).GetChild(0).GetComponent<Image>().color;
            handleNormalColour = sliderPrefab.GetComponent <Slider>().colors.normalColor;
            handleHighlightedColour = sliderPrefab.GetComponent<Slider>().colors.highlightedColor;
            handlePressedColour = sliderPrefab.GetComponent<Slider>().colors.pressedColor;
            handleSelectedColour = sliderPrefab.GetComponent<Slider>().colors.selectedColor;
            handleDisabledColour = sliderPrefab.GetComponent<Slider>().colors.disabledColor;
            handleWidth = sliderPrefab.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().sizeDelta.x;

            dropdownFontAsset = dropdownPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().font;
            dropdownTextColour = dropdownPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color;
            dropdownSize = dropdownPrefab.GetComponent<RectTransform>().sizeDelta;
            dropdownNormalColour = dropdownPrefab.GetComponent<TMP_Dropdown>().colors.normalColor;
            dropdownHighlightedColour = dropdownPrefab.GetComponent<TMP_Dropdown>().colors.highlightedColor;
            dropdownPressedColour = dropdownPrefab.GetComponent<TMP_Dropdown>().colors.pressedColor;
            dropdownSelectedColour = dropdownPrefab.GetComponent<TMP_Dropdown>().colors.selectedColor;
            dropdownDisabledColour = dropdownPrefab.GetComponent<TMP_Dropdown>().colors.disabledColor;

            toggleFontAsset = togglePrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().font;
            toggleTextColour = togglePrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color;
            toggleSize = togglePrefab.GetComponent<RectTransform>().sizeDelta;
            toggleNormalColour = togglePrefab.GetComponent<Toggle>().colors.normalColor;
            toggleHighlightedColour = togglePrefab.GetComponent<Toggle>().colors.highlightedColor;
            togglePressedColour = togglePrefab.GetComponent<Toggle>().colors.pressedColor;
            toggleSelectedColour = togglePrefab.GetComponent<Toggle>().colors.selectedColor;
            toggleDisabledColour = togglePrefab.GetComponent<Toggle>().colors.disabledColor;

            NormalColour = buttonPrefab.GetComponent<Button>().colors.normalColor;
            HighlightedColour = buttonPrefab.GetComponent<Button>().colors.highlightedColor;
            PressedColour = buttonPrefab.GetComponent<Button>().colors.pressedColor;
            SelectedColour = buttonPrefab.GetComponent<Button>().colors.selectedColor;
            DisabledColour = buttonPrefab.GetComponent<Button>().colors.disabledColor;
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        //Style GUI
        unifyColourData = EditorGUILayout.Toggle("Unify Colour data", unifyColourData);
        setStyleData = EditorGUILayout.Toggle("Set Style Data", setStyleData);
        EditorGUILayout.Space(10);

        if(setStyleData == true) 
        {
            foldStyle = EditorGUILayout.Foldout(foldStyle, "Stlye Format", true);
            if (foldStyle)
            {
                EditorGUILayout.Space(10);

                EditorGUILayout.LabelField("Text Style", EditorStyles.boldLabel);
                textFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("Font (optional)", textFontAsset, typeof(TMP_FontAsset), true);
                textSize = EditorGUILayout.IntField("Text Size", textSize);
                textColour = EditorGUILayout.ColorField("Colour", textColour);
                EditorGUILayout.Space(10);

                EditorGUILayout.LabelField("Header Style", EditorStyles.boldLabel);
                headerFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("Header Font (optional)", headerFontAsset, typeof(TMP_FontAsset), true);
                headerSize = EditorGUILayout.IntField("Header Text Size", headerSize);
                headerColour = EditorGUILayout.ColorField("Colour", headerColour);
                EditorGUILayout.Space(10);

                EditorGUILayout.LabelField("Button Style", EditorStyles.boldLabel);
                buttonFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("Text Font (optional)", buttonFontAsset, typeof(TMP_FontAsset), true);
                buttonTextColour = EditorGUILayout.ColorField("Text Colour", buttonTextColour);
                buttonSize = EditorGUILayout.Vector2Field("Size", buttonSize);
                if (unifyColourData == false)
                {
                    buttonNormalColour = EditorGUILayout.ColorField("Normal Colour", buttonNormalColour);
                    buttonHighlightedColour = EditorGUILayout.ColorField("Highlighted Colour", buttonHighlightedColour);
                    buttonPressedColour = EditorGUILayout.ColorField("Pressed Colour", buttonPressedColour);
                    buttonSelectedColour = EditorGUILayout.ColorField("Selected Colour", buttonSelectedColour);
                    buttonDisabledColour = EditorGUILayout.ColorField("Disabled Colour", buttonDisabledColour);
                }

                EditorGUILayout.Space(10);

                EditorGUILayout.LabelField("Slider Style", EditorStyles.boldLabel);
                sliderSize = EditorGUILayout.Vector2Field("Size", sliderSize);
                sliderBackgroundColour = EditorGUILayout.ColorField("Slider Background Colour", sliderBackgroundColour);
                sliderFillColour = EditorGUILayout.ColorField("Slider Fill Colour", sliderFillColour);
                if (unifyColourData == false)
                {
                    handleNormalColour = EditorGUILayout.ColorField("Handle Normal Colour", handleNormalColour);
                    handleHighlightedColour = EditorGUILayout.ColorField("Handle Highlighted Colour", handleHighlightedColour);
                    handlePressedColour = EditorGUILayout.ColorField("Handle Pressed Colour", handlePressedColour);
                    handleSelectedColour = EditorGUILayout.ColorField("Handle Selected Colour", handleSelectedColour);
                    handleDisabledColour = EditorGUILayout.ColorField("Handle Disabled Colour", handleDisabledColour);
                }
                handleWidth = EditorGUILayout.FloatField("Handle Width", handleWidth);
                EditorGUILayout.Space(10);

                EditorGUILayout.LabelField("Dropdown Style", EditorStyles.boldLabel);
                dropdownFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("Text Font (optional)", dropdownFontAsset, typeof(TMP_FontAsset), true);
                dropdownTextColour = EditorGUILayout.ColorField("Text Colour", dropdownTextColour);
                dropdownSize = EditorGUILayout.Vector2Field("Size", dropdownSize);
                if (unifyColourData == false)
                {
                    dropdownNormalColour = EditorGUILayout.ColorField("Normal Colour", dropdownNormalColour);
                    dropdownHighlightedColour = EditorGUILayout.ColorField("Highlighted Colour", dropdownHighlightedColour);
                    dropdownPressedColour = EditorGUILayout.ColorField("Pressed Colour", dropdownPressedColour);
                    dropdownSelectedColour = EditorGUILayout.ColorField("Selected Colour", dropdownSelectedColour);
                    dropdownDisabledColour = EditorGUILayout.ColorField("Disabled Colour", dropdownDisabledColour);
                }
                EditorGUILayout.Space(10);

                EditorGUILayout.LabelField("Toggle Style", EditorStyles.boldLabel);
                toggleFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("Text Font (optional)", toggleFontAsset, typeof(TMP_FontAsset), true);
                toggleTextColour = EditorGUILayout.ColorField("Text Colour", dropdownTextColour);
                toggleSize = EditorGUILayout.Vector2Field("Size", toggleSize);
                if (unifyColourData == false)
                {
                    toggleNormalColour = EditorGUILayout.ColorField("Normal Colour", toggleNormalColour);
                    toggleHighlightedColour = EditorGUILayout.ColorField("Highlighted Colour", toggleHighlightedColour);
                    togglePressedColour = EditorGUILayout.ColorField("Pressed Colour", togglePressedColour);
                    toggleSelectedColour = EditorGUILayout.ColorField("Selected Colour", toggleSelectedColour);
                    toggleDisabledColour = EditorGUILayout.ColorField("Disabled Colour", toggleDisabledColour);
                }

                if (unifyColourData == true)
                {
                    EditorGUILayout.Space(10);
                    EditorGUILayout.LabelField("Unified Colour Style Data", EditorStyles.boldLabel);
                    NormalColour = EditorGUILayout.ColorField("Normal Colour", NormalColour);
                    HighlightedColour = EditorGUILayout.ColorField("Highlighted Colour", HighlightedColour);
                    PressedColour = EditorGUILayout.ColorField("Pressed Colour", PressedColour);
                    SelectedColour = EditorGUILayout.ColorField("Selected Colour", SelectedColour);
                    DisabledColour = EditorGUILayout.ColorField("Disabled Colour", DisabledColour);

                    EditorGUILayout.LabelField("Unifying Button, Slider Handle, Dropdown, Toggle box");
                }


                EditorGUILayout.Space(25);

                EditorGUILayout.LabelField("NOTE: For more customised style options and control");
                EditorGUILayout.LabelField("you can duplicate and edit the prefabs themselves and");
                EditorGUILayout.LabelField("press ''Set Default style data'' button again");
            }
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        //Step 3 - Menu GUI
        menuType = (MenuType)EditorGUILayout.EnumPopup("Menu Type", menuType);

        //Step 4 & 5 Layout Selection & Setup
        if (menuType == MenuType.MainMenu)
        {
            mainMenuLayout = (MainMenuLayout)EditorGUILayout.EnumPopup("Layout Type", mainMenuLayout);
            EditorGUILayout.Space(10);

            //Shared With Paused variables
            childAlignment = (TextAnchor)EditorGUILayout.EnumPopup("Button & Text Alignment", childAlignment);
            visibleBGPanel = EditorGUILayout.Toggle("Visible BG Panel Colour", visibleBGPanel);
            if (visibleBGPanel == true)
            {
                bgPanelColour = EditorGUILayout.ColorField("BG Panel Colour", bgPanelColour);
            }
            createPanelToOpen = EditorGUILayout.Toggle("Create Menu Panels", createPanelToOpen);
            setButtonSizeToText = EditorGUILayout.Toggle("Button Size to Text", setButtonSizeToText);
            EditorGUILayout.Space(10);

            //Buttons
            EditorGUILayout.PropertyField(buttonProperty, true);
            so.ApplyModifiedProperties();

            //Name of Scene to load
            if (buttonsToCreate.Count != 0 && showlabel == true)
            {
                EditorGUILayout.LabelField("Name of Scene to Load", EditorStyles.boldLabel);
            }

            bool check = false;
            for (int i = 0; i < buttonsToCreate.Count; i++)
            {
                if (buttonSceneName.Count < buttonsToCreate.Count)
                {
                    buttonSceneName.Add("");
                    buttonLable.Add("");
                }

                if (buttonsToCreate[i].buttonType == ButtonObject.ButtonType.LoadScene)
                {
                    if (check == false)
                    {
                        showlabel = true;
                        check = true;
                    }

                    buttonLable[i] = buttonsToCreate[i].buttonText + " (Optional)";
                    if (buttonsToCreate[i].buttonText == "")
                    {
                        buttonLable[i] = "(unnamed)";
                    }
                    buttonSceneName[i] = EditorGUILayout.TextField(buttonLable[i], buttonSceneName[i]);
                }
                else
                {
                    if (check == false)
                    {
                        showlabel = false;
                    }
                }
            }
            EditorGUILayout.Space(10);

            //Loading screen
            loadingScreen = EditorGUILayout.Toggle("Loading screen", loadingScreen);
            if (loadingScreen == true)
            {
                loadingScreenPefab = (GameObject)EditorGUILayout.ObjectField("Loading screen Prefab", loadingScreenPefab, typeof(GameObject), true);
            }
            startingScreen = EditorGUILayout.Toggle("Starting screen", startingScreen);
        }

        if (menuType == MenuType.PauseMenu)
        {
            pauseMenuLayout = (PauseMenuLayout)EditorGUILayout.EnumPopup("Layout Type", pauseMenuLayout);
            EditorGUILayout.Space(10);

            //Shared With Main Menu variables
            childAlignment = (TextAnchor)EditorGUILayout.EnumPopup("Button & Text Alignment", childAlignment);
            visibleBGPanel = EditorGUILayout.Toggle("Visible BG Panel Colour", visibleBGPanel);
            if (visibleBGPanel == true)
            {
                bgPanelColour = EditorGUILayout.ColorField("BG Panel Colour", bgPanelColour);
            }
            createPanelToOpen = EditorGUILayout.Toggle("Create Menu Panels", createPanelToOpen);
            setButtonSizeToText = EditorGUILayout.Toggle("Button Size to Text", setButtonSizeToText);
            EditorGUILayout.Space(10);

            //Buttons
            EditorGUILayout.PropertyField(buttonProperty, true);
            so.ApplyModifiedProperties();

            //Name of Scene to load
            if (buttonsToCreate.Count != 0 && showlabel == true)
            {
                EditorGUILayout.LabelField("Name of Scene to Load", EditorStyles.boldLabel);
            }

            bool check = false;
            for (int i = 0; i < buttonsToCreate.Count; i++)
            {
                if (buttonSceneName.Count < buttonsToCreate.Count)
                {
                    buttonSceneName.Add("");
                    buttonLable.Add("");
                }

                if (buttonsToCreate[i].buttonType == ButtonObject.ButtonType.LoadScene)
                {
                    if(check == false)
                    {
                        showlabel = true;
                        check = true;
                    }
                    
                    buttonLable[i] = buttonsToCreate[i].buttonText + " (Optional)";
                    if (buttonsToCreate[i].buttonText == "")
                    {
                        buttonLable[i] = "(unnamed)";
                    }
                    buttonSceneName[i] = EditorGUILayout.TextField(buttonLable[i], buttonSceneName[i]);
                }
                else
                {
                    if (check == false)
                    {
                        showlabel = false;
                    }
                }
            }
            EditorGUILayout.Space(10);

            //Loading screen
            loadingScreen = EditorGUILayout.Toggle("Loading screen", loadingScreen);
            if (loadingScreen == true)
            {
                loadingScreenPefab = (GameObject)EditorGUILayout.ObjectField("Loading screen Prefab", loadingScreenPefab, typeof(GameObject), true);
            }

            isTimePauseable = EditorGUILayout.Toggle("is Game Time pauseable", isTimePauseable);
        }

        if (menuType != MenuType.MainMenu && menuType != MenuType.PauseMenu)
        {
            buttonSceneName.Clear();
            buttonLable.Clear();
        }

        if (menuType == MenuType.SettingsMenu)
        {
            settingsMenuLayout = (SettingsMenuLayout)EditorGUILayout.EnumPopup("Layout Type", settingsMenuLayout);
            EditorGUILayout.Space(10);

            panelToPlaceSettings = (GameObject)EditorGUILayout.ObjectField("Panel to Place Settings", panelToPlaceSettings, typeof(GameObject), true);
            audioMixer = (AudioMixer)EditorGUILayout.ObjectField("Audio Mixer", audioMixer, typeof(AudioMixer), true);
            EditorGUILayout.Space(10);

            EditorGUILayout.PropertyField(settingProperty, true);
            so.ApplyModifiedProperties();

            if(settingObjectToCreate.Count != 0 && showlabel == true)
            {
                EditorGUILayout.LabelField("Audio Mixer Exposed Parameter/s", EditorStyles.boldLabel);
            }

            bool check = false;
            for (int i = 0; i < settingObjectToCreate.Count; i++)
            {
                if (volumeParamaters.Count < settingObjectToCreate.Count)
                {
                    settingVolumeParameterLable.Add("");
                    volumeParamaters.Add("");
                }
                
                if (settingObjectToCreate[i].settingType == SettingObject.SettingType.Volume)
                {
                    if (check == false)
                    {
                        showlabel = true;
                        check = true;
                    }

                    settingVolumeParameterLable[i] = settingObjectToCreate[i].settingText;
                    if (settingObjectToCreate[i].settingText == "")
                    {
                        settingVolumeParameterLable[i] = "(unnamed)";
                    }
                    volumeParamaters[i] = EditorGUILayout.TextField(settingVolumeParameterLable[i], volumeParamaters[i]);
                }
                else
                {
                    if (check == false)
                    {
                        showlabel = false;
                    }
                }
                //Debug.Log("SettingOBJ = " + settingObjectToCreate.Count + " || VolParameters = " + volumeParamaters.Count);
            }
            
        }
        else
        {
            volumeParamaters.Clear();
            settingVolumeParameterLable.Clear();
        }

        if (menuType == MenuType.HowToUse)
        {
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Prerequisites:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(" - Text Mesh Pro");
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("How to use:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("1. Please add Canvas & prefabs above, if empty, before you begin");
            EditorGUILayout.LabelField("2. Once added press the ''Set default style data'' button ");
            EditorGUILayout.LabelField("Doing so will set the ''Style Format'' data to its default");
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("(Optional) Customise Style Format");
            EditorGUILayout.LabelField("Click ''Style Format'' if you wish to customise data");
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("3. Select Menu Type");
            EditorGUILayout.LabelField("4. Select Layout Type");
            EditorGUILayout.LabelField("Note: Make Main menu or Pause menu before Settings menu.");
            EditorGUILayout.LabelField("As it is recommended to use a panel created from the");
            EditorGUILayout.LabelField("Main menu or Pause menu");
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("5. Setup menu preferences");
            EditorGUILayout.LabelField("6. Press the 'Create' button to generate menu");
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Update Canvas Data:");
            EditorGUILayout.LabelField("Setup the canvas for use (Recommended)");

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("All added scripts can be found on the created menu assets");
            EditorGUILayout.LabelField("E.g. Button scripts on button gameobject");
            EditorGUILayout.LabelField("E.g. Pause script on Pause Menu gameobject");
            EditorGUILayout.LabelField("E.g.Setting Controller can be found on the ");
            EditorGUILayout.LabelField("gameobject you set as your ''Panel to Place settings'' in the editor");
            

        }

        //Set text alignment options based on child/anchor alignment(Which is used for Layout groups)
        if (childAlignment == TextAnchor.LowerLeft || childAlignment == TextAnchor.MiddleLeft || childAlignment == TextAnchor.UpperLeft)
        {
            textAlignmentOptions = TextAlignmentOptions.Left;
        }
        if (childAlignment == TextAnchor.LowerCenter || childAlignment == TextAnchor.MiddleCenter || childAlignment == TextAnchor.UpperCenter)
        {
            textAlignmentOptions = TextAlignmentOptions.Center;
        }
        if (childAlignment == TextAnchor.LowerRight || childAlignment == TextAnchor.MiddleRight || childAlignment == TextAnchor.UpperRight)
        {
            textAlignmentOptions = TextAlignmentOptions.Right;
        }
        
        //Generate Area
        EditorGUILayout.Space(25);
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        updateCanvas = EditorGUILayout.Toggle("Update Canvas data", updateCanvas);

        //Step 6 - Generate
        if(GUILayout.Button("Create", GUILayout.Height(30f)))
        {
            if (canvas != null)
            {
                //Debug.Log("Canvas found");
                if (updateCanvas == true)
                {
                    Debug.Log("Update Canvas Data");
                    canvas.GetComponent<CanvasScaler>().enabled = true;
                    canvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
                    canvas.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.5f;
                }
            }
            else{ Debug.LogError("Canvas NOT found  :  Please create Canvas if none exist and add to Menu Window Editor"); return; }

            if (panelPrefab == null)
            { Debug.LogError("Panel Prefab NOT found  :  Please add Panel prefab from the Project Window. A pre-made Panel prefab has already been created (Assets > Menu Tool > Prefabs)"); return; }
            if (buttonPrefab == null)
            { Debug.LogError("Button Prefab NOT found  :  Please add Button prefab from the Project Window. A pre-made Button prefab has already been created (Assets > Menu Tool > Prefabs)"); return; }
            if (textPrefab == null)
            { Debug.LogError("Text Prefab NOT found  :  Please add Text prefab from the Project Window. A pre-made Text prefab has already been created (Assets > Menu Tool > Prefabs)"); return; }

            if (menuType == MenuType.SettingsMenu)
            {
                if (sliderPrefab == null)
                { Debug.LogError("Slider Prefab NOT found  :  Please add Slider prefab from the Project Window. A pre-made Slider prefab has already been created (Assets > Menu Tool > Prefabs)"); return; }
                if (dropdownPrefab == null)
                { Debug.LogError("Dropdown Prefab NOT found  :  Please add Dropdown prefab from the Project Window. A pre-made Dropdown prefab has already been created (Assets > Menu Tool > Prefabs)"); return; }
                if (togglePrefab == null)
                { Debug.LogError("Toggle Prefab NOT found  :  Please add Toggle prefab from the Project Window. A pre-made Toggle prefab has already been created (Assets > Menu Tool > Prefabs)"); return; }
            }
            else
            {
                if (loadingScreen == true && loadingScreenPefab == null)
                { Debug.LogError("Loading Screen Pefab NOT found  :  Please add Loading Screen prefab from the Project Window. A pre-made Loading Screen prefab has already been created (Assets > Menu Tool > Prefabs)"); return; }
            }

            if (menuType != MenuType.HowToUse)
            {
                GenerateMenu();
            }
            else
            {
                Debug.LogError("Menu Type is not selected");
            }
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    void GenerateMenu()
    {
        Debug.Log("Generate");
        if (menuType == MenuType.MainMenu)
        {
            //Panel Main
            GameObject mainPanel = Instantiate(panelPrefab, canvas.transform);
            mainPanel.name = "MainMenu";

            if (visibleBGPanel == false)
            {
                bgPanelColour = new Vector4(0, 0, 0, 0);
            }
            mainPanel.GetComponent<Image>().color = bgPanelColour;
            mainPanel.GetComponent<Image>().raycastTarget = false;

            //Anchor preset : StretchAll
            RectTransform rectTramsform = mainPanel.GetComponent<RectTransform>();
            rectTramsform.anchorMin = new Vector2(0, 0);
            rectTramsform.anchorMax = new Vector2(1, 1);
            rectTramsform.sizeDelta = new Vector4(0, 0, 0, 0);

            //Panel 1
            GameObject panel_1 = Instantiate(panelPrefab, mainPanel.transform);
            panel_1.name = "Buttons Panel";
            panel_1.GetComponent<Image>().color = new Vector4(0, 0, 0, 0);

            //Layout
            if (mainMenuLayout == MainMenuLayout.Simple_Left)
            {
                panel_1.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                panel_1.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                panel_1.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
                panel_1.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, 0);
                //textAlignmentOptions = TextAlignmentOptions.Left;
            }
            if (mainMenuLayout == MainMenuLayout.Simple_Center)
            {
                panel_1.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
                panel_1.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
                panel_1.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                panel_1.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                //textAlignmentOptions = TextAlignmentOptions.Center;
            }
            if (mainMenuLayout == MainMenuLayout.Simple_Right)
            {
                panel_1.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
                panel_1.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
                panel_1.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
                panel_1.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, 0);
                //textAlignmentOptions = TextAlignmentOptions.Right;
            }
            panel_1.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, buttonSize.x);

            //Panel 1 - VerticalLayoutGroup
            VerticalLayoutGroup panel_1_vLayerG = panel_1.AddComponent<VerticalLayoutGroup>();
            panel_1_vLayerG.childControlWidth = true;
            panel_1_vLayerG.childForceExpandWidth = true;
            panel_1_vLayerG.childForceExpandHeight = false;
            panel_1_vLayerG.childAlignment = childAlignment;
            panel_1_vLayerG.spacing = 20f;
            RectOffset newPadding = panel_1_vLayerG.padding;
            newPadding.top = 75;
            newPadding.bottom = 75;
            panel_1_vLayerG.padding = newPadding;

            //Assets in menu to create
            if(buttonsToCreate.Count != 0)
            {
                for (int i = 0; i < buttonsToCreate.Count; i++)
                {
                    CreateButton(i, panel_1);
                    if(buttonsToCreate[i].buttonType == ButtonObject.ButtonType.LoadScene)
                    {
                        //buttonGO set in CreateButton();
                        buttonGo.GetComponent<Button_Load>().sceneToLoad = buttonSceneName[i];
                    }
                }
            }
            else
            { Debug.LogWarning("No buttons to create"); }

            if (startingScreen == true)
            {
                GameObject startPanel = Instantiate(panelPrefab, canvas.transform);
                startPanel.name = "StartingScreen";
                startPanel.GetComponent<Image>().color = bgPanelColour;
                startPanel.AddComponent<StartingScreen>().mainMenu = mainPanel;

                textGo = Instantiate(textPrefab, startPanel.transform);
                SetText(textGo, "Press [any] button to Start");

                rectTramsform = textGo.GetComponent<RectTransform>();
                rectTramsform.anchorMin = new Vector2(0, 0);
                rectTramsform.anchorMax = new Vector2(1, 0);
                rectTramsform.pivot = new Vector2(0.5f, 0);
                rectTramsform.sizeDelta = new Vector4(0, 0, 0, 0);
                rectTramsform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50);
                rectTramsform.anchoredPosition = new Vector2(0, 20);

                textGo.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

                mainPanel.GetComponent<CanvasGroup>().alpha = 0;
                mainPanel.GetComponent<CanvasGroup>().interactable = false;
                mainPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }

            if(loadingScreen == true)
            {
                CreateLoadingScreen();
            }
        }

        if (menuType == MenuType.PauseMenu)
        {
            //Panel Main
            GameObject mainPanel = Instantiate(panelPrefab, canvas.transform);
            mainPanel.name = "Pause Menu";
            if (visibleBGPanel == false)
            {
                bgPanelColour = new Vector4(0, 0, 0, 0);
            }
            mainPanel.GetComponent<Image>().color = bgPanelColour;

            //Anchor preset : StretchAll
            mainPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            mainPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            mainPanel.GetComponent<RectTransform>().sizeDelta = new Vector4(0, 0, 0, 0);
            mainPanel.GetComponent<Image>().raycastTarget = false;

            mainPanel.GetComponent<Image>().raycastTarget = false;
            mainPanel.GetComponent<CanvasGroup>().alpha = 0;
            mainPanel.GetComponent<CanvasGroup>().interactable = false;
            mainPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

            //Panel 1
            GameObject panel_1 = Instantiate(panelPrefab, mainPanel.transform);
            panel_1.GetComponent<Image>().color = new Vector4(0, 0, 0, 0);

            //Layout
            if (pauseMenuLayout == PauseMenuLayout.SimpleFullscreen_Left)
            {
                panel_1.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                panel_1.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                panel_1.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
                panel_1.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, 0);
                //textAlignmentOptions = TextAlignmentOptions.Left;
            }
            if (pauseMenuLayout == PauseMenuLayout.SimpleFullscreen_Center)
            {
                panel_1.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
                panel_1.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
                panel_1.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                panel_1.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                //textAlignmentOptions = TextAlignmentOptions.Center;
            }
            if (pauseMenuLayout == PauseMenuLayout.SimpleFullscreen_Right)
            {
                panel_1.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
                panel_1.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
                panel_1.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
                panel_1.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, 0);
                //textAlignmentOptions = TextAlignmentOptions.Right;
            }
            panel_1.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, buttonSize.x);

            //Panel 1 - VerticalLayoutGroup
            VerticalLayoutGroup panel_1_vLayerG = panel_1.AddComponent<VerticalLayoutGroup>();
            panel_1_vLayerG.childControlWidth = true;
            panel_1_vLayerG.childForceExpandWidth = true;
            panel_1_vLayerG.childForceExpandHeight = false;
            panel_1_vLayerG.childAlignment = childAlignment;
            panel_1_vLayerG.spacing = 20f;
            RectOffset newPadding = panel_1_vLayerG.padding;
            newPadding.top = 75;
            newPadding.bottom = 75;
            panel_1_vLayerG.padding = newPadding;

            pauseScript = mainPanel.AddComponent<Pause>();
            pauseScript.isTimePausable = isTimePauseable;

            //Assets in menu to create
            GameObject pauseHeader = Instantiate(textPrefab, panel_1.transform);
            SetHeader(pauseHeader,"Paused");

            if (buttonsToCreate.Count != 0)
            {
                for (int i = 0; i < buttonsToCreate.Count; i++)
                {
                    CreateButton(i, panel_1);
                    if (buttonsToCreate[i].buttonType == ButtonObject.ButtonType.LoadScene)
                    {
                        //buttonGO set in CreateButton();
                        buttonGo.GetComponent<Button_Load>().sceneToLoad = buttonSceneName[i];
                    }
                }
            }
            else
            { Debug.LogWarning("No buttons to create"); }



            if (loadingScreen == true)
            {
                CreateLoadingScreen();
            }
        }

        if (menuType == MenuType.SettingsMenu)
        {
            if (panelToPlaceSettings == null)
            { Debug.LogError("Panel to Place Setting NOT found  :  Please create and/or add Panel to Menu Window Editor under Menu Type Settings");
                return; }

            if(audioMixer == null && volumeParamaters.Count != 0)
            {
                Debug.LogError("Audio Mixer NOT found  :  Please add Audio Mixer from the project window. a pre-made Audio Mixer ''MainMixer'' had already been created (Assets > Menu Tool > Settings)");
                return;
            }
            GameObject Panel_1 = Instantiate(panelPrefab, panelToPlaceSettings.transform);
            Panel_1.name = "Listed Panel";
            if (visibleBGPanel == false)
            {
                bgPanelColour = new Vector4(0, 0, 0, 0);
            }
            Panel_1.GetComponent<Image>().color = bgPanelColour;
            Panel_1.GetComponent<Image>().enabled = false;

            //Anchor preset : Stretch Horizontal Top
            RectTransform rectTramsform = Panel_1.GetComponent<RectTransform>();
            rectTramsform.anchorMin = new Vector2(0, 1);
            rectTramsform.anchorMax = new Vector2(1, 1);
            rectTramsform.pivot = new Vector2(0.5f, 1);
            rectTramsform.offsetMin = new Vector2(30, rectTramsform.offsetMin.y); //Left
            rectTramsform.offsetMax = new Vector2(-30, rectTramsform.offsetMax.y); //Right
            rectTramsform.offsetMax = new Vector2(rectTramsform.offsetMax.x, -30); //Top
            //rectTramsform.offsetMin = new Vector2(rectTramsform.offsetMin.x, 30); //Bottom

            VerticalLayoutGroup vlg = Panel_1.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = 10;
            vlg.childControlWidth = true;

            ContentSizeFitter csf = Panel_1.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            if (panelToPlaceSettings.GetComponent<SettingsController>() == null)
            {   panelToPlaceSettings.AddComponent<SettingsController>();  }
            panelToPlaceSettings.GetComponent<SettingsController>().audioMixer = audioMixer;

            if(settingsMenuLayout == SettingsMenuLayout.SimpleListed)
            {
                for(int i = 0; i < settingObjectToCreate.Count ;i++)
                {
                    GameObject Panel_2 = Instantiate(panelPrefab, Panel_1.transform);
                    Panel_2.name = settingObjectToCreate[i].settingText;
                    Panel_2.GetComponent<RectTransform>().sizeDelta = new Vector4(50, 50, 50, 50);
                    
                    GameObject Panel_3 = Instantiate(panelPrefab, Panel_2.transform);
                    Panel_3.name = "Panel";
                    Panel_3.GetComponent<Image>().enabled = false;
                    

                    //Stretch all
                    Panel_3.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                    Panel_3.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);

                    //Rect Tranform
                    rectTramsform = Panel_3.GetComponent<RectTransform>();
                    rectTramsform.offsetMin = new Vector2(30, rectTramsform.offsetMin.y); //Left
                    rectTramsform.offsetMax = new Vector2(-30, rectTramsform.offsetMax.y); //Right
                    rectTramsform.offsetMax = new Vector2(rectTramsform.offsetMax.x, -10); //Top
                    rectTramsform.offsetMin = new Vector2(rectTramsform.offsetMin.x, 10); //Bottom

                    //Horizontal Layout Group
                    HorizontalLayoutGroup hlg = Panel_3.AddComponent<HorizontalLayoutGroup>();
                    hlg.spacing = 10;
                    hlg.childControlWidth = true;
                    hlg.childControlHeight = true;

                    GameObject Panel_4a = Instantiate(panelPrefab, Panel_3.transform);
                    Panel_4a.name = "Text Panel";
                    Panel_4a.GetComponent<Image>().enabled = false;

                    //Text
                    GameObject textObject = Instantiate(textPrefab, Panel_4a.transform);
                    SetText(textObject, settingObjectToCreate[i].settingText);
                    textObject.GetComponent<TextMeshProUGUI>().enableAutoSizing = true;
                    textObject.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
                    textObject.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
                    textObject.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                    textObject.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);


                    rectTramsform = textObject.GetComponent<RectTransform>();
                    rectTramsform.offsetMin = new Vector2(0, 0); //Left % Top
                    rectTramsform.offsetMax = new Vector2(0, 0); //Right & Bottom

                    GameObject Panel_4b = Instantiate(panelPrefab, Panel_3.transform);
                    Panel_4b.name = "Setting Panel";
                    Panel_4b.GetComponent<Image>().enabled = false;

                    if (settingObjectToCreate[i].settingType == SettingObject.SettingType.Resolution)
                    {
                        CreateDropDown(Panel_4b);
                        dropdownGo.AddComponent<Settings_Resolution>();
                        dropdownGo.GetComponent<Settings_Resolution>().settingsController = panelToPlaceSettings.GetComponent<SettingsController>();
                        dropdownGo.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
                        dropdownGo.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                        dropdownGo.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
                        rectTramsform = dropdownGo.GetComponent<RectTransform>();

                    }
                    if (settingObjectToCreate[i].settingType == SettingObject.SettingType.Volume)
                    {
                        CreateSlider(Panel_4b, -80, 20, 0, SliderText.DisplayType.IntValue);
                        sliderGo.AddComponent<Settings_Volume>();
                        sliderGo.GetComponent<Settings_Volume>().paramaterName = volumeParamaters[i];
                        sliderGo.GetComponent<Settings_Volume>().settingsController = panelToPlaceSettings.GetComponent<SettingsController>();
                        sliderGo.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
                        sliderGo.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                        sliderGo.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
                        rectTramsform = sliderGo.GetComponent<RectTransform>();

                    }
                    if (settingObjectToCreate[i].settingType == SettingObject.SettingType.WindowMode)
                    {
                        CreateToggle(Panel_4b);
                        toggleGo.AddComponent<Settings_Fullscreen>();
                        toggleGo.GetComponent<Settings_Fullscreen>().settingsController = panelToPlaceSettings.GetComponent<SettingsController>();
                        toggleGo.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
                        toggleGo.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                        toggleGo.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
                        rectTramsform = toggleGo.GetComponent<RectTransform>();
                        
                    }
                    rectTramsform.offsetMin = new Vector2(0, rectTramsform.offsetMin.y); //Left
                    rectTramsform.offsetMax = new Vector2(-0, rectTramsform.offsetMax.y); //Right
                    rectTramsform.offsetMax = new Vector2(rectTramsform.offsetMax.x, -0); //Top
                    rectTramsform.offsetMin = new Vector2(rectTramsform.offsetMin.x, 0); //Bottom
                }


                textGo = Instantiate(textPrefab, Panel_1.transform);
                SetText(textGo, "Press [Backspace] button to return");

                rectTramsform = textGo.GetComponent<RectTransform>();
                rectTramsform.anchorMin = new Vector2(0, 0);
                rectTramsform.anchorMax = new Vector2(1, 0);
                rectTramsform.pivot = new Vector2(0.5f, 0);
                rectTramsform.sizeDelta = new Vector4(0, 0, 0, 0);
                rectTramsform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50);
                rectTramsform.anchoredPosition = new Vector2(0, 20);

                textGo.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

                
            }
        }
    }

    //UI Methods
    void SetText(GameObject tp_textPrefab, string tp_text)
    {
        tp_textPrefab.name = "text";

        if(setStyleData == true)
        {
            if (textFontAsset != null)
            { tp_textPrefab.GetComponent<TextMeshProUGUI>().font = textFontAsset; }
            tp_textPrefab.GetComponent<TextMeshProUGUI>().fontSize = textSize;
            tp_textPrefab.GetComponent<TextMeshProUGUI>().color = textColour;
            tp_textPrefab.GetComponent<TextMeshProUGUI>().text = tp_text;
            tp_textPrefab.GetComponent<TextMeshProUGUI>().alignment = textAlignmentOptions;
        }
    }

    void SetHeader(GameObject tp_textPrefab, string tp_text)
    {
        tp_textPrefab.name = "Header";

        if (setStyleData == true)
        {
            if (headerFontAsset != null)
            { tp_textPrefab.GetComponent<TextMeshProUGUI>().font = headerFontAsset; }
            tp_textPrefab.GetComponent<TextMeshProUGUI>().fontSize = headerSize;
            tp_textPrefab.GetComponent<TextMeshProUGUI>().color = headerColour;
            tp_textPrefab.GetComponent<TextMeshProUGUI>().text = tp_text;
            tp_textPrefab.GetComponent<TextMeshProUGUI>().alignment = textAlignmentOptions;
        }

        tp_textPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 100);
        ContentSizeFitter content = tp_textPrefab.AddComponent<ContentSizeFitter>();
        content.horizontalFit = ContentSizeFitter.FitMode.PreferredSize; //Makes width in sizeDelta in RectTransform obselete 

        if (childAlignment == TextAnchor.LowerLeft || childAlignment == TextAnchor.MiddleLeft || childAlignment == TextAnchor.UpperLeft)
        {
            tp_textPrefab.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            tp_textPrefab.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            tp_textPrefab.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
            tp_textPrefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, 0);
        }
        if (childAlignment == TextAnchor.LowerCenter || childAlignment == TextAnchor.MiddleCenter || childAlignment == TextAnchor.UpperCenter)
        {
            tp_textPrefab.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
            tp_textPrefab.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            tp_textPrefab.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            tp_textPrefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }
        if (childAlignment == TextAnchor.LowerRight || childAlignment == TextAnchor.MiddleRight || childAlignment == TextAnchor.UpperRight)
        {
            tp_textPrefab.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
            tp_textPrefab.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            tp_textPrefab.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
            tp_textPrefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, 0);
        }
    }

    void CreateButton(int i, GameObject tp_parent)// For array of Buttons
    {
        buttonGo = Instantiate(buttonPrefab, tp_parent.transform);
        buttonGo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = buttonsToCreate[i].buttonText;
        buttonGo.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, buttonSize.y);
        CreateButtonCont(buttonsToCreate[i], buttonGo);
    }

    void CreateButton(ButtonObject tp_buttonObject,  GameObject tp_parent)//For Single Button
    {
        buttonGo = Instantiate(buttonPrefab, tp_parent.transform);
        buttonGo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tp_buttonObject.buttonText;
        buttonGo.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, buttonSize.y);
        CreateButtonCont(tp_buttonObject, buttonGo);
    }

    void CreateButtonCont(ButtonObject tp_buttonObject, GameObject tp_buttonGO)
    {
        TextMeshProUGUI buttonText = tp_buttonGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Button button = tp_buttonGO.GetComponent<Button>();

        buttonGOList.Add(tp_buttonGO);

        if (childAlignment == TextAnchor.LowerLeft || childAlignment == TextAnchor.MiddleLeft || childAlignment == TextAnchor.UpperLeft)
        {
            tp_buttonGO.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            tp_buttonGO.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            tp_buttonGO.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
            tp_buttonGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, 0);
        }
        if (childAlignment == TextAnchor.LowerCenter || childAlignment == TextAnchor.MiddleCenter || childAlignment == TextAnchor.UpperCenter)
        {
            tp_buttonGO.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
            tp_buttonGO.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            tp_buttonGO.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            tp_buttonGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }
        if (childAlignment == TextAnchor.LowerRight || childAlignment == TextAnchor.MiddleRight || childAlignment == TextAnchor.UpperRight)
        {
            tp_buttonGO.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
            tp_buttonGO.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            tp_buttonGO.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
            tp_buttonGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, 0);
        }

        tp_buttonGO.name = tp_buttonObject.buttonText + " Button";
        buttonText.alignment = textAlignmentOptions;

        if (setStyleData == true)
        {
            if (buttonFontAsset != null)
            { buttonText.font = buttonFontAsset; }
            buttonText.color = buttonTextColour;

            ColorBlock cb = button.colors;

            if (unifyColourData == false)
            {
                cb.normalColor = buttonNormalColour;
                cb.highlightedColor = buttonHighlightedColour;
                cb.pressedColor = buttonPressedColour;
                cb.selectedColor = buttonSelectedColour;
                cb.disabledColor = buttonDisabledColour;
            }
            else
            {
                cb.normalColor = NormalColour;
                cb.highlightedColor = HighlightedColour;
                cb.pressedColor = PressedColour;
                cb.selectedColor = SelectedColour;
                cb.disabledColor = DisabledColour;
            }
            button.colors = cb;
        }

        if(setButtonSizeToText == true)
        {
            VerticalLayoutGroup vlg = tp_buttonGO.AddComponent<VerticalLayoutGroup>();
            RectOffset padding = new RectOffset(10,10,10,10);
            vlg.padding = padding;
            vlg.childAlignment = TextAnchor.MiddleCenter;
            vlg.childForceExpandHeight = false;
            vlg.childForceExpandWidth = false;
            vlg.childControlHeight = true;
            vlg.childControlWidth = true;
            ContentSizeFitter csf = tp_buttonGO.AddComponent<ContentSizeFitter>();
            csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        } 

        if (tp_buttonObject.buttonType == ButtonObject.ButtonType.LoadScene)
        {
            Button_Load loadScript = tp_buttonGO.AddComponent<Button_Load>();
        }
        if (tp_buttonObject.buttonType == ButtonObject.ButtonType.OpenAnotherMenu)
        {
            Button_OpenClosePanel attatchedMenu = tp_buttonGO.AddComponent<Button_OpenClosePanel>();
            GameObject otherMenuPanel;

            attatchedMenu.previousMenu = tp_buttonGO.transform.parent.parent.gameObject;

            if (createPanelToOpen == true)
            {
                otherMenuPanel = Instantiate(panelPrefab, canvas.transform);

                otherMenuPanel.name = (tp_buttonObject.buttonText + " Menu");
                otherMenuPanel.GetComponent<Image>().raycastTarget = false;
                otherMenuPanel.GetComponent<Image>().color = bgPanelColour;
                otherMenuPanel.GetComponent<CanvasGroup>().alpha = 0;
                otherMenuPanel.GetComponent<CanvasGroup>().interactable = false;
                otherMenuPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
                attatchedMenu.panelToOpenClose = otherMenuPanel;
            }
        }
        if (tp_buttonObject.buttonType == ButtonObject.ButtonType.Quit)
        {
            tp_buttonGO.AddComponent<Button_Quit>();
        }
        if (tp_buttonObject.buttonType == ButtonObject.ButtonType.Resume)
        {
            tp_buttonGO.AddComponent<Button_Pause>();
            tp_buttonGO.GetComponent<Button_Pause>().pauseScript = pauseScript;
        }
    }

    void CreateSlider(GameObject tp_parent, float tp_minValue, float tp_maxValue, float tp_defaultValue, SliderText.DisplayType tp_displayType)
    {
        sliderGo = Instantiate(sliderPrefab, tp_parent.transform);
        
        Slider slider = sliderGo.GetComponent<Slider>();
        SliderText sliderTextScript = sliderGo.transform.GetChild(3).GetComponent<SliderText>();

        slider.minValue = tp_minValue;
        slider.maxValue = tp_maxValue;
        slider.value = tp_defaultValue;
        sliderTextScript.displayType = tp_displayType;

        ColorBlock cb = slider.colors;

        if (setStyleData == true)
        {
            if(unifyColourData == false)
            {
                cb.normalColor = handleNormalColour;
                cb.highlightedColor = handleHighlightedColour;
                cb.pressedColor = handlePressedColour;
                cb.selectedColor = handleSelectedColour;
                cb.disabledColor = handleDisabledColour;
            }
            else
            {
                cb.normalColor = NormalColour;
                cb.highlightedColor = HighlightedColour;
                cb.pressedColor = PressedColour;
                cb.selectedColor = SelectedColour;
                cb.disabledColor = DisabledColour;
            }
        }
    }

    void CreateDropDown(GameObject tp_parent)
    {
        dropdownGo = Instantiate(dropdownPrefab, tp_parent.transform);
        TMP_Dropdown dropdown = dropdownPrefab.GetComponent<TMP_Dropdown>();

        ColorBlock cb = dropdown.colors;

        if (setStyleData == true)
        {
            if (unifyColourData == false)
            {
                cb.normalColor = dropdownNormalColour;
                cb.highlightedColor = dropdownHighlightedColour;
                cb.pressedColor = dropdownPressedColour;
                cb.selectedColor = dropdownSelectedColour;
                cb.disabledColor = dropdownDisabledColour;
            }
            else
            {
                cb.normalColor = NormalColour;
                cb.highlightedColor = HighlightedColour;
                cb.pressedColor = PressedColour;
                cb.selectedColor = SelectedColour;
                cb.disabledColor = DisabledColour;
            }
        }
    }

    void CreateToggle(GameObject tp_parent)
    {
        toggleGo = Instantiate(togglePrefab, tp_parent.transform);
        Toggle toggle = togglePrefab.GetComponent<Toggle>();

        ColorBlock cb = toggle.colors;

        if (setStyleData == true)
        {
            if (unifyColourData == false)
            {
                cb.normalColor = toggleNormalColour;
                cb.highlightedColor = toggleHighlightedColour;
                cb.pressedColor = togglePressedColour;
                cb.selectedColor = toggleSelectedColour;
                cb.disabledColor = toggleDisabledColour;
            }
            else
            {
                cb.normalColor = NormalColour;
                cb.highlightedColor = HighlightedColour;
                cb.pressedColor = PressedColour;
                cb.selectedColor = SelectedColour;
                cb.disabledColor = DisabledColour;
            }
        }
    }

    void CreateLoadingScreen()
    {
        GameObject loadingScreenPanel = Instantiate(loadingScreenPefab, canvas.transform);
        loadingScreenPanel.name = "Loading Screen";
        loadingScreenPanel.SetActive(false);

        
        if (buttonsToCreate.Count != 0)
        {
            for (int i = 0; i < buttonsToCreate.Count; i++)
            {
                if (buttonsToCreate[i].buttonType == ButtonObject.ButtonType.LoadScene)
                {
                    buttonGOList[i].GetComponent<Button_Load>().loadingScreen = loadingScreenPanel;
                }
            }

            buttonGOList.Clear();
        }
        else
        { Debug.LogWarning("No ''LoadScene'' button types to set loading screen"); }
    }
}
