namespace Polytech.Common.Telemetron
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using Polytech.Common.Telemetron;
	using Polytech.Common.Telemetron.Configuration;

    public class ConsoleConfiguration : TelemetronConfigurationBase, IConsoleConfiguration
    {
	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.Debug"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? DebugForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.Debug"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? DebugBackgroundColor { get; set; }

	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.Verbose"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? VerboseForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.Verbose"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? VerboseBackgroundColor { get; set; }

	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.Info"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? InfoForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.Info"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? InfoBackgroundColor { get; set; }

	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.Warning"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? WarningForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.Warning"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? WarningBackgroundColor { get; set; }

	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.Error"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? ErrorForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.Error"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? ErrorBackgroundColor { get; set; }

	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.Fatal"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? FatalForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.Fatal"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? FatalBackgroundColor { get; set; }

	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.OperationInfo"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? OperationInfoForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.OperationInfo"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? OperationInfoBackgroundColor { get; set; }

	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.OperationError"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? OperationErrorForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.OperationError"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? OperationErrorBackgroundColor { get; set; }

	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.Metric"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? MetricForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.Metric"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? MetricBackgroundColor { get; set; }

	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.Event"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? EventForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.Event"/> severity events that are emitted. 
        /// </summary>
        public ConsoleColor? EventBackgroundColor { get; set; }

    }

	public interface IConsoleConfiguration : ITelemetronConfigurationBase
    {
	    /// <summary>
        /// Gets the foreground color for <see cref="EventSeverity.Debug"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? DebugForegroundColor { get; }

		/// <summary>
        /// Gets the background color for <see cref="EventSeverity.Debug"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? DebugBackgroundColor { get; }

	    /// <summary>
        /// Gets the foreground color for <see cref="EventSeverity.Verbose"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? VerboseForegroundColor { get; }

		/// <summary>
        /// Gets the background color for <see cref="EventSeverity.Verbose"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? VerboseBackgroundColor { get; }

	    /// <summary>
        /// Gets the foreground color for <see cref="EventSeverity.Info"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? InfoForegroundColor { get; }

		/// <summary>
        /// Gets the background color for <see cref="EventSeverity.Info"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? InfoBackgroundColor { get; }

	    /// <summary>
        /// Gets the foreground color for <see cref="EventSeverity.Warning"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? WarningForegroundColor { get; }

		/// <summary>
        /// Gets the background color for <see cref="EventSeverity.Warning"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? WarningBackgroundColor { get; }

	    /// <summary>
        /// Gets the foreground color for <see cref="EventSeverity.Error"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? ErrorForegroundColor { get; }

		/// <summary>
        /// Gets the background color for <see cref="EventSeverity.Error"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? ErrorBackgroundColor { get; }

	    /// <summary>
        /// Gets the foreground color for <see cref="EventSeverity.Fatal"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? FatalForegroundColor { get; }

		/// <summary>
        /// Gets the background color for <see cref="EventSeverity.Fatal"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? FatalBackgroundColor { get; }

	    /// <summary>
        /// Gets the foreground color for <see cref="EventSeverity.OperationInfo"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? OperationInfoForegroundColor { get; }

		/// <summary>
        /// Gets the background color for <see cref="EventSeverity.OperationInfo"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? OperationInfoBackgroundColor { get; }

	    /// <summary>
        /// Gets the foreground color for <see cref="EventSeverity.OperationError"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? OperationErrorForegroundColor { get; }

		/// <summary>
        /// Gets the background color for <see cref="EventSeverity.OperationError"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? OperationErrorBackgroundColor { get; }

	    /// <summary>
        /// Gets the foreground color for <see cref="EventSeverity.Metric"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? MetricForegroundColor { get; }

		/// <summary>
        /// Gets the background color for <see cref="EventSeverity.Metric"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? MetricBackgroundColor { get; }

	    /// <summary>
        /// Gets the foreground color for <see cref="EventSeverity.Event"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? EventForegroundColor { get; }

		/// <summary>
        /// Gets the background color for <see cref="EventSeverity.Event"/> severity events that are emitted. 
        /// </summary>
        ConsoleColor? EventBackgroundColor { get; }

    }

	public partial class ConsoleTelemetron 
	{

	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.Debug"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor debugForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.Debug"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor debugBackgroundColor { get; set; }


	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.Verbose"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor verboseForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.Verbose"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor verboseBackgroundColor { get; set; }


	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.Info"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor infoForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.Info"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor infoBackgroundColor { get; set; }


	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.Warning"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor warningForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.Warning"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor warningBackgroundColor { get; set; }


	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.Error"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor errorForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.Error"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor errorBackgroundColor { get; set; }


	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.Fatal"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor fatalForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.Fatal"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor fatalBackgroundColor { get; set; }


	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.OperationInfo"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor operationInfoForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.OperationInfo"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor operationInfoBackgroundColor { get; set; }


	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.OperationError"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor operationErrorForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.OperationError"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor operationErrorBackgroundColor { get; set; }


	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.Metric"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor metricForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.Metric"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor metricBackgroundColor { get; set; }


	    /// <summary>
        /// Gets or sets the foreground color for <see cref="EventSeverity.Event"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor eventForegroundColor { get; set; }

		/// <summary>
        /// Gets or sets the background color for <see cref="EventSeverity.Event"/> severity events that are emitted. 
        /// </summary>
        private ConsoleColor eventBackgroundColor { get; set; }


		private void CopyConfigLocal(IConsoleConfiguration configuration)
		{
			//////
			// debug
			if (!configuration.DebugForegroundColor.HasValue) 
			{
				this.debugForegroundColor = ConsoleColor.DarkGray;
			}
			else 
			{
				this.debugForegroundColor = configuration.DebugForegroundColor.Value;
			}

			if (!configuration.DebugBackgroundColor.HasValue) 
			{
				this.debugBackgroundColor = ConsoleColor.Black;
			}
			else 
			{
				this.debugBackgroundColor = configuration.DebugBackgroundColor.Value;
			}

			//////
			// verbose
			if (!configuration.VerboseForegroundColor.HasValue) 
			{
				this.verboseForegroundColor = ConsoleColor.Gray;
			}
			else 
			{
				this.verboseForegroundColor = configuration.VerboseForegroundColor.Value;
			}

			if (!configuration.VerboseBackgroundColor.HasValue) 
			{
				this.verboseBackgroundColor = ConsoleColor.Black;
			}
			else 
			{
				this.verboseBackgroundColor = configuration.VerboseBackgroundColor.Value;
			}

			//////
			// info
			if (!configuration.InfoForegroundColor.HasValue) 
			{
				this.infoForegroundColor = ConsoleColor.White;
			}
			else 
			{
				this.infoForegroundColor = configuration.InfoForegroundColor.Value;
			}

			if (!configuration.InfoBackgroundColor.HasValue) 
			{
				this.infoBackgroundColor = ConsoleColor.Black;
			}
			else 
			{
				this.infoBackgroundColor = configuration.InfoBackgroundColor.Value;
			}

			//////
			// warning
			if (!configuration.WarningForegroundColor.HasValue) 
			{
				this.warningForegroundColor = ConsoleColor.Yellow;
			}
			else 
			{
				this.warningForegroundColor = configuration.WarningForegroundColor.Value;
			}

			if (!configuration.WarningBackgroundColor.HasValue) 
			{
				this.warningBackgroundColor = ConsoleColor.Black;
			}
			else 
			{
				this.warningBackgroundColor = configuration.WarningBackgroundColor.Value;
			}

			//////
			// error
			if (!configuration.ErrorForegroundColor.HasValue) 
			{
				this.errorForegroundColor = ConsoleColor.Red;
			}
			else 
			{
				this.errorForegroundColor = configuration.ErrorForegroundColor.Value;
			}

			if (!configuration.ErrorBackgroundColor.HasValue) 
			{
				this.errorBackgroundColor = ConsoleColor.Black;
			}
			else 
			{
				this.errorBackgroundColor = configuration.ErrorBackgroundColor.Value;
			}

			//////
			// fatal
			if (!configuration.FatalForegroundColor.HasValue) 
			{
				this.fatalForegroundColor = ConsoleColor.White;
			}
			else 
			{
				this.fatalForegroundColor = configuration.FatalForegroundColor.Value;
			}

			if (!configuration.FatalBackgroundColor.HasValue) 
			{
				this.fatalBackgroundColor = ConsoleColor.DarkRed;
			}
			else 
			{
				this.fatalBackgroundColor = configuration.FatalBackgroundColor.Value;
			}

			//////
			// operationInfo
			if (!configuration.OperationInfoForegroundColor.HasValue) 
			{
				this.operationInfoForegroundColor = ConsoleColor.DarkGray;
			}
			else 
			{
				this.operationInfoForegroundColor = configuration.OperationInfoForegroundColor.Value;
			}

			if (!configuration.OperationInfoBackgroundColor.HasValue) 
			{
				this.operationInfoBackgroundColor = ConsoleColor.Black;
			}
			else 
			{
				this.operationInfoBackgroundColor = configuration.OperationInfoBackgroundColor.Value;
			}

			//////
			// operationError
			if (!configuration.OperationErrorForegroundColor.HasValue) 
			{
				this.operationErrorForegroundColor = ConsoleColor.DarkGray;
			}
			else 
			{
				this.operationErrorForegroundColor = configuration.OperationErrorForegroundColor.Value;
			}

			if (!configuration.OperationErrorBackgroundColor.HasValue) 
			{
				this.operationErrorBackgroundColor = ConsoleColor.Black;
			}
			else 
			{
				this.operationErrorBackgroundColor = configuration.OperationErrorBackgroundColor.Value;
			}

			//////
			// metric
			if (!configuration.MetricForegroundColor.HasValue) 
			{
				this.metricForegroundColor = ConsoleColor.Magenta;
			}
			else 
			{
				this.metricForegroundColor = configuration.MetricForegroundColor.Value;
			}

			if (!configuration.MetricBackgroundColor.HasValue) 
			{
				this.metricBackgroundColor = ConsoleColor.Black;
			}
			else 
			{
				this.metricBackgroundColor = configuration.MetricBackgroundColor.Value;
			}

			//////
			// event
			if (!configuration.EventForegroundColor.HasValue) 
			{
				this.eventForegroundColor = ConsoleColor.Cyan;
			}
			else 
			{
				this.eventForegroundColor = configuration.EventForegroundColor.Value;
			}

			if (!configuration.EventBackgroundColor.HasValue) 
			{
				this.eventBackgroundColor = ConsoleColor.Black;
			}
			else 
			{
				this.eventBackgroundColor = configuration.EventBackgroundColor.Value;
			}

	
		}

		private ConsoleColor GetForegroundColor(EventSeverity es)
		{
			switch (es)
			{
				case EventSeverity.Debug:
					return this.debugForegroundColor;
				case EventSeverity.Verbose:
					return this.verboseForegroundColor;
				case EventSeverity.Info:
					return this.infoForegroundColor;
				case EventSeverity.Warning:
					return this.warningForegroundColor;
				case EventSeverity.Error:
					return this.errorForegroundColor;
				case EventSeverity.Fatal:
					return this.fatalForegroundColor;
				case EventSeverity.OperationInfo:
					return this.operationInfoForegroundColor;
				case EventSeverity.OperationError:
					return this.operationErrorForegroundColor;
				case EventSeverity.Metric:
					return this.metricForegroundColor;
				case EventSeverity.Event:
					return this.eventForegroundColor;
				default: 
					return this.infoForegroundColor;
			}
		}

		
		private ConsoleColor GetBackgroundColor(EventSeverity es)
		{
			switch (es)
			{
				case EventSeverity.Debug:
					return this.debugBackgroundColor;
				case EventSeverity.Verbose:
					return this.verboseBackgroundColor;
				case EventSeverity.Info:
					return this.infoBackgroundColor;
				case EventSeverity.Warning:
					return this.warningBackgroundColor;
				case EventSeverity.Error:
					return this.errorBackgroundColor;
				case EventSeverity.Fatal:
					return this.fatalBackgroundColor;
				case EventSeverity.OperationInfo:
					return this.operationInfoBackgroundColor;
				case EventSeverity.OperationError:
					return this.operationErrorBackgroundColor;
				case EventSeverity.Metric:
					return this.metricBackgroundColor;
				case EventSeverity.Event:
					return this.eventBackgroundColor;
				default: 
					return this.infoBackgroundColor;
			}
		}
	}
}
