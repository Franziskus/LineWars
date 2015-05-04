# In Unity Editor
For a designer the most interesting stuff is in the scene
Scene > Player (GameObject) > SpawnManager (Script)
Hero Points: Defines how match points the SpawnManger gets for spawning heros.This is indirect if he can’t spawn new one because the maximum of heroes is reached. The spawn manager will save the points
Enemy Points: Defines how match points the spawn manger gets for spawning enemies.

Scene > Player (GameObject) > Walking (Script)
Speed: Base speed of the Player.
SpeedUp: Defines the speed that adds over time.

In the Project view
data > EntriesDatabase
Here are all values of Heroes and Enemies stored.

In the Project view
data > EntriesDatabase
Here are all values of Heroes and Enemies stored.

# Programmier Notes
I comment the Code but I want to give some extra overview here:
* Controls (The three different control types)
* Helper (Helper classes for like Singletons, XML readers, …)
  * GameLogic (GameManager, Spawn Manager, Player and other stuff for the board)
* Ui (MainMenu and other GUI elements)

## Helper Package
The most important thing in here is MainHelper. In Games you often need Singletons. But we all know that singletons lack of flexibility. So I’m using this concept here for many of my Projects now. So we have only one singleton and all other scripts register here. But you can also register yourself with a super type and even exchange scripts that way.  However we have a clean central point for scripts here.

## Game Manager
Let's get over to the Game Manager:
![UML Class diagramm](http://yuml.me/diagram/scruffy/class/[%3C%3CIRestartable%3E%3E]-[Player],[%3C%3CIRestartable%3E%3E]%5E-.-[SpawnManager],[%3C%3CIInformEnable%3E%3E]%5E-.-[SpawnManager],[%3C%3CIInformEnable%3E%3E]%5E-.-[Player],[%3C%3CIInformEnable%3E%3E]%5E-.-[ActivateGameObject],[ActivateGameObject]-[MainManu],[ActivateGameObject]*-1[GameManager],[Player]1-1[GameManager],[SpawnManager]1-1[GameManager],[GameManager]-[note:%20enum%20GameState%20RESTART%20PAUSE%20RUN%20STARTED%20END%7Bbg:cornsilk%7D])(http://yuml.me/8bc56e1f)
The GameManger has different states. Depending on the current state it will inform other scripts to activate or deactivate them. For example if the user press pause. The Player and SpawnMager Scripts get disabled and the main Menu gets enables simply by switching to the pause state. So at the start the scripts have to register themselves to their interested states.

## ControlsManager
And here the Controls Manager:
![UML Class diagramm](http://yuml.me/diagram/scruffy/class/[%3C%3CIControl%3E%3E]%5E-.-[KeyboardControlled],[%3C%3CIControl%3E%3E]%5E-.-[KeyboardControlled2],[%3C%3CIControl%3E%3E]%5E-.-[TouchControlled],[%3C%3CIControl%3E%3E]++1-1%3E[ControlsManager],[KeyboardControlled2]++1-1%3E[%3C%3CIPlayerDirection%3E%3E],[TouchControlled]++1-1%3E[%3C%3CIPlayerDirection%3E%3E],[%3C%3CIPlayerDirection%3E%3E]%5E-.-[Walking],[Walking]-%3E[ControlsManager])(http://yuml.me/1c35fe18)
The ControlsManager has one control this can be KeyboardControlled with WASD or KeyboardControlled2 and TouchControlled dependent on the player direction.
In the end walking is one of the main classes that ask for input.
We could put everything in one class but by dividing controls from move implementation we can easily switch out the control types.
