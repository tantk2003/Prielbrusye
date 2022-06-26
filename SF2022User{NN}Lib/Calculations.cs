using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF2022User_NN_Lib
{
    public class Calculations
    {
        public static string[] AvailablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
        {
            List<string> times = new List<string>();
            List<TimeSpan> starts = new List<TimeSpan>();
            starts.AddRange(startTimes);
            starts.Add(endWorkingTime);

            List<int> durs = new List<int>();
            durs.AddRange(durations);
            durs.Add(consultationTime);

            while (beginWorkingTime.Add(new TimeSpan(0, consultationTime, 0)) < endWorkingTime)
            {
                for (int i = 0; i < startTimes.Length; i++)
                {
                    if (beginWorkingTime.TotalSeconds >= starts[i].TotalSeconds
                        && beginWorkingTime.TotalSeconds <= starts[i].TotalSeconds + durs[i]
                        || beginWorkingTime.TotalSeconds + consultationTime * 60 >= starts[i].TotalSeconds
                        && (beginWorkingTime.TotalSeconds + consultationTime * 60) <= (starts[i].TotalSeconds + durs[i] * 60)
                        || beginWorkingTime.TotalSeconds + consultationTime * 60 >= starts[i + 1].TotalSeconds
                        && beginWorkingTime.TotalSeconds + consultationTime * 60 <= starts[i + 1].TotalSeconds + durs[i + 1])
                    {
                        beginWorkingTime = beginWorkingTime.Add(new TimeSpan(0, durs[i], 0));
                        continue;
                    }
                }

                double secondsStart = beginWorkingTime.TotalSeconds;

                double startMins = secondsStart % 3600 / 60;
                secondsStart -= startMins * 60;
                double startHour = secondsStart / 3600;

                double secondsEnd = beginWorkingTime.TotalSeconds + consultationTime * 60;

                double EndMins = secondsEnd % 3600 / 60;
                secondsEnd -= EndMins * 60;
                double EndHour = secondsEnd / 3600;

                times.Add($"{startHour.ToString("00")}:{startMins.ToString("00")} - {EndHour.ToString("00")}:{EndMins.ToString("00")}");

                beginWorkingTime = beginWorkingTime.Add(new TimeSpan(0, consultationTime, 0));
            }

            string[] r = new string[times.Count];
            for (int i = 0; i < times.Count; i++)
            {
                r[i] = times[i];
            }
            return r;
        }
    }
}
