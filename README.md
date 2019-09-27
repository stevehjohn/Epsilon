# Epsilon - ε

Improved isometric engine based upon learning experiences with Δ.

## To Do / Notes

- Inertial scrolling
- Terrain manipulation?
- Not just height based terrain colouring. Might want areas of desert, mountains grassland etc...
- Unit tests?
- Animated water overlays?
- Quadrant seasons?
- Prevent scrolling too far from map edge
- Don't use the shaded water edges above map edge, only on board edge
- Sky is overlapping raised tile edges
- LAVA!!!
- Water bug - need to render short bit of water when a tile is directly below
- Day/Night cycle?
- Weather
- Sky to account for scale
- Waterfall fade over world edge

## Controls

### Viewport

- <kbd>Left Mouse</kbd> Drag map
- <kbd>Right Arrow</kbd> Rotate 90° 
- <kbd>Left Arrow</kbd> Rotate -90°
- <kbd>S</kbd> Toggle scale

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
