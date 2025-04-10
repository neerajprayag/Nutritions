﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Nutritions.Model
{
    public class AminoAcids
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Benefits { get; set; }
        public string Diet {  get; set; }
        public string Sources    { get; set; }
        public string Rda { get; set; }
        public string Supplement_details {  get; set; } 
    }
}
