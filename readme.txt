The projectt was developed in Unity 5.4.1.

The Game:
Each player starts off with an initial set of cores. You can build new cores within your territory. There is a timer and when the timer hits zero, the cores send out units which automatically start moving to the closest opponent core shooting along the way. Destroying an enemy core gives you more build points, allowing you to build more cores. Each core costs the player build points.
The aim of the game is to strategically place cores to destroy all enemy cores, before the enemy destroys you!

Controls:
•	Touch the floor (within your territory) to place a core
•	Tilt the device to pan the screen
•	Touch and drag to move the camera on the map

App Certification:
•	A certification report is attached that passes almost all tests.

Lighting and Shading:
Floor Shader: 
A custom shader was implemented that not only made a grid, but also coloured the lines on the grid, surrounding the core, to represent the territory it owned. The intensity of the colour fades as you reach the border of a core’s territory.
Cores act as light sources and an array of the cores' position as well as their colour is passed to the shader. For every pixel, the shader determines which area of the grid it falls on, i.e should it's primary colour be black (the tiles) or white (the grid lines). It then find's the closest core that pixel, whitin the territory range, and cslculstes the appropriate tint for the pixel. If the core is close to the boundry of the core's territory, the intensity of the tint is decreased, to give the effect of the colour fading as it reaches the boundry. Finally shadow tints are determined and applied appropiately, so that the colour of the shadows matched that of the core casting it. 

•	Unit Shader: Phong Illuminitation was used on the units, with the cores acting as light sources.
•	A particle system is used to emulate explosions when cores and units are destroyed
•	Cores can cast shadows to the floor. As the floor is black, these shadows have been coloured so they show up.


