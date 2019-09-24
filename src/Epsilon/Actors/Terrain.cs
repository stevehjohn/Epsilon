// #define SlowRender

using System;
using Epsilon.Coordination;
using Epsilon.Environment;
using Epsilon.Infrastructure;
using Epsilon.Infrastructure.Configuration;
using Epsilon.Maths;
using Epsilon.State;
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
        private Texture2D _scenery;
        private Texture2D _sky;
        private Coordinates _previousPosition = new Coordinates(-1, -1);
        private byte[,] _tileMap;
        private float _depth;

        private readonly Map _map;
        private readonly Coordinates[,] _screenToTileMap;

        public bool UpdateTileMap { get; set; }

        public Coordinates SelectedTile { get; set; }

        public Terrain(Map map, EventManager eventManager)
        {
            _map = map;
            _screenToTileMap = new Coordinates[Constants.ScreenBufferWidth, Constants.ScreenBufferHeight];

            eventManager.AddSubscription(EventType.RotationChanged, () => UpdateTileMap = true);
        }

        public void Initialise()
        {
        }

        public void LoadContent(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            _contentManager = contentManager;
            _spriteBatch = spriteBatch;

            _tiles = _contentManager.Load<Texture2D>("tile-set");
            _scenery = _contentManager.Load<Texture2D>("scenery");
            _sky = _contentManager.Load<Texture2D>("sky");

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

                    var baseHeight = Math.Min(_map.GetTile(x + 1, y)?.Height ?? tile.Height, _map.GetTile(x, y + 1)?.Height ?? tile.Height);

                    if (baseHeight > tile.Height)
                    {
                        baseHeight = tile.Height;
                    }

                    if (x == 0 || y == 0)
                    {
                        var skyBase = position.Y - Constants.SkySpriteHeight - Constants.SeaFloor * Constants.BlockHeight + Constants.TileHeightHalf;

                        if (x == 0)
                        {
                            _spriteBatch.Draw(_sky,
                                              new Vector2(position.X, skyBase),
                                              new Rectangle(Constants.SkySpriteWidth, 0, Constants.SkySpriteWidth, Constants.SkySpriteHeight),
                                              new Color(GameState.Brightness, GameState.Brightness, GameState.Brightness), 0, Vector2.Zero, Vector2.One, SpriteEffects.None, _depth);
                        }

                        _depth += Constants.DepthIncrement;

                        if (y == 0)
                        {
                            _spriteBatch.Draw(_sky,
                                              new Vector2(position.X, skyBase),
                                              new Rectangle(0, 0, Constants.SkySpriteWidth, Constants.SkySpriteHeight),
                                              new Color(GameState.Brightness, GameState.Brightness, GameState.Brightness), 0, Vector2.Zero, Vector2.One, SpriteEffects.None, _depth);
                        }

                        _depth += Constants.DepthIncrement;
                    }

                    for (var h = baseHeight; h <= tile.Height; h++)
                    {
                        Draw(position.X, position.Y, h, h > tile.Height - 2 ? tile.TerrainType : tile.IsEdge ? TerrainType.Rock : Map.GetDefaultTerrainType(h + tile.EdgeOffset), x, y);
                    }

                    if (tile.SceneryType != null)
                    {
                        DrawScenery(position.X, position.Y, tile.Height, tile.SceneryType.Value);
                    }

                    var edge = tile.IsEdge || x == Constants.BoardSize - 1 || y == Constants.BoardSize - 1;

                    if (edge && AppSettings.Instance.Rendering.RenderBoardEdges)
                    {
                        for (var h = Constants.SeaFloor; h <= tile.Height; h++)
                        {
                            if (_map.GetTile(x, y + 1) == null || y == Constants.BoardSize - 1)
                            {
                                DrawEdge(position.X, position.Y, h, tile.IsEdge ? TerrainType.Rock : Map.GetDefaultTerrainType(h + tile.EdgeOffset), true);
                            }

                            if (_map.GetTile(x + 1, y) == null || x == Constants.BoardSize - 1)
                            {
                                DrawEdge(position.X, position.Y, h, tile.IsEdge ? TerrainType.Rock : Map.GetDefaultTerrainType(h + tile.EdgeOffset), false);
                            }
                        }

                        if (tile.IsEdge &&  GameState.WaterLevel > tile.Height)
                        {
                            var colour = new Color(GameState.Brightness, GameState.Brightness, GameState.Brightness) * 0.6f;

                            if (_map.GetTile(x, y + 1) == null)
                            {
                                for (var h = tile.Height + 1; h > -100; h--)
                                {
                                    // TODO: Magic number -2
                                    DrawEdge(position.X, position.Y + 5, h, TerrainType.WaterLeftEdge, true, colour, -2);
                                }
                            }

                            if (_map.GetTile(x + 1, y) == null)
                            {
                                for (var h = tile.Height + 1; h > -100; h--)
                                {
                                    // TODO: Magic number -2
                                    DrawEdge(position.X, position.Y + 5, h, TerrainType.WaterRightEdge, false, colour, -2);
                                }
                            }
                        }
                    }

                    if (tile.Height < GameState.WaterLevel)
                    {
                        Draw(position.X, position.Y, GameState.WaterLevel, TerrainType.Water);

                        if (edge)
                        {
                            for (var i = GameState.WaterLevel; i > tile.Height; i--)
                            {
                                if (_map.GetTile(x, y + 1) == null || y == Constants.BoardSize - 1)
                                {
                                    Draw(position.X, position.Y, i, TerrainType.WaterLeftEdge);
                                }

                                if (_map.GetTile(x + 1, y) == null || x == Constants.BoardSize - 1)
                                {
                                    Draw(position.X, position.Y, i, TerrainType.WaterRightEdge);
                                }
                            }
                        }
                    }
                }
            }

            var mouseState = Mouse.GetState();

            SelectedTile = null;

            if (mouseState.X >= 0 && mouseState.X < Constants.ScreenBufferWidth && mouseState.Y >= 0 && mouseState.Y < Constants.ScreenBufferHeight)
            {
                var tile = _screenToTileMap[mouseState.X, mouseState.Y];

                if (tile != null)
                {
                    var position = Translations.BoardToScreen(tile.X, tile.Y);

                    var mapTile = _map.GetTile(tile.X, tile.Y);

                    if (mapTile != null)
                    {
                        Draw(position.X, position.Y, mapTile.Height, TerrainType.Highlight);

                        SelectedTile = tile;
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
                              new Rectangle(GetTerrainXOffset(terrainType ?? Map.GetDefaultTerrainType(height)), 0, Constants.TileSpriteWidth, Constants.TileSpriteHeight),
                              GetColor(terrainType ?? Map.GetDefaultTerrainType(height), height), 0, Vector2.Zero, Vector2.One, SpriteEffects.None, _depth);

            _depth += Constants.DepthIncrement;

            if (tileX.HasValue && tileY.HasValue)
            {
                AddTileToScreenMap(x, y, tileX.Value, tileY.Value, height);
            }
        }

        private void DrawEdge(int x, int y, int height, TerrainType? terrainType, bool left, Color? colour = null, int yOffset = 0)
        {
            _spriteBatch.Draw(_tiles,
                              new Vector2(x + (left ? 0 : Constants.TileSpriteWidthHalf), y - height * Constants.BlockHeight + yOffset),
                              new Rectangle(GetTerrainXOffset(terrainType ?? Map.GetDefaultTerrainType(height)) + (left ? 0 : Constants.TileSpriteWidthHalf),
                                            Constants.TileSpriteHeight,
                                            Constants.TileSpriteWidthHalf,
                                            Constants.TileSpriteHeight),
                              colour ?? new Color(GameState.Brightness, GameState.Brightness, GameState.Brightness), 0, Vector2.Zero, Vector2.One, SpriteEffects.None, _depth);

            _depth += Constants.DepthIncrement;
        }

        private void DrawScenery(int x, int y, int height, SceneryType sceneryType)
        {
            _spriteBatch.Draw(_scenery,
                              new Vector2(x, y - (height + 1) * Constants.BlockHeight - (Constants.ScenerySpriteHeight - Constants.TileSpriteHeight)),
                              new Rectangle((int) sceneryType * Constants.ScenerySpriteWidth, 0, Constants.ScenerySpriteWidth, Constants.ScenerySpriteHeight),
                              GetColor(null, height), 0, Vector2.Zero, Vector2.One, SpriteEffects.None, _depth);

            _depth += Constants.DepthIncrement;
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

            var colours = new Color[Constants.TileSpriteWidth * terrains.Length * Constants.TileSpriteHeight * 2];

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

        private Color GetColor(TerrainType? terrainType, int height)
        {
            if (terrainType == TerrainType.Water || terrainType == TerrainType.WaterLeftEdge || terrainType == TerrainType.WaterRightEdge)
            {
                return new Color(GameState.Brightness, GameState.Brightness, GameState.Brightness) * 0.6f;
            }

            if (height >= GameState.WaterLevel || terrainType == TerrainType.Highlight)
            {
                return new Color(GameState.Brightness, GameState.Brightness, GameState.Brightness);
            }

            var intensity = (int) (255 * ((Constants.SeaFloor * 1.5f - (height - GameState.WaterLevel)) / (Constants.SeaFloor * 1.5f)));

            intensity -= 255 - GameState.Brightness;

            if (intensity < 0)
            {
                intensity = 0;
            }

            return new Color(intensity, intensity, intensity);
        }

        private static int GetTerrainXOffset(TerrainType terrainType)
        {
            return (int) terrainType * Constants.TileSpriteWidth;
        }
    }
}