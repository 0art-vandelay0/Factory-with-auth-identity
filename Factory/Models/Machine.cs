using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Factory.Models
{
    public class Machine
    {
        public int MachineId { get; set; }
        [Required(ErrorMessage = "Machine's Name required")]
        public string Name { get; set; }
        public List<EngineerMachine> JoinEntities { get; set; }
    }
}