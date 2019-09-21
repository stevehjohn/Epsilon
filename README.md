# Epsilon - ε

Improved isometric engine based upon learning experiences with Δ.

## To Dos / Notes

- Inertial scrolling
- Terrain manipulation
- Not just height based terrain colouring. Might want areas of desert, mountains grassland etc...
- Unit tests?

### Scaling / Zooming

On an integer zoom level, SamplerState.PointClamp looks best.

When non integer zoomed, solid sprites look good with Anisotropic but translucent ones look bad on the edges. Investigate changing sampler state during batch?
