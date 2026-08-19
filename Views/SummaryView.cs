using System;
using InfoMasterKonsole.ViewModels;

namespace InfoMasterKonsole.Views
{
    public class SummaryView : IView
    {
        private readonly SummaryViewModel _viewModel;

        public SummaryView(SummaryViewModel viewModel)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }

        public void Show()
        {
            try
            {
                ScreenHelper.ShowTitle("Customer Summary/Statistics");
                var summary = _viewModel.GetSummary();

                Console.WriteLine($"Total customers: {summary.TotalCustomers}");

                Console.WriteLine("\nCustomers by type:");
                if (summary.CustomersByType.Count == 0)
                {
                    Console.WriteLine("  (no types available)");
                }
                else
                {
                    foreach (var kv in summary.CustomersByType)
                    {
                        Console.WriteLine($"  {kv.Key}: {kv.Value}");
                    }
                }

                Console.WriteLine();
                if (summary.EarliestRegistration.HasValue)
                {
                    Console.WriteLine($"Earliest registration: {summary.EarliestRegistration:yyyy-MM-dd}");
                }
                else
                {
                    Console.WriteLine("Earliest registration: N/A");
                }

                if (summary.LatestRegistration.HasValue)
                {
                    Console.WriteLine($"Latest registration:   {summary.LatestRegistration:yyyy-MM-dd}");
                }
                else
                {
                    Console.WriteLine("Latest registration:   N/A");
                }

                Console.WriteLine("----------------------------");
                ScreenHelper.PauseAndClear();
            }
            catch (Exception ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                Console.WriteLine("An error occurred while generating the summary. See logs for details.");
                ScreenHelper.PauseAndClear();
            }
        }
    }
}
