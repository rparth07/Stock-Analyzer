using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Service.ScrapperJob
{
  public class ScrapperScheduler
  {
    private Timer Timer8am;
    private Timer Timer6pm;
    private Timer Timer7pm;
    private Timer Timer8pm;

    private readonly ScrapperJob ScrapperJob;

    public ScrapperScheduler(ScrapperJob job)
    {
      ScrapperJob = job;
    }

    public void Start()
    {
      ScheduleJobAt(8, 0);  // 8am
      ScheduleJobAt(18, 0); // 6pm
      ScheduleJobAt(19, 0); // 7pm
      ScheduleJobAt(20, 0); // 8pm
    }

    private void ScheduleJobAt(int hour, int minute)
    {
      // Get the current time
      DateTime now = DateTime.Now;

      // Calculate the time until the next occurrence of the specified time
      DateTime nextScheduledTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);
      if (now > nextScheduledTime)
      {
        nextScheduledTime = nextScheduledTime.AddDays(1);
      }

      // Set up the timer to run the job at the specified time every day
      Timer timer = new Timer(ExecuteJob, null, nextScheduledTime - now, TimeSpan.FromHours(24));

      // Assign the timer to the appropriate field based on the specified time
      if (hour == 8 && minute == 0)
      {
        Timer8am = timer;
      }
      else if (hour == 18 && minute == 0)
      {
        Timer6pm = timer;
      }
      else if (hour == 19 && minute == 0)
      {
        Timer7pm = timer;
      }
      else if (hour == 20 && minute == 0)
      {
        Timer8pm = timer;
      }
    }

    private void ExecuteJob(object state)
    {
      ScrapperJob.ExecuteAsync();
    }

    public void Stop()
    {
      // Stop the timer when needed
      Timer8am?.Change(Timeout.Infinite, 0);
      Timer6pm?.Change(Timeout.Infinite, 0);
      Timer7pm?.Change(Timeout.Infinite, 0);
      Timer8pm?.Change(Timeout.Infinite, 0);
    }
  }
}
