﻿# Epsilon - ε

Improved isometric engine based upon learning experiences with Δ.

## To Do / Notes

- Inertial scrolling
- Terrain manipulation
- Not just height based terrain colouring. Might want areas of desert, mountains grassland etc...
- Unit tests?
- Make sky tall tile, fading to transparency
- Actor to control stars which move parallaxially
- Animated water overlays?
- Make edges rock and follow sea level + 1
- Change terrain generation so that it gets lower as edges of circle are approached
- Every other edge block has overflow?

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
