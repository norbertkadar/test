using ExamenAppDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamenAppDotNet.ViewModels.PackagesViewModels
{
    public class PackagePostModel
    {
        public string CountryOrigin { get; set; }
        public string Expeditor { get; set; }
        public string DestinationCountry { get; set; }
        public string Destinatar { get; set; }
        public double Cost { get; set; }
        public string TrackingCode { get; set; }



        public static Package ToPackage(PackagePostModel packagePostModel)
        {
            return new Package()
            {
                CountryOrigin = packagePostModel.CountryOrigin,
                Expeditor = packagePostModel.Expeditor,
                DestinationCountry = packagePostModel.DestinationCountry,
                Destinatar = packagePostModel.Destinatar,
                Cost = packagePostModel.Cost,
                TrackingCode = packagePostModel.TrackingCode
            };
        }

    }

    

}
