﻿# Epsilon - ε

Improved isometric engine based upon learning experiences with Δ.

![Screenshot](https://github.com/stevehjohn/Epsilon/blob/master/ScreenShot.jpg)

## To Do / Notes

- Inertial scrolling
- Terrain manipulation
- Not just height based terrain colouring. Might want areas of desert, mountains grassland etc...
- Unit tests?
- Animated water overlays? 
- Quadrant seasons?
- Prevent scrolling too far from map edge
- Don't use the shaded water edges above map edge, only on board edge
- Sky is overlapping raised tile edges
- Terrain edges to account for the height normalisation step
- LAVA!!!
- Minor sky bug
- Water bug

## Controls

### Viewport

- <kbd>Left Mouse</kbd> Drag map
- <kbd>Right Arrow</kbd> Rotate 90° 
- <kbd>Left Arrow</kbd> Rotate -90°

### Environment

- <kbd>Up Arrow</kbd> Raise water level
- <kbd>Down Arrow</kbd> Lower water level
- <kbd>Left Ctrl + Up Arrow</kbd> Raise water level one step
- <kbd>Left Ctrl + Down Arrow</kbd> Lower water level one step

### Feature Toggles

- <kbd>E</kbd> Edge rendering

## Scaling / Zooming

On an integer zoom level, SamplerState.PointClamp looks best.

When non integer zoomed, solid sprites look good with Anisotropic but translucent ones look bad on the edges. Investigate changing sampler state during batch?
