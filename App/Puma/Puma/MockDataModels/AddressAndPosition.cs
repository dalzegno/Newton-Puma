using Puma.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Puma.MockDataModels
{
    internal class AddressAndPosition
    {
        public int Id { get; set; } 
        public Address Address { get; set; }
        public PositionPoi Position { get; set; }
    }
}
