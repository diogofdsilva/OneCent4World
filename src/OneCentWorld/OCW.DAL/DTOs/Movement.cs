using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCW.DAL.DTOs
{
    public abstract class Movement
    {
        public abstract string TypeMovement { get; }
        public abstract DateTime DateMovement { get; }
        public abstract string OrganizationMovement { get; }
        public abstract string SourceMovement { get; }
        public abstract string DestinyMovement { get; }
        public abstract Decimal ValueMovement { get; }
        public abstract Decimal DonationMovement { get; }
    }
}
