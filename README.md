# vertigocase

Unity Version: 2019.4.10f1

IMPORTANT REMARKS:  
-Number of Hexagon colors(number of hexagon types) can be controlled by the "Tile Manager" game object in Inspector. Number of colors will be set to length of "Base Hex Elements"   array. Elements of arrays are the HexElements in Prefabs/HexElements folder. If you increase the length of the array, you can add more different HexElements. You can duplicate   already existing HexElements and change their color if you want to add more HexElements than I created.  
-"Wait Between Falls" in "Tile Manager" is seconds that game waits between each fall down of the tiles when a tile is broken.
-"Bomb Countdowns" in "Tile Manager" determines after how much turns bombs will explode.
-Game grid can be changed from "Comtrol Dot Manager" game object in inspector by "Dim N" and "Dim M" varibales. Positions and size of each hexagon will be adjusted for the screen dynamically.
-"Upper Gap Amount" in "Control Dot Manager" is the rate of the blank space at the top of the game. "0.3" means that grid will be start at the %30 percent length from the top. This can also be changed.
-"Wait Between Turns" in "Control Dot Manager" is the seconds that game waits between each rotation of tiles. It is set to 0.25, it can be changed.
-I used prefabs and Instantiated simple objects. With this design, it is easy to add different hexagon designs and animations. Normally for the bomb countdows, a sprite that has a bomb image and colored number can be used. Since I dont have those kind of sprites, I used a canvas and a text for the bomb countdows which will be rendered at the top of the bomb hexagon. It is easy to change it what it supposed to be, but with this version, bomb countdown text can be too big, or too small to detect on different screens. Bomb hexagons are implemented and works fine, if text is too small, please pay attention to that.
-I only added "Assets", "Packages" and "Project Settings" in the repo, you can create the project with these files.
