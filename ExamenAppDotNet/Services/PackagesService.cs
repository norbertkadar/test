using ExamenAppDotNet.Models;
using ExamenAppDotNet.ViewModels.PackagesViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExamenAppDotNet.Services
{
    public interface IPackagesService
    {
        PackagePostModel AddPackage(PackagePostModel package);
        Package UpsertPackage(int id, PackagePostModel package);
        Package DeletePackage(int id);
        Package GetPackage(int id);
        List<PackageGetModel> GetAllPackages(string filter);
        List<PackageGetModel> GrupByExpeditorPackages(string expeditor);
        List<DestinatarModel> GetDestinatariSortetByCost();
    }
    public class PackagesService : IPackagesService
    {
        private ExamDbContext context;
        
        public PackagesService(ExamDbContext context)
        {
            this.context = context;
        }

        public PackagePostModel AddPackage(PackagePostModel packagePostModel)
        {
            var package = PackagePostModel.ToPackage(packagePostModel);

            if (package == null)
            {
                return null;
            }
            context.Packages.Add(package);
            context.SaveChanges();

            return packagePostModel;
        }

        public Package UpsertPackage(int id, PackagePostModel packagePostModel)
        {
            var existingPackage = context.Packages.AsNoTracking().FirstOrDefault(c => c.Id == id);

            if (existingPackage == null)
            {
                var package = PackagePostModel.ToPackage(packagePostModel);
                context.Packages.Add(package);
                context.SaveChanges();
                return package;
            }

            var resPackage = PackagePostModel.ToPackage(packagePostModel);
            resPackage.Id = id;
            context.Packages.Update(resPackage);
            context.SaveChanges();

            return resPackage;
        }

        public Package DeletePackage(int id)
        {
            var found = context.Packages.FirstOrDefault(package => package.Id == id);

            if (found == null)
                return null;

            context.Packages.Remove(found);
            context.SaveChanges();

            return found;
        }

        public List<PackageGetModel> GetAllPackages(string filter)
        {
            var result = context.Packages
                .Where(c => string.IsNullOrEmpty(filter) || c.Expeditor.Contains(filter))
                .Select(packagePostModel => new PackageGetModel
            {
                Id = packagePostModel.Id,
                CountryOrigin = packagePostModel.CountryOrigin,
                Expeditor = packagePostModel.Expeditor,
                DestinationCountry = packagePostModel.DestinationCountry,
                Destinatar = packagePostModel.Destinatar,
                Cost = packagePostModel.Cost,
                TrackingCode = packagePostModel.TrackingCode
            }).ToList();

            return result;  

        }

        public List<PackageGetModel> GrupByExpeditorPackages(string expeditor)
        {
            var result = context.Packages
                .Where(c => c.Expeditor == expeditor)
                .Select(packagePostModel => new PackageGetModel
                {
                    Id = packagePostModel.Id,
                    CountryOrigin = packagePostModel.CountryOrigin,
                    Expeditor = packagePostModel.Expeditor,
                    DestinationCountry = packagePostModel.DestinationCountry,
                    Destinatar = packagePostModel.Destinatar,
                    Cost = packagePostModel.Cost,
                    TrackingCode = packagePostModel.TrackingCode
                }).ToList();

            return result;

        }

        public Package GetPackage(int id)
        {
            Package package = context.Packages.FirstOrDefault(pac => pac.Id ==id);
            return package;
        }

        public List<DestinatarModel> GetDestinatariSortetByCost()
        {
            var result = context.Packages
                .OrderBy(p=>p.Cost)
                .Select(packagePostModel => new DestinatarModel
                {
                    Destinatar = packagePostModel.Destinatar,
                    Cost = packagePostModel.Cost,
                }).ToList();

            return result;
        }
    }
}
