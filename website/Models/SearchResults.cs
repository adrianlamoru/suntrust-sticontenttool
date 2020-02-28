using st1001.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace st1001.website.Models
{
    public class SearchResultsViewModel 
    {
        public IEnumerable<DashboardProjectViewModel> Projects { get; set; }
        public IEnumerable<MediaAssetViewModel> Assets { get; set; }

        public SearchResultsViewModel() {
            Projects = new List<DashboardProjectViewModel>();
            Assets = new List<MediaAssetViewModel>();
        }
    }
}