using Benchmarker.Core;
using Benchmarker.MVVM.Model;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Benchmarker.MVVM.ViewModel
{
	class BenchmarkStartViewModel : ObservableObject
	{
		public RelayCommand SwitchViewCommand { get; set; }
		public RelayCommand SelectProcessCommand { get; set; }

		public KeyValuePair<Process, List<Process>> SelectedProcess { get; set; }

		public int ProgressMinValue { get; set; }
		public int ProgressMaxValue { get; set; }
		public int ProgressValue { get; set; }
		public string ProgressStatus { get; set; }

		public string ShowProgressBind
		{
			get { return showProgress ? "Visible" : "Hidden"; }
		}

		private bool showProgress = false;

		private BackgroundWorker processBackgroundWorker;
		private Dictionary<Process, List<Process>> topLevelProcesses;

		public BenchmarkStartViewModel(RelayCommand switchView)
		{
			SwitchViewCommand = switchView;

			SelectProcessCommand = new RelayCommand(o =>
			{
				SelectProcess();
			});
		}

		public void SelectProcess()
		{
			SetProgressVisibility(true);

			ProcessService.OnProgressSetMax += SetProgressBarMaxValue;
			ProcessService.OnProgressValueUpdate += UpdateProgressBar;
			ProcessService.OnProgressStatusUpdate += SetProgressBarStatus;

			processBackgroundWorker = new BackgroundWorker();

			topLevelProcesses = new Dictionary<Process, List<Process>>();

			processBackgroundWorker.DoWork += (o, e) =>
			{
				topLevelProcesses = ProcessService.GetTopLevelProcesses();
			};

			processBackgroundWorker.RunWorkerAsync();
			processBackgroundWorker.RunWorkerCompleted += OnWorkerComplete;
		}

		public void ResetScreen()
		{
			SetProgressVisibility(false);
			UpdateProgressBar(0);
			ProgressStatus = "";
			OnPropertyChanged(nameof(ProgressStatus));
		}

		private void SetProgressVisibility(bool visible)
		{
			showProgress = visible;
			OnPropertyChanged(nameof(ShowProgressBind));
		}

		private void SetProgressBarMaxValue(int maxValue)
		{
			ProgressMaxValue = maxValue;
			OnPropertyChanged(nameof(ProgressMaxValue));
		}

		private void UpdateProgressBar(int value)
		{
			ProgressValue = value;
			OnPropertyChanged(nameof(ProgressValue));
		}

		private void SetProgressBarStatus(string value)
		{
			ProgressStatus = $"Scanning processes... ({value})";
			OnPropertyChanged(nameof(ProgressStatus));
		}

		private void OnWorkerComplete(object o, RunWorkerCompletedEventArgs e)
		{
			ProgressStatus = $"Scanning complete";
			OnPropertyChanged(nameof(ProgressStatus));

			var processWindow = new ProcessSelectionWindow(topLevelProcesses);
			bool? success = processWindow.ShowDialog();
			if (success == true)
			{
				SelectedProcess = processWindow.ChosenProcess;
				processWindow.Close();
				SwitchViewCommand.Execute(this);
			}

			ProcessService.OnProgressSetMax -= SetProgressBarMaxValue;
			ProcessService.OnProgressValueUpdate -= UpdateProgressBar;
			ProcessService.OnProgressStatusUpdate -= SetProgressBarStatus;
		}
	}
}
