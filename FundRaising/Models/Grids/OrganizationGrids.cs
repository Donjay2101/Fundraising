using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FundRaising.ViewModels;

namespace FundRaising.Models.Grids
{
    public class OrganizationsGrid : GridMvc.Grid<OrganizationViewModel>
    {
        public OrganizationsGrid(IQueryable<OrganizationViewModel> items) 
            :base(items)
        {

        }
    }
}
