// #define SlowRender

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

                    int baseHeight;

                    if (x == Constants.BoardSize - 1 || y == Constants.BoardSize - 1)
                    {
                        baseHeight = Constants.SeaFloor;
                    }
                    else
                    {
                        baseHeight = Math.Min(_map.GetTile(x + 1, y)?.Height ?? Constants.SeaFloor, _map.GetTile(x, y + 1)?.Height ?? Constants.SeaFloor);
                    }

                    if (baseHeight > tile.Height)
                    {
                        baseHeight = tile.Height;
                    }

                    for (var h = baseHeight; h <= tile.Height; h++)
                    {
                        Draw(position.X, position.Y, h, tile.TerrainType);
                    }

                    if (tile.Height < 0)
                    {
                        Draw(position.X, position.Y, 0, TerrainType.Water);
                    }
                }

                if (HighlightTile != null)
                {
                    var position = Translations.BoardToScreen(HighlightTile.X, HighlightTile.Y);

                    Draw(position.X, position.Y, 0, TerrainType.Highlight);
                }
            }

            return _depth;
        }

        private void Draw(int x, int y, int height, TerrainType terrainType)
        {
            _spriteBatch.Draw(_tiles,
                              new Vector2(x, y - height * Constants.BlockHeight),
                              new Rectangle(GetTerrainXOffset(terrainType), 0, Constants.TileSpriteWidth, Constants.TileSpriteHeight),
                              GetColor(terrainType, height), 0, Vector2.Zero, Vector2.One, SpriteEffects.None, _depth);

            _depth += Constants.DepthIncrement;
        }

        private static Color GetColor(TerrainType terrainType, int height)
        {
            if (terrainType == TerrainType.Water)
            {
                return Color.White * 0.6f;
            }

            if (height >= 0 || terrainType == TerrainType.Highlight)
            {
                return Color.White;
            }

            var intensity = (int) (255 * ((Constants.SeaFloor * 1.5f - height) / (Constants.SeaFloor * 1.5f)));

            return new Color(intensity, intensity, intensity);
        }

        private static int GetTerrainXOffset(TerrainType terrainType)
        {
            return (int) terrainType * Constants.TileSpriteWidth;
        }
    }
}