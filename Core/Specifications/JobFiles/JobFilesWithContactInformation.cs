using Ardalis.Specification;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.JobFiles
{
    public class JobFilesWithContactInformation : Specification<JobFile>
    {
        public JobFilesWithContactInformation()
        {
            Query.Include(x=>x.ContactId);
        }
    }
}
