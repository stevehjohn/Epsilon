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

        public float Render(float depth)
        {
            for (var x = 0; x < Constants.BoardSize; x++)
            {
                for (var y = 0; y < Constants.BoardSize; y++)
                {
                    var position = Translations.BoardToScreen(x, y);

                    var tile = _map.GetTile(x, y, 0);

                    if (tile == null)
                    {
                        continue;
                    }

                    _spriteBatch.Draw(_tiles, 
                                      new Vector2(position.X, position.Y - tile.Height * Constants.BlockHeight), 
                                      new Rectangle(GetTerrainXOffset(tile.TerrainType), 0, Constants.TileSpriteWidth, Constants.TileSpriteHeight), 
                                      tile.TerrainType == TerrainType.Water ? Color.White * 0.6f : Color.White , 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth); 

                    depth += Constants.DepthIncrement;

                    if (tile.Height < 0)
                    {
                        _spriteBatch.Draw(_tiles,
                                          new Vector2(position.X, position.Y),
                                          new Rectangle(GetTerrainXOffset(TerrainType.Water), 0, Constants.TileSpriteWidth, Constants.TileSpriteHeight),
                                          Color.White * 0.6f, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth);

                        depth += Constants.DepthIncrement;
                    }
                }
            }

            if (HighlightTile != null)
            {
                var position = Translations.BoardToScreen(HighlightTile.X, HighlightTile.Y);

                _spriteBatch.Draw(_tiles,
                                  new Vector2(position.X, position.Y),
                                  new Rectangle(7 * Constants.TileSpriteWidth, 0, Constants.TileSpriteWidth, Constants.TileSpriteHeight),
                                  Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth);

                depth += Constants.DepthIncrement;
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