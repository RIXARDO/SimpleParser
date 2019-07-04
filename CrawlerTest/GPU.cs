using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlerTest
{
    public class GPU
    {

        public int GPUID{ get;set;}

        public string Manufacture { get; set; }

        public string Name { get; set; }

        public string Architecture { get; set; }

        public int BoostClock { get; set; }

        public int FrameBuffer { get; set; }

        public int MemorySpeed { get; set; }

        public int AverageBench { get; set; }

        //public List<Price> prices { get; set; }

        //public ICollection<BuildEntity> BuildEntities { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                GPU gpu = (GPU)obj;
                return (this.GPUID == gpu.GPUID);
            }
        }

        public override int GetHashCode()
        {
            return GPUID;
        }
    }
}