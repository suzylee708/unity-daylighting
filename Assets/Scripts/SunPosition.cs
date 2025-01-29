using System;
using UnityEngine;

namespace BrydenWoodUnity.Lighting
{
    /*! 
         * \brief Calculates the sun light. 
         * 
         * CalcSunPosition calculates the suns "position" based on a 
         * given date and time in local time, latitude and longitude 
         * expressed in decimal degrees. It is based on the method 
         * found here: 
         * http://www.astro.uio.no/~bgranslo/aares/calculate.html 
         * The calculation is only satisfiably correct for dates in 
         * the range March 1 1900 to February 28 2100. 
         * \param dateTime Time and date in local time. 
         * \param latitude Latitude expressed in decimal degrees. 
         * \param longitude Longitude expressed in decimal degrees. 
         */

    /// <summary>
    /// A MonoBehaviour for controlling the Sun Light through DateTime and LatLon
    /// </summary>
    public class SunPosition : MonoBehaviour
    {
        #region Public Fields and Properties
        [Header("Scene References:")]
        public SunControlUI_ sunControlUI;

        [Header("Date Time and Position Properties:")]
        public double lat = 51.549090;
        public double lon = -0.074478;
        public int year = 2017;
        public int month = 3;
        public int day = 21;
        public int hour = 9;
        public int minute = 0;
        public double azimuth, altitude;
        public DateTime date;
        #endregion

        #region Private Fields and Propertie
        private const double Deg2Rad = Math.PI / 180.0;
        private const double Rad2Deg = 180.0 / Math.PI;
        #endregion

        #region MonoBehaviour Methods

        private void Start()
        {
           if (sunControlUI != null)
            {
                sunControlUI.enabled = true;
                sunControlUI.SetCurrentDateTime();
            }
        }

        private void Update()
        {
            UpdateDateTime();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Updates the Date and Time of the Sun
        /// </summary>
        public void UpdateDateTime()
        {
            date = dat();
            CalculateSunPosition(date, lat, lon);
            transform.eulerAngles = new Vector3((float)altitude, (float)azimuth, 0);
        }

        /// <summary>
        /// Sets a new Date and Time for the Sun
        /// </summary>
        /// <param name="dateTime">New Date & Time</param>
        public void SetNewDate(DateTime dateTime)
        {
            year = dateTime.Year;
            month = dateTime.Month;
            day = dateTime.Day;
            hour = dateTime.Hour;
            minute = dateTime.Minute;
        }
        #endregion

        #region Private Methods
        private DateTime dat()
        {
            if (year < 1900)
            {
                year = 1900;
            }
            if (year > 2100)
            {
                year = 2100;
            }
            if (month < 1)
            {
                month = 1;
            }
            if (month > 12)
            {
                month = 12;
            }
            if (day > 30)
            {
                day = 30;
            }
            if (day < 1)
            {
                day = 1;
            }
            if (hour < 0)
            {
                hour = 0;
            }
            if (hour > 23)
            {
                hour = 23;
            }
            if (minute < 0)
            {
                minute = 0;
            }
            if (minute > 59)
            {
                minute = 59;
            }
            return new DateTime(year, month, day, hour, minute, 0);
        }
        private void CalculateSunPosition(
            DateTime dateTime, double latitude, double longitude)
        {
            // Convert to UTC  
            dateTime = dateTime.ToUniversalTime();

            // Number of days from J2000.0.  
            double julianDate = 367 * dateTime.Year -
                (int)((7.0 / 4.0) * (dateTime.Year +
                (int)((dateTime.Month + 9.0) / 12.0))) +
                (int)((275.0 * dateTime.Month) / 9.0) +
                dateTime.Day - 730531.5;

            double julianCenturies = julianDate / 36525.0;

            // Sidereal Time  
            double siderealTimeHours = 6.6974 + 2400.0513 * julianCenturies;

            double siderealTimeUT = siderealTimeHours +
                (366.2422 / 365.2422) * (double)dateTime.TimeOfDay.TotalHours;

            double siderealTime = siderealTimeUT * 15 + longitude;

            // Refine to number of days (fractional) to specific time.  
            julianDate += (double)dateTime.TimeOfDay.TotalHours / 24.0;
            julianCenturies = julianDate / 36525.0;

            // Solar Coordinates  
            double meanLongitude = CorrectAngle(Deg2Rad *
                (280.466 + 36000.77 * julianCenturies));

            double meanAnomaly = CorrectAngle(Deg2Rad *
                (357.529 + 35999.05 * julianCenturies));

            double equationOfCenter = Deg2Rad * ((1.915 - 0.005 * julianCenturies) *
                Math.Sin(meanAnomaly) + 0.02 * Math.Sin(2 * meanAnomaly));

            double elipticalLongitude =
                CorrectAngle(meanLongitude + equationOfCenter);

            double obliquity = (23.439 - 0.013 * julianCenturies) * Deg2Rad;

            // Right Ascension  
            double rightAscension = Math.Atan2(
                Math.Cos(obliquity) * Math.Sin(elipticalLongitude),
                Math.Cos(elipticalLongitude));

            double declination = Math.Asin(
                Math.Sin(rightAscension) * Math.Sin(obliquity));

            // Horizontal Coordinates  
            double hourAngle = CorrectAngle(siderealTime * Deg2Rad) - rightAscension;

            if (hourAngle > Math.PI)
            {
                hourAngle -= 2 * Math.PI;
            }

            altitude = Math.Asin(Math.Sin(latitude * Deg2Rad) *
                Math.Sin(declination) + Math.Cos(latitude * Deg2Rad) *
                Math.Cos(declination) * Math.Cos(hourAngle));

            // Nominator and denominator for calculating Azimuth  
            // angle. Needed to test which quadrant the angle is in.  
            double aziNom = -Math.Sin(hourAngle);
            double aziDenom =
                Math.Tan(declination) * Math.Cos(latitude * Deg2Rad) -
                Math.Sin(latitude * Deg2Rad) * Math.Cos(hourAngle);

            azimuth = Math.Atan(aziNom / aziDenom);

            if (aziDenom < 0) // In 2nd or 3rd quadrant  
            {
                azimuth += Math.PI;
            }
            else if (aziNom < 0) // In 4th quadrant  
            {
                azimuth += 2 * Math.PI;
            }

            altitude *= Rad2Deg;
            azimuth *= Rad2Deg;
            azimuth += 180;

            // Altitude  
            //Console.WriteLine("Altitude: " + altitude * Rad2Deg);

            // Azimut  
            //Console.WriteLine("Azimuth: " + azimuth * Rad2Deg);
        }

        /*! 
        * \brief Corrects an angle. 
        * 
        * \param angleInRadians An angle expressed in radians. 
        * \return An angle in the range 0 to 2*PI. 
        */
        #endregion

        #region Static Methods
        private static double CorrectAngle(double angleInRadians)
        {
            if (angleInRadians < 0)
            {
                return 2 * Math.PI - (Math.Abs(angleInRadians) % (2 * Math.PI));
            }
            else if (angleInRadians > 2 * Math.PI)
            {
                return angleInRadians % (2 * Math.PI);
            }
            else
            {
                return angleInRadians;
            }
        }
        #endregion
    }
}