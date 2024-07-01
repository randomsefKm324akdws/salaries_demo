namespace bl.Helpers;

internal static class DateHelpers
{
	public static int YearsBetweenDates(DateTime start, DateTime end)
	{
		return end.Year - start.Year - 1 +
		       (end.Month > start.Month ||
		        (end.Month == start.Month && end.Day >= start.Day)
			       ? 1
			       : 0);
	}
}