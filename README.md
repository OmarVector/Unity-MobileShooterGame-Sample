# TT : Earth Protectors

Space Shooter Game Prototype .

## Scripts Documentation 
###### Player Scripts Name

1. Mainplayer Class : 
	- Script that hold mainplayer properties such as health.	
2. PlayerController :
	- Tracking player input touch and when jet status will be shown .
3. JetStatusCanvasController :
	- A script control and manage the behavoir of status canvas once its enabled.

###### Weapon Scripts Name

1. WEAPON :
	- A parent class that hold all properties of weapons.
2. MainCannonWeapon Class:
	- Class that hold all properties of MainCannon
	- Its Inherited from WEAPON Class
3. WingCannonClass :
	- Class that hold all properties of 2nd main cannon which is WingCannon
	- Its inherited from WEAPON Class
4. HommingMissileWeapon
	- Class that holds all properties of Missiles.
	- Its inherited from WEAPON Class
5. LaserWeapon :
	- Class that holds all laser weapon properties
	- Inherited from WEAPON class
	- Implement also ISuperWeapon interface 
6. Shield :
	- Class that holds all shield weapon properties
	- Inherited from WEAPON class
	- Implement also ISuperWeapon interface 
7.HommingMissileController :
	- Script that drive the behavior of Missiles .
8. RocketRadar :
	- Script that act as radar to find enemies within its trigger volume.
9. ISuperWeapon :
	- An interface that has only two method ActivateSuperWeapon() and DeactivateSuperWeapon()
10. EnemyWeaponController:
	- Script that control behavior of weapons that may fire on player.
11. SubCannonController :
	- Script that only control the behavior of cannons particles against player.

###### Enemy Scripts Name

1. ENEMY :
	- Parent class that hold all common properties of enemies .
2. AirUnitEnemy :
	- Class that hold properties of any basic air units.
	- Inherted from ENEMY.
3. TerrainUnitEnemy : 
	- Class that hold properties of any terrain units.
	- Inherted from ENEMY
4. EliteEnemy :
	- Class that hold properties for elite units "now only support air unit"
	- Inherted from ENEMY
5. EliteUnitController :
	- Script that conotrol the behavior of EliteUnit .
6. EnemyGroup :
	- A Script where it setup enemy formation and initializing them.

###### Managers Scripts Name

1. ExplosionPoolManager :
	- An object pool that hold all explosions particles .
2. ScoreAndDropManager :
	- A script that manage level score .
	- An Object pool for all drops .
3. ShipDataManager :
	- A SingleTone instance that hold all ship data such as how many lasers player will have and coins...etc .
4. UI_MainMenuManger :
	- A script that manage and animate MainMenu canvas.
	
###### General Scripts Name 

1. Drop : 
	- class hold properties for drops type and behavior when they are collected.
2. ExplosionContainer :
	- Its a scriptable object storing two differnt types of explosions.
	- 1st list for terrains explosion "larger scale"
	- 2nd list for air unit explosion .
3.ParticlesCallback :
	- its a simple script that should be attached to any particles that has to be returned back to explosions pool once its finished playing.
	
###### How to try the game :

- You can download directly the APK https://drive.google.com/file/d/1PxKn2oINoLdL4IG3TOqhWVfLVwvwyxWk/view?usp=sharing .

OR 

1. Make a clone to the project and try it inside editor.
2. Once its downloaded, switch the project platform to android for correct resolution , since the game designed for portrait mode.
3. Wait until everything is loaded and press play.

This a gif animation of how it looks like https://gfycat.com/UnhealthyBabyishAlabamamapturtle 

###### Game Rules :
1. Shoot all enemies and safe the earth .
2. Collect the drops 
	- yellow drops : Coins 
	- Red drops : HealthKit "currently no limit for player health"
	- Blue drops : Shield " by default players has 100 avaible shield to test "
	- Orange drops : Laser " by default players has 100 avaiable lasers to test "
	- Green drops : powerup where it increate the fire rate for both maincannon and wingcannon. max collected 8 per level.
3. Remove your finger from screen to access the ship status canvas
	- Use shield to enable shield super weapon
	- Use Laser to enable laser super weapon
	- or simply press back to exit the level.
4. Only collected Coins will be saved inside ShipDataManager.

###### Well Known Issues :

 Funny Physics behavior , not much .
 
## Note :

1. The game runs on 30-40 fps on Hawuii G8 which is 4 years old device.
2. Alll assets here created by me excpet for trees and some rocks where I did re-texturing for both of them, but they were a pack from unity store.
3. All custom shaders are created using AmplifyShaderEditor .
4. Before doing a build for mobile, make sure to check PlayerController script at Line 102 just to save a little bit of performance .
5. Explosion particles is an assetbundle for unity store.
6. This is a 5 day project I did :
	- everything is made except for 3D assets, I only created the space ship and basic enemy unit especaily for this project, however,I've imported all other assets I've already to save time. .
	

	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
