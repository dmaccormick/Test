This project was an assignment for a 3rd year engines class in 2018.

Texture References:
Controller ->  https://thewolfbunny.deviantart.com/art/Xbox-One-Controller-Template-558306289
Buttons ->  https://opengameart.org/content/free-keyboard-and-controllers-prompts-pack (Some edits were made to DPAD and thumbstick images, most are originals)


How To Run:
- Navigate to UnityProject/Assets/My Assets/Scenes/ and open Main.unity
- If editor window isn't visible, go to Window/Custom/Controller Values and dock it. Recommend docking by inspector for extra height
- Connect controllers. ID's start at 0. Changing the ID in the window will change which controller is visualized.
- Run scene. The capsule (player) with the ID matching the gamepad will be controllable. The text object connected to the player shows the ID.
- All buttons perform some kind of action (see below for each action)
- DLL code is contained in the ControllerDLL folder on the root level


Unity Side Overview:
Window is under Window/Custom/Controller Values, in case it doesn't show up in the default layout. Recommend docking with inspector for the extra height.

Controller_Window is the editor window code. Handles drawing the window and showing the controller. Changing the ID in the field will change which controller is visualized.
If the ID represents a controller that isn't connected, a message will be shown. The actual values are visible underneath the graphical representation as well, as text. 
All the textures are loaded through the EditorGUIUtility in OnEnable().

Controller_Manager is the DLL bridge / singleton controller. Handles updating all of the controller values and allows for access to the entry points. When in the scene, the end user
does not have to call update on the controllers manually. 

PlayerController handles the scene functionality. Each capsule in the scene has this script. It attaches itself to a specific controller number. The number in the scene behind it
is the same number as the controller (shows which controller moves which player). All buttons perform some action.
	> Triggers scale up and down
	> Left thumbstick moves
	> Right thumbstick aims / rotates on Y-axis
	> Face buttons change colour
	> Dpad changes shape
	> Bumpers rotate on Z-axis
	> Clicking in left stick resets all rotation
	> Clicking in right stick resets height
	> Select and start move up and down
	
	
DLL Side Overview:
Wrapper.h and Wrapper.cpp has the entry points. Controllers are held in array in the .cpp and entry points interact with them.

Controller class (.h and .cpp) hooks into XInput and when updated, retrieves new values. Uses deadzones for thumbsticks and triggers. Getters allow access to values. 
