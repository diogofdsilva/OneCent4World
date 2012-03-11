using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCW.DAL.IndependentLayerEntities
{
    public class IndependentOngProfile
    {

        public String Name { get; set; }
        public String Url { get; set; }
        public String Email { get; set; }
        public int TypeId { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String PostalCode { get; set; }
        public String City { get; set; }
        public int Country { get; set; }
        public String Password { get; set; }
    }

    public class IndependentOrganization
    {

        public String Name { get; set; }
        public int Id { get; set; }
    }
}
