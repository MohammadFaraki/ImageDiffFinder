using System.ComponentModel.DataAnnotations;

namespace ImageDiffFinder.Models.ViewModels
{
    public class SignInVM
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Language")]
        public string Language { get; set; }

        //[Required]
        //[Display(Name = "Remember Me")]
        //public bool RememberMe { get; set; }

        /*
         * This property has moved to the AppState class which is not binded to any razor page
         * 
         * Don't use [BindProperty] for properties which are registered services and are injected and used to persist state:
	     * Propeties of a razor page, which are decorated with [BindProperty], can't be used
	     * as registered services(be injected to class), for example to store state of the 
	     * app in them, because they will be newed and data will be lost from them 
	     * each time the page is leaved
         */
        // public string Token { get; set; }
    }
}
