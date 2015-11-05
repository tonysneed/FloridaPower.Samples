using System;

namespace ModelBinding.Web.Models
{
    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static bool TryParse(string input, out Location location)
        {
            location = new Location();
            var parts = input.Split(',');
            if (parts.Length != 2) return false;

            int x, y;
            if (Int32.TryParse(parts[0], out x) && Int32.TryParse(parts[1], out y))
            {
                location.X = x;
                location.Y = y;
                return true;
            }
            return false;
        }
    }
}