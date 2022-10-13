Menu Generation Tool by Nathan Vong
A tool to easily create and setup your main menu, pause menu and setting menu and options.
with working buttons, sliders.

--------------------------------------------------------------------------------------------------

PREREQUISITES:
 - TextMeshPro

 Tested Unity Version: 2020.3.30f1

--------------------------------------------------------------------------------------------------

WHERE TO FIND WINDOW EDITOR:
#Custom Window > Menu Generation Tool

#Custom Window can be found in the tabs on the top left of the Unity engine window
between Component and Window.

--------------------------------------------------------------------------------------------------

HOW TO USE:
1. Add Canvas and prefabs to the top of Menu Generation Tool Editor.
2. Once added press the "Set default style data" button. Doing so will set the "Style Format" data
to it's default. Getting said data from the prefabs you added,

(Optional) Customised Style Format data. 
Click "Style Format" if you wish to customise the data

3. Select Menu Type
4. Select Layout Type
NOTE: Create Main Menu or Pause Menu before making the setting menu.
As a panel is needed the place the created UI for the setting menu. Which can be created in
the Main and pause menu.

5. Setup menu preferences
6. Press the "Create" buttin to generate the menu

NOTE: there is a "How to use" Menu Type that is set as default that desplays the above steps.

--------------------------------------------------------------------------------------------------

Update Canvas data:
Setup the canvas for use (Recommended)

--------------------------------------------------------------------------------------------------

Prefabs can be customised as the users wish. If errors occure from the change just 
make "Set Style data" = false. This will prevent the tool from making changes to the prefab when
its created.

This Menu Generation Tool also includes:
	- A Defualt Audio Mixer (Master, SFX, Music, Voice) with exposed Parameters. 
 Under (Assets > Menu Tool > Settings)
	- Pause Script
	- Button Scripts such as Button_Pause, Button_Load, Button_OpenClosePanel & Button_Quit
		- Button_Pause is also used for the resume button.
	- Settings Scripts. A SettingsContoller & its children such as Settings_Volume, 
 Settings_Resolution & Settings_Fullscreen.
		- SettingsContoller contains all the settings code
		- its children allows UI input (Slider, Toggle, Dropdown) to connect to the 
		SettingsController

All added script components can be found on the gameobject its created for.
E.g. Button scripts on button gameobject
E.g. Pause Script on Pause Menu gameobject
E.g. Setting Controller can be found on the gameobject you set as your "Panel to Place settings" 
in the editor
E.g. SettingsContoller children (E.g. Settings_Volume) can be found on the created UI GameObject

--------------------------------------------------------------------------------------------------

A Main and Pause Menu scene & build have already been made. Which was mostly created with the
Menu Generation Tool. The only UI that wasn't generated was the HUD in the for testing the
Pause Menu in the LevelScene.