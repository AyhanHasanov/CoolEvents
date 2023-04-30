using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolEvents.ViewModels.Events
{
    public class EventEditVM
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Event name: ")]
        [MaxLength(64, ErrorMessage = "Event name is too long!")]
        [MinLength(2, ErrorMessage = "Event name is too short!")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Description of the event: ")]
        [MaxLength(255, ErrorMessage = "Event description is too long!")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Date of the event: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [NotMapped]
        [DisplayName("Upload image: ")]
        public IFormFile? ImageFile { get; set; }

        public string? ImageUrl { get; set; }
    }
}
