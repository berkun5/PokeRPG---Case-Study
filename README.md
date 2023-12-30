# PokeRPG---Case-Study
A pokemon themed turn-based RPG game case study.<br><br>
![ezgif com-video-to-gif-converter](https://github.com/berkun5/PokeRPG---Case-Study/assets/80388989/ac6c4083-2604-4f4d-addb-24dba35ff2c9)

## General Info
- <b>Development Hours</b>: ~25h
- <b>Engine</b>: Unity3D
- <b>Editor Version</b>: 2022.3.13f

## Build
- <b>Date</b>: 30.12.2023
- <b>Platfrom</b>: Android
- <b>BuildVersion</b>: 0.1
- <b>Minimum API Level</b>: 22
- <a href="https://drive.google.com/file/d/1MmbiQtBq1Rq8PbGSmBMxPHkmn2sxqhcM/view?usp=sharing">Download APK</a> (20MB)
- <a href="https://drive.google.com/file/d/1wF0sPKZ2lePbFqz4zAZafX5qOfnxwJ2f/view?usp=sharing">Download Developer Build APK</a> (40MB)

  ## Imported Assets
- <a href="https://dotween.demigiant.com">DoTween</a>
- <a href="https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.2/manual/index.html">TextMeshPro</a>
- <a href="https://docs.unity3d.com/Packages/com.unity.2d.sprite@1.0/manual/index.html">SpriteEditor</a>

## Rundown
 #### Data Management
Game data is handled by <b>RemoteDataBase.cs</b> and its inheritors. Each inheritor is responsible for managing a specific type of data and overseeing its save and load processes, whether it's stored remotely or locally using PlayerPrefs.

### Implemented Patterns
#### Service Locator
The project implements the Service Locator pattern, centralizing service management and access. The <b>GameServiceLocator.cs</b> class enables easy registration, retrieval, and late initialization of services.

Using <b>Init and</b> <b>LateStart</b> methods for managers ensures a clean and organized initialization of the execution order sequence, promoting efficient decoupling of components and streamlined dependency management.

#### Model-View-ViewModel
The UI follows the MVVM pattern. Each view is controlled through a ViewModel interface, acting as an intermediary between the front end and the underlying data. The ViewModel retrieves business logic from remote or local data classes, ensuring communication between the UI and the game's data.

#### Observers
- A simple version of <b>ReactiveProperty.cs</b> is implementated as an Observer pattern tailored for handling property changes reactively.
- Similar approach to ReactiveProperty, <b>ObservableList.cs</b> is tailored for handling list structures.

#### Factory
The project implements a Factory Method pattern for character entity creation, featuring a base factory interface <b>ICharacterEntityFactory</b> and different concrete factories. The system utilizes a spawner <b>CharacterEntitySpawner</b> to manage character spawns at specified positions, fostering flexibility and encapsulation.

#### Command
The project implements the Command Pattern for handling actions using the <b>Command.cs</b> base class. The <b>CombatCommandManager.cs</b> manages a queue of commands, executing them in sequence during the Unity Update cycle. Using the update cycle ensures synchronization with frame updates, and maintains consistency with the execution order.
