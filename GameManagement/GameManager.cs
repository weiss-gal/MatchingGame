using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame.GameManagement
{
    class GameManager
    {
        LoadManager loadManager = null;
        public GameManager()
        {
            
        }

        public LoadManager getLoadManager(string fileName)
        {
            if (loadManager != null)
                throw new Exception($"Attemping to load a new file '{fileName}' while another file load is in progress");

            loadManager = new LoadManager(fileName);
            
            return loadManager;
        }
    }
}
