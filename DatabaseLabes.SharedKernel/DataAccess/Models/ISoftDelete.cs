using System;

namespace DatabaseLabes.SharedKernel.DataAccess.Models
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }

        DateTime? Deleted { get; set; }
    }
}