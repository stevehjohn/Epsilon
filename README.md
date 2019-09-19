# Epsilon

Improved isometric engine based upon learning experiences with Δ.

## To Dos / Notes

### Scaling / Zooming

On an integer zoom level, SamplerState.PointClamp looks best.

When non integer zoomed, solid sprites look good with Anisotropic but translucent ones look bad on the edges. Investigate changing sampler state during batch?
