using System;
using System.IO;
using System.Windows;
using Referee.Infrastructure.SettingsFd;
#if !DEBUG
using Referee.Infrastructure.Update;
#endif
using Referee.ViewModels;
using Serilog;
using Serilog.Enrichers.CallerInfo;
using Serilog.Events;

namespace Referee
{
	public partial class App : Application
	{
		private static readonly ILogger _logger = new LoggerConfiguration()
		                                          .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information,
			                                          outputTemplate:
			                                          "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] [{ThreadId}] [{SourceContextName}.{Method}] {Message}{NewLine}{Exception}")
		                                          .WriteTo.File("events.log",
			                                          outputTemplate:
			                                          "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] [{ThreadId}] [{SourceContextName}.{Method}] {Message}{NewLine}{Exception}")
		                                          .Enrich.WithComputed("SourceContextName",
			                                          "Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1)")
		                                          .Enrich.WithThreadId() // Or Name
		                                          .Enrich.WithCallerInfo(false, "")
		                                          .CreateLogger().ForContext<App>();


		protected override async void OnStartup(StartupEventArgs e)
		{
			_logger.Information("starting application");
			var settings = await SettingsHelper.LoadSettings();
			var db = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Referee.db");
			if (!File.Exists(Constants.DatabasePath))
			{
				_logger.Warning("settings does not exists, creating settings");
				File.Copy(db, Constants.DatabasePath);
			}

			Window window = new MainWindow(new MainViewModel(settings, _logger));
			window.Show();

			base.OnStartup(e);
#if !DEBUG
			await Updater.CheckVersion(_logger).ConfigureAwait(false);
#endif
		}
	}
}
