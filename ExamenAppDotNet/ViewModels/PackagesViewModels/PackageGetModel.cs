using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamenAppDotNet.ViewModels.PackagesViewModels
{
    public class PackageGetModel
    {
        public int Id { get; set; }
        public string CountryOrigin { get; set; }
        public string Expeditor { get; set; }
        public string DestinationCountry { get; set; }
        public string Destinatar { get; set; }
        public double Cost { get; set; }
        public string TrackingCode { get; set; }
    }
}
