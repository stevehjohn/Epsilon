#define SlowRender

using System;
using Epsilon.Environment;
using Epsilon.Infrastructure;
using Epsilon.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Epsilon.Actors
{
    public class Terrain : IActor
    {
        private SpriteBatch _spriteBatch;
        private ContentManager _contentManager;
        private Texture2D _tiles;

        private readonly Map _map;

        private float _depth;

        public Coordinates HighlightTile { get; set; }

        public Terrain(Map map)
        {
            _map = map;
        }

        public void Initialise()
        {
        }

        public void LoadContent(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            _contentManager = contentManager;
            _spriteBatch = spriteBatch;

            _tiles = _contentManager.Load<Texture2D>("tile-set");
        }

        public void UpdateState()
        {
        }

#if SlowRender
        private int i = 1;
#endif

        public float Render(float depth)
        {
            _depth = depth;

#if SlowRender
            var j = 0;
#endif

            for (var x = 0; x < Constants.BoardSize; x++)
            {
                for (var y = 0; y < Constants.BoardSize; y++)
                {
#if SlowRender
                    j++;

                    if (j >= i)
                    {
                        i++;

                        return _depth;
                    }
#endif
                    var position = Translations.BoardToScreen(x, y);

                    var tile = _map.GetTile(x, y);

                    if (tile == null)
                    {
                        continue;
                    }

                    for (var h = Constants.SeaFloor; h <= tile.Height; h++)
                    {
                        Draw(position.X, position.Y, h, tile.TerrainType);
                    }

                    if (tile.Height < 0)
                    {
                        Draw(position.X, position.Y, 0, TerrainType.Water);
                    }
                }
            }

            // TODO: Don't like this.
            if (HighlightTile != null)
            {
                var position = Translations.BoardToScreen(HighlightTile.X, HighlightTile.Y);

                _spriteBatch.Draw(_tiles,
                                  new Vector2(position.X, position.Y),
                                  new Rectangle(7 * Constants.TileSpriteWidth, 0, Constants.TileSpriteWidth, Constants.TileSpriteHeight),
                                  Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, _depth);

                _depth += Constants.DepthIncrement;
            }

            return _depth;
        }

        private void Draw(int x, int y, int height, TerrainType terrainType)
        {
            _spriteBatch.Draw(_tiles,
                              new Vector2(x, y - height * Constants.BlockHeight),
                              new Rectangle(GetTerrainXOffset(terrainType), 0, Constants.TileSpriteWidth, Constants.TileSpriteHeight),
                              terrainType == TerrainType.Water ? Color.White * 0.6f : Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, _depth);

            _depth += Constants.DepthIncrement;
        }

        private int GetTerrainXOffset(TerrainType terrainType)
        {
            switch (terrainType)
            {
                case TerrainType.Grass:
                    return 0;
                case TerrainType.Sand:
                    return 1 * Constants.TileSpriteWidth;
                case TerrainType.Soil:
                    return 2 * Constants.TileSpriteWidth;
                case TerrainType.Rock:
                    return 3 * Constants.TileSpriteWidth;
                case TerrainType.Snow:
                    return 4 * Constants.TileSpriteWidth;
                case TerrainType.Water:
                    return 5 * Constants.TileSpriteWidth;
                default:
                    throw new ArgumentOutOfRangeException(nameof(terrainType), terrainType, null);
            }
        }
    }
}