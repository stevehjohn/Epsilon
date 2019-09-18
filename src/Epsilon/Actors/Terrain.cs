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

        public float Render(float depth)
        {
            for (var x = 0; x < Constants.BoardSize; x++)
            {
                for (var y = 0; y < Constants.BoardSize; y++)
                {
                    var position = Translations.BoardToScreen(x, y);

                    // var tile = _map.GetTile(x, y); // 0°
                    var tile = _map.GetTile(y, 24 - x); // 90°
                    // var tile = _map.GetTile(28 - x, 28 - y); // 180°
                    // var tile = _map.GetTile(28 - y, x); // 270°

                    if (tile == null)
                    {
                        continue;
                    }

                    Console.WriteLine(tile.Height);

                    _spriteBatch.Draw(_tiles, 
                                      new Vector2(position.X, position.Y - tile.Height * Constants.BlockHeight), 
                                      new Rectangle(GetTerrainXOffset(tile.TerrainType), 0, Constants.TileSpriteWidth, Constants.TileSpriteHeight), 
                                      Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth); 

                    depth += Constants.DepthIncrement;
                }
            }

            return depth;
        }

        private int GetTerrainXOffset(TerrainType terrainType)
        {
            switch (terrainType)
            {
                case TerrainType.Grass:
                    return 0;
                case TerrainType.Sand:
                    return 54;
                case TerrainType.Soil:
                    return 108;
                case TerrainType.Rock:
                    return 162;
                case TerrainType.Snow:
                    return 216;
                case TerrainType.Water:
                    return 324;
                default:
                    throw new ArgumentOutOfRangeException(nameof(terrainType), terrainType, null);
            }
        }
    }
}