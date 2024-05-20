using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_B_Client_App.Controllers;

public class AnimationController
{
    private readonly Dictionary<object, Animation> _anims = [];
    private object _lastKey;

    public void AddAnimation(object key, Animation animation)
    {
        _anims.Add(key, animation);
        _lastKey ??= key;
    }

    public void Update(object key, GameTime gameTime)
    {
        if (_anims.TryGetValue(key, out Animation value))
        {
            value.Start();
            _anims[key].Update(gameTime);
            _lastKey = key;
        }
        else
        {
            _anims[_lastKey].Stop();
            _anims[_lastKey].Reset();
        }
    }

    public void Draw(Vector2 position, SpriteBatch spriteBatch)
    {
        _anims[_lastKey].Draw(position, spriteBatch);
    }
}