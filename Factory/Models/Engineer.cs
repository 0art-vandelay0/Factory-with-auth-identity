using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Factory.Models
{
    public class Engineer
    {
        public int EngineerId { get; set; }
        [Required(ErrorMessage = "Engineer's Name required")]
        public string Name { get; set; }
        public List<EngineerMachine> JoinEntities { get; set; }
    }

}