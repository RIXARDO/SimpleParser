using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CrawlerTest
{
    [Serializable]
    public class CPU
    {
        /// <summary>
        /// Id
        /// </summary>
        public int CPUID { get; set; }

        public string Manufacture { get; set; }

        public string ProcessorNumber { get; set; }

        public int NumberOfCores { get; set; }

        public int NumberOfThreads { get; set; }

        /// <summary>
        /// Processor Base Frequency(GHz)
        /// </summary>
        public float PBF { get; set; }

        public string Cache { get; set; }

        public string TDP { get; set; }

        //public List<Price> Prices { get; set; }

        public int AverangeBench { get; set; }

        //public ICollection<BuildEntity> BuildEntities { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                CPU cpu = (CPU)obj;
                return (this.CPUID == cpu.CPUID);
            }
        }

        public override int GetHashCode()
        {
           return CPUID;
        }
    }
}