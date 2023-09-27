using System;
using Unity.Mathematics;
using UnityEngine;

public class DateSystem
{
    public struct DateString
    {
        public string Date;
        public string Week;
    }

    private static string[] weekString = { " Sunday", " Monday", " Tuesday", " Wednesday", " Thursday", " Friday", " Saturday" };
    private static string[] monthString = { "Jan.", "Feb.", "Mar.", "Apr.", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec" };

    public static DateString DateGenerator(int minutes, int days, int startedYear, bool enableTime)
    {
        DateString outcome;

        int hour = minutes / 60;
        int minute = minutes % 60;

        int year = days / 365 + startedYear;
        days %= 365;

        int month = CalculateMonth(ref days); // Pass days by reference to adjust it
        int day = days + 1;
        int week = CalculateDayOfWeek(year, month, day);

        string _weekString = weekString[week];
        string _date = GenerateDateString(year, month, day, hour, minute, enableTime);

        outcome.Date = _date;
        outcome.Week = _weekString;

        return outcome;
    }

    public static string DateGenerator(int minutes, bool enableTime)
    {
        string outcome;

        int hour = minutes / 60;
        int minute = minutes % 60;

        string timeFormat = enableTime ? "{0}" : "";
        string time = string.Format(timeFormat, $"{hour:00}:{minute:00}");

        outcome = string.Format("{0}", time);

        return outcome;
    }

    private static int CalculateMonth(ref int days)
    {
        int month = 0;
        int daysInMonth = 0;

        while (days > 0)
        {
            daysInMonth = CalculateDaysInMonth(month);

            if (days >= daysInMonth)
            {
                days -= daysInMonth;
                month++;
            }
            else
            {
                break;
            }
        }

        return month;
    }

    private static int CalculateDaysInMonth(int month)
    {
        if (month == 1) // February
        {
            return 28; // Assume non-leap year for simplicity
        }
        else if (month == 3 || month == 5 || month == 8 || month == 10) // April, June, September, November
        {
            return 30;
        }

        return 31; // January, March, May, July, August, October, December
    }

    private static int CalculateDayOfWeek(int year, int month, int day)
    {
        int adjustedMonth = month + 1; // Adjust month to be within the valid range (1 to 12)
        DateTime date = new DateTime(year, adjustedMonth, 1);

        int daysInMonth = DateTime.DaysInMonth(year, adjustedMonth);
        int validDay = Mathf.Clamp(day, 1, daysInMonth); // Ensure day is within valid range

        date = date.AddDays(validDay - 1); // Subtract 1 because DateTime uses 1-based day

        return (int)date.DayOfWeek;
    }

    private static string GenerateDateString(int year, int month, int day, int hour, int minute, bool enableTime)
    {
        string timeFormat = enableTime ? " {0}" : "";
        string time = string.Format(timeFormat, $"{hour:00}:{minute:00}");
        return string.Format("{0} {1} {2} {3}", monthString[month], ConvertToDayString(day), year, time);
    }

    private static string ConvertToDayString(int number)
    {
        string suffix;

        if (number >= 11 && number <= 13)
        {
            suffix = "th";
        }
        else if (number % 10 == 1)
        {
            suffix = "st";
        }
        else if (number % 10 == 2)
        {
            suffix = "nd";
        }
        else if (number % 10 == 3)
        {
            suffix = "rd";
        }
        else
        {
            suffix = "th";
        }

        return $"{number}{suffix}";
    }
}