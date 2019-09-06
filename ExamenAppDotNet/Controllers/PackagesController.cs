using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamenAppDotNet.Models;
using ExamenAppDotNet.Services;
using ExamenAppDotNet.ViewModels.PackagesViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamenAppDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private IPackagesService packagesService;

        public PackagesController(IPackagesService packagesService)
        {
            this.packagesService = packagesService;
        }

        // GET: api/Packages
        [HttpGet("{id}")]
        [Authorize(Roles = "Regular, Moderator, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public IActionResult GetPackage(int id)
        {
            var result = packagesService.GetPackage(id);

            if (result== null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Regular, Moderator, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public IEnumerable<PackageGetModel> GetAllPackages(string filter)
        {
            return packagesService.GetAllPackages(filter);
        }

        [HttpGet("destinatari")]
        [Authorize(Roles = "Regular, Moderator, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public IEnumerable<DestinatarModel> GetAllDestinatari()
        {
            return packagesService.GetDestinatariSortetByCost();
        }

        [HttpGet("expeditor")]
        [Authorize(Roles = "Regular, Moderator, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public IEnumerable<PackageGetModel> GetPackagesGrupedByExpeditori(string expeditor)
        {
            return packagesService.GrupByExpeditorPackages(expeditor);
        }


        // POST: api/Packages
        [Authorize(Roles = "Moderator, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public IActionResult Post([FromBody] PackagePostModel package)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(package);
            }
            packagesService.AddPackage(package);
            return Ok(package);
        }

        // PUT: api/Packages/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] PackagePostModel package)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(package);
            }
            var result = packagesService.UpsertPackage(id, package);

            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = packagesService.DeletePackage(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
