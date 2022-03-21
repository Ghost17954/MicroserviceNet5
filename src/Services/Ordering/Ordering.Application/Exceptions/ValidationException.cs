using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Exceptions
{
    public class ValidationException:ApplicationException
    {
        public Dictionary<string, string[]> Errors { get; }

        public ValidationException():base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }
      
        public ValidationException(IEnumerable<ValidationFailure> validationFailures):this()
        {
            Errors=validationFailures
                .GroupBy(e=>e.PropertyName,e=>e.ErrorMessage)
                .ToDictionary(failureGrp=>failureGrp.Key,failureGrp=>failureGrp.ToArray());
        }
    }
}
