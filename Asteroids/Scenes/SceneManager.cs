using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asteroids.Scenes
{
    public class SceneManager
    {
        private static SceneManager _sceneManager;
        private BaseScene _scene;

        public static SceneManager Get()
        {
            if (_sceneManager == null)
                _sceneManager = new SceneManager();
            return _sceneManager;
        }

        public IScene Init<T>(Form form) where T : BaseScene, new()
        {
            if (_scene != null)
                _scene.Dispose();

            _scene = new T();
            _scene.Init(form);
            return _scene;
        }

    }
}
