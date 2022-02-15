
namespace Puma.Models
{
    public class PositionPoi
    {
        public PositionPoi(double lat, double lon)
        {
            Latitude = lat;
            Longitude = lon;
        }

        public PositionPoi()
        {

        }
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
