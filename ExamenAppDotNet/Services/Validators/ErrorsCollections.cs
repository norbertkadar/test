using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamenAppDotNet.Services.Validators
{
    public class ErrorsCollections
    {
        public string Entity { get; set; }
        public List<string> ErrorMessages { get; set; }

        public ErrorsCollections()
        {
            ErrorMessages = new List<string>();
        }

    }
}
