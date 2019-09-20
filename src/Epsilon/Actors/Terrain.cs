// #define SlowRender

using System;
using Epsilon.Environment;
using Epsilon.Infrastructure;
using Epsilon.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Epsilon.Actors
{
    public class Terrain : IActor
    {
        private SpriteBatch _spriteBatch;
        private ContentManager _contentManager;
        private Texture2D _tiles;
        private Coordinates _previousPosition = new Coordinates(-1, -1);
        private byte[,] _tileMap;
        private float _depth;

        private readonly Map _map;
        private readonly Coordinates[,] _screenToTileMap;

        public bool UpdateTileMap { get; set; }

        public Terrain(Map map)
        {
            _map = map;
            _screenToTileMap = new Coordinates[Constants.ScreenBufferWidth, Constants.ScreenBufferHeight];
        }

        public void Initialise()
        {
        }

        public void LoadContent(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            _contentManager = contentManager;
            _spriteBatch = spriteBatch;

            _tiles = _contentManager.Load<Texture2D>("tile-set");

            GenerateTileMap();
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

            if (_previousPosition.X != _map.Position.X || _previousPosition.X != _map.Position.X || UpdateTileMap)
            {
                Array.Clear(_screenToTileMap, 0, _screenToTileMap.Length);
            }

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
                        Draw(position.X, position.Y, h, tile.TerrainType, x, y);
                    }

                    if (tile.Height < 0)
                    {
                        Draw(position.X, position.Y, 0, TerrainType.Water);
                    }
                }

                var mouseState = Mouse.GetState();

                if (mouseState.X >= 0 && mouseState.X < Constants.ScreenBufferWidth && mouseState.Y >= 0 && mouseState.Y < Constants.ScreenBufferHeight)
                {
                    var tile = _screenToTileMap[mouseState.X, mouseState.Y];

                    if (tile != null)
                    {
                        var position = Translations.BoardToScreen(tile.X, tile.Y);

                        Console.WriteLine($"{tile.X}, {tile.Y}");

                        Draw(position.X, position.Y, _map.GetTile(tile.X, tile.Y).Height, TerrainType.Highlight);
                    }
                }
            }

            _previousPosition = _map.Position;

            UpdateTileMap = false;

            return _depth;
        }

        private void Draw(int x, int y, int height, TerrainType? terrainType, int? tileX = null, int? tileY = null)
        {
            _spriteBatch.Draw(_tiles,
                              new Vector2(x, y - height * Constants.BlockHeight),
                              new Rectangle(GetTerrainXOffset(terrainType ?? GetDefaultTerrainType(height)), 0, Constants.TileSpriteWidth, Constants.TileSpriteHeight),
                              GetColor(terrainType ?? GetDefaultTerrainType(height), height), 0, Vector2.Zero, Vector2.One, SpriteEffects.None, _depth);

            _depth += Constants.DepthIncrement;

            if (tileX.HasValue && tileY.HasValue)
            {
                AddTileToScreenMap(x, y, tileX.Value, tileY.Value, height);
            }
        }

        private static TerrainType GetDefaultTerrainType(int height)
        {
            if (height < -4)
            {
                return TerrainType.Soil;
            }

            if (height < 5)
            {
                return TerrainType.Sand;
            }

            if (height < 8)
            {
                return TerrainType.Soil;
            }

            if (height < 20)
            {
                return TerrainType.Grass;
            }

            if (height < 30)
            {
                return TerrainType.Rock;
            }

            return TerrainType.Snow;
        }

        private void AddTileToScreenMap(int sx, int sy, int tx, int ty, int height)
        {
            if (_previousPosition.X == _map.Position.X && _previousPosition.Y == _map.Position.Y && ! UpdateTileMap)
            {
                return;
            }

            var coordinates = new Coordinates(tx, ty);

            for (var x = 0; x < Constants.TileSpriteWidth; x++)
            {
                for (var y = 0; y < Constants.TileHeight; y++)
                {
                    var ax = sx + x;
                    var ay = sy + y - height * Constants.BlockHeight;

                    if (ax >= 0 && ax < Constants.ScreenBufferWidth && ay >= 0 && ay < Constants.ScreenBufferHeight && _tileMap[x, y] > 0)
                    {
                        _screenToTileMap[ax, ay] = coordinates;
                    }
                }
            }
        }

        private void GenerateTileMap()
        {
            var terrains = (TerrainType[]) Enum.GetValues(typeof(TerrainType));

            var colours = new Color[Constants.TileSpriteWidth * terrains.Length * Constants.TileSpriteHeight];

            _tiles.GetData(colours);

            _tileMap = new byte[Constants.TileSpriteWidth, Constants.TileSpriteHeight];

            for (var x = 0; x < Constants.TileSpriteWidth; x++)
            {
                for (var y = 0; y < Constants.TileSpriteHeight; y++)
                {
                    _tileMap[x, y] = colours[y * Constants.TileSpriteWidth * terrains.Length + x + (int) TerrainType.Reference * Constants.TileSpriteWidth].A;
                }
            }
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