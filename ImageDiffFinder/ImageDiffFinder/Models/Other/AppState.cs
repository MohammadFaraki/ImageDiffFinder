using ImageDiffFinder.Enums;

namespace ImageDiffFinder.Models.Other
{
    /// <summary>
    /// 
    /// </summary>
    public class AppState : WebUtils.Models.AppStateBase
    {
        public long PersonId { get; set; }
        public Person Person { get; set; }
    }
}
