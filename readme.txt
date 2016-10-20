This project was developed in Unity 5.4.1.

# The Game

The player starts off with an initial set of cores. You can build new cores
within your territory. The cores send out units periodically which
automatically start moving to the closest opponent core, shooting along the
way.

Destroying an enemy core gives you more build points, allowing you to build
more cores. Each core costs the player build points.

The aim of the game is to strategically place cores to destroy all enemy cores,
before the enemy destroys you!

# Controls

Tap the floor within your territory (colored blue) to place a core. Note that
this will only work if you have enough build points (visible in the top left
hand corner)

Tilting the device sideways to rotate the camera around the map.

Touch and drag to move the camera on the map.

There is also a top down view which will be activated when the device is laid
down face up.

# Models

All models in the game are simple geometric shapes, cylinders for cores and
cubes (balanced on a corner) for units.

# App Certification

A certification report is attached that passes almost all tests.

# Lighting and Shading

## Floor Shader

A custom shader was implemented that not only made a grid, but also coloured
the lines in the area surrounding a core to represent the territory the core
owned.

The grid draw code is based on code from http://www.madebyevan.com/shaders/grid.

The intensity of the colour fades as you reach the border of a core’s
territory. Cores act as light sources and an array of the cores' position as
well as their colour is passed to the shader.

For every pixel, the shader determines which area of the grid it falls on, i.e
should it's primary colour be black (the tiles) or white (the grid lines). It
then find's the closest core to that pixel that is whitin the territory range,
and calculates the appropriate tint for the pixel. If the pixel is close to the
boundry of the core's territory, the intensity of the tint is decreased, to
give the effect of the colour fading as it reaches the boundry.

### Shadows

Finally shadow tints are determined and applied appropiately, so that the
colour of the shadows matches the territory of the shadow is cast in. (Note
that shadows are only drawn within territory.)

## Unit Shader

Phong Illumination was used on the units, with the cores acting as bright light
sources.

## Youtube link
https://www.youtube.com/watch?v=aUrV2GPMNI0&edit=vd
