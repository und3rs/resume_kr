using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PureWorldLib
{
    public class WorldsCreateStartEventArgs : EventArgs
    {
        public World world;

        public WorldsCreateStartEventArgs(World world)
        {
            this.world = world;
        }
    }


    public class WorldsCreateCompletedEventArgs : EventArgs
    {
        public World world;

        public WorldsCreateCompletedEventArgs(World world) {
            this.world = world;
        }
    }

    public class WorldsCreateProgressEventArgs : EventArgs {
        public int percent;

        public WorldsCreateProgressEventArgs(int count) {
            this.percent = (int)((count / Property.VERTICAL_IMG_NUM * Property.HORIZON_IMG_NUM) * 100);
        }
    }
}
