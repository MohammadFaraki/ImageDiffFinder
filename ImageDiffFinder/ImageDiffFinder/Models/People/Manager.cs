using ImageDiffFinder.Models.Other;
using System.ComponentModel.DataAnnotations;

namespace ImageDiffFinder.Models
{
    public class Manager : Person
    {
        //[Required]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        //public Department Department { get; set; }
        //public int? DepartmentID { get; set; }
    }
}
