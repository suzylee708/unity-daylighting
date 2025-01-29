using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrydenWoodUnity.Lighting
{
    /// <summary>
    /// A MonoBehaviour component for UI control over the sun position script
    /// </summary>
    public class SunControlUI_ : MonoBehaviour
    {
        #region Public Fields
        [Tooltip("The directional light which is used as a sun")]
        public SunPosition sun;

        [Tooltip("The text to display the Date-Time information")]
        public Text dateTimeText;
        public InputField latInputfield;
        public InputField lonInputfield;
        public Slider timeOfDaySlider;
        public Slider dayOfYearSlider;
        public Button iterationButton;
        public Toggle march;
        public Toggle june;
        public Toggle september;
        public Toggle december;

        public bool useCurrentTimeLocation = true;
        public bool useSpecificDates = true;

        [Header("Iteration Settings")]
        public float startingHour = 6;
        public float endingHour = 20;
        public float minutesStep = 10.0f;
        public bool iterateDay = false;

        public DateTime startDate;
        public DateTime endDate;

        [HideInInspector]
        public DateTime dateTime;
        public DateTime this_is_now;

        #endregion

        #region Private Fields and Properties
        private float lastValue;
        #endregion

        #region MonoBehaviour Methods
        // Use this for initialization
        void Awake()
        {
            try
            {
                //GetGeometries.locationChanged += OnLocationChanged;
                if (useCurrentTimeLocation)
                {
                    if (double.TryParse(latInputfield.text, out sun.lat) && double.TryParse(lonInputfield.text, out sun.lon))
                    {
                        sun.lat = float.Parse(latInputfield.text);
                        sun.lon = float.Parse(lonInputfield.text);
                        var now = DateTime.Now;

                        timeOfDaySlider.value = Mathf.Floor((float)now.TimeOfDay.TotalMinutes);
                        //dayOfYearSlider.value = Mathf.Floor(now.DayOfYear);
                        sun.SetNewDate(now);
                        //===
                        this_is_now = now;
                        dateTimeText.text = now.ToString();
                    }
                }
                else if (useSpecificDates)
                {
                    if (double.TryParse(latInputfield.text, out sun.lat) && double.TryParse(lonInputfield.text, out sun.lon))
                    {
                        sun.lat = float.Parse(latInputfield.text);
                        sun.lon = float.Parse(lonInputfield.text);
                        var now = DateTime.Now;
                        timeOfDaySlider.value = Mathf.Floor((float)now.TimeOfDay.TotalMinutes);
                        //dayOfYearSlider.value = Mathf.Floor(now.DayOfYear);
                        if (march.isOn)
                        {
                            now = new DateTime(now.Year, 3, 21, now.Hour, now.Millisecond, now.Second);
                            //===
                            this_is_now = now;
                        }
                        else if (june.isOn)
                        {
                            now = new DateTime(now.Year, 6, 21, now.Hour, now.Millisecond, now.Second);
                            //===
                            this_is_now = now;
                        }
                        else if (september.isOn)
                        {
                            now = new DateTime(now.Year, 9, 23, now.Hour, now.Millisecond, now.Second);
                            //===
                            this_is_now = now;
                        }
                        else if (december.isOn)
                        {
                            now = new DateTime(now.Year, 12, 21, now.Hour, now.Millisecond, now.Second);
                            //===
                            this_is_now = now;
                        }
                        sun.SetNewDate(now);
                        dateTimeText.text = now.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (iterateDay)
            {
                if (timeOfDaySlider.value <= endingHour * 60.0f)
                {
                    timeOfDaySlider.value += minutesStep;
                }
                else
                {
                    timeOfDaySlider.value = lastValue;
                    ToggleIteration();
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Called when the location of the site has been changed
        /// </summary>
        /// <param name="lat">New Latitude</param>
        /// <param name="lon">New Longitude</param>
        public void OnLocationChanged(float lat, float lon)
        {
            sun.lat = lat;
            sun.lon = lon;
            sun.UpdateDateTime();
        }

        /// <summary>
        /// Sets the current date and time to the sun script
        /// </summary>
        public void SetCurrentDateTime()
        {
            if (double.TryParse(latInputfield.text, out sun.lat) && double.TryParse(lonInputfield.text, out sun.lon))
            {
                sun.lat = float.Parse(latInputfield.text);
                sun.lon = float.Parse(lonInputfield.text);
                var now = DateTime.Now;

                timeOfDaySlider.value = Mathf.Floor((float)now.TimeOfDay.TotalMinutes);
                //dayOfYearSlider.value = Mathf.Floor(now.DayOfYear);
                sun.SetNewDate(now);
                this_is_now = now;
                dateTimeText.text = now.ToString();
            }
        }

        /// <summary>
        /// Sets the time of day
        /// </summary>
        /// <param name="minutes">Tha time of day in total minutes (from 00:00)</param>
        public void SetTimeOfDay(float minutes)
        {
            var curDate = sun.date;
            var hours = Mathf.Floor(minutes / 60.0f);
            var mins = minutes - (hours * 60);
            var newDate = new DateTime(curDate.Year, curDate.Month, curDate.Day, 0, 0, 0) + new TimeSpan((int)hours, (int)mins, 0);
            sun.SetNewDate(newDate);
            sun.UpdateDateTime();
            this_is_now = newDate;
            dateTimeText.text = sun.date.ToString();
        }

        /// <summary>
        /// Sets the day of the year
        /// </summary>
        /// <param name="day">The total days ofthe year (from 1/1)</param>
        public void SetDayOfYear(float day)
        {
            var curDate = sun.date;
            var newDate = new DateTime(curDate.Year, 1, 1, curDate.Hour, curDate.Minute, curDate.Second) + new TimeSpan((int)day, 0, 0, 0);
            sun.SetNewDate(newDate);
            this_is_now = newDate;
            dateTimeText.text = sun.date.ToString();
        }

        /// <summary>
        /// Sets the latitude
        /// </summary>
        /// <param name="lat">New Latitude</param>
        public void SetLatitude(string lat)
        {
            sun.lat = float.Parse(lat);
            sun.UpdateDateTime();
        }

        /// <summary>
        /// Sets the Longitude
        /// </summary>
        /// <param name="lon">New Longitude</param>
        public void SetLongitude(string lon)
        {
            sun.lon = float.Parse(lon);
            sun.UpdateDateTime();
        }

        /// <summary>
        /// Toggles the automated iteration through the hours of day
        /// </summary>
        public void ToggleIteration()
        {
            iterateDay = !iterateDay;
            if (iterateDay)
            {
                lastValue = timeOfDaySlider.value;
                iterationButton.transform.GetChild(0).GetComponent<Text>().text = "STOP";
                timeOfDaySlider.value = startingHour * 60.0f;
            }
            else
            {
                iterationButton.transform.GetChild(0).GetComponent<Text>().text = "START";
            }
        }

        /// <summary>
        /// Called when the 21st of March is set as date
        /// </summary>
        /// <param name="toggle">Whether it was pressed</param>
        public void OnMarchPressed(bool toggle)
        {
            if (toggle)
            {
                var curDate = sun.date;
                var hours = Mathf.Floor(timeOfDaySlider.value / 60.0f);
                var mins = timeOfDaySlider.value - (hours * 60);
                var newDate = new DateTime(curDate.Year, 3, 21, 0, 0, 0) + new TimeSpan((int)hours, (int)mins, 0);
                sun.SetNewDate(newDate);
                this_is_now = newDate;
                dateTimeText.text = sun.date.ToString();
            }
        }

        /// <summary>
        /// Called when the 21st of June is set as date
        /// </summary>
        /// <param name="toggle">Whether it was pressed</param>
        public void OnJunePressed(bool toggle)
        {
            if (toggle)
            {
                var curDate = sun.date;
                var hours = Mathf.Floor(timeOfDaySlider.value / 60.0f);
                var mins = timeOfDaySlider.value - (hours * 60);
                var newDate = new DateTime(curDate.Year, 6, 21, 0, 0, 0) + new TimeSpan((int)hours, (int)mins, 0);
                sun.SetNewDate(newDate);
                this_is_now = newDate;
                dateTimeText.text = sun.date.ToString();
            }
        }

        /// <summary>
        /// Called when the 23rd of September is set as date
        /// </summary>
        /// <param name="toggle">Whether it was pressed</param>
        public void OnSeptemberPressed(bool toggle)
        {
            if (toggle)
            {
                var curDate = sun.date;
                var hours = Mathf.Floor(timeOfDaySlider.value / 60.0f);
                var mins = timeOfDaySlider.value - (hours * 60);
                var newDate = new DateTime(curDate.Year, 9, 23, 0, 0, 0) + new TimeSpan((int)hours, (int)mins, 0);
                sun.SetNewDate(newDate);
                this_is_now = newDate;
                dateTimeText.text = sun.date.ToString();
            }
        }

        /// <summary>
        /// Called when the 21st of December is set as date
        /// </summary>
        /// <param name="toggle">Whether it was pressed</param>
        public void OnDecemberPressed(bool toggle)
        {
            if (toggle)
            {
                var curDate = sun.date;
                var hours = Mathf.Floor(timeOfDaySlider.value / 60.0f);
                var mins = timeOfDaySlider.value - (hours * 60);
                var newDate = new DateTime(curDate.Year, 12, 21, 0, 0, 0) + new TimeSpan((int)hours, (int)mins, 0);
                sun.SetNewDate(newDate);
                this_is_now = newDate;
                dateTimeText.text = sun.date.ToString();
            }

        }

        #endregion
    }
}
