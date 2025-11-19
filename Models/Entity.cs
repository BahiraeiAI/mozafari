using System;
namespace RoshedTehran.Models
{
	public class Entity
	{
		public Guid Id { get; set; }
		public Uri? Domain { get; set; }
		public Uri? URI { get; set; }
		public Uri? Instagram { get; set; }
        public Uri? GoogleMapIdentifierURI { get; set; }

        public string? TitleTag { get; set; }
		public string? MetaDescription { get; set; }
		public string? DOM { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Location { get; set; }
		public string? Email { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

		public DateOnly RegistrationDate { get; set; }
		//here are are presuming each website has only 1 number,email,instagram,...

    }
}

