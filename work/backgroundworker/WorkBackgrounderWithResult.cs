using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.work.backgroundworker
{
    public class WorkBackgrounderWithResult<T1, T2>
    {
        readonly BackgroundWorker _innerBg = new BackgroundWorker();
        private Func<T1, BackgroundWorker, T2> _workAction;
        private Action<T2> _finishAction;
        private Action<Exception> _finishWithErrorAction;

        public bool IsCompleted { get; private set; }

        public Action<int, object> ReportProgressAction { get; set; }

        public WorkBackgrounderWithResult()
        {
            _innerBg.DoWork += DoWork;
            _innerBg.RunWorkerCompleted += RunWorkerCompleted;
        }

        public Func<T1, BackgroundWorker, T2> WorkAction
        {
            get => _workAction;
            set
            {
                if (_innerBg.IsBusy)
                {
                    throw new Exception("Impossible de modifier l'action de travail quand le travail est en cours.");
                }
                _workAction = value;
            }
        }
        public Action<T2> FinishAction
        {
            get => _finishAction;
            set
            {
                if (_innerBg.IsBusy)
                {
                    throw new Exception("Impossible de modifier l'action de fin quand le travail est en cours.");
                }
                _finishAction = value;
            }
        }

        public Action<Exception> FinishWithErrorAction
        {
            get => _finishWithErrorAction;
            set
            {
                if (_innerBg.IsBusy)
                {
                    throw new Exception("Impossible de modifier l'action de fin quand le travail est en cours.");
                }
                _finishWithErrorAction = value;
            }
        }

        public bool CanBeCanceled
        {
            get => _innerBg.WorkerSupportsCancellation;
            set
            {
                if (_innerBg.IsBusy)
                {
                    throw new Exception("Impossible de modifier cette propriété quand le travail est en cours.");
                }
                _innerBg.WorkerSupportsCancellation = value;
            }
        }



        public bool RunAsync(T1 args, Func<T1, BackgroundWorker, T2> doWork = null, Action<T2> finishCallback = null)
        {
            if (doWork != null)
            {
                WorkAction = doWork;
            }

            if (finishCallback != null)
            {
                FinishAction = finishCallback;
            }

            if (ReportProgressAction != null)
            {
                _innerBg.WorkerReportsProgress = true;
                _innerBg.ProgressChanged += ReportProgress;
            }


            if (WorkAction == null)
            {
                throw new Exception("Les action de travail et de fin ne sont pas paramétrées.");
            }


            _innerBg.RunWorkerAsync(args);

            return true;
        }



        private void DoWork(object sender, DoWorkEventArgs e)
        {
            T1 args = (T1)e.Argument;
            T2 res = WorkAction.Invoke(args, _innerBg);
            e.Result = res;
        }

        private void ReportProgress(object sender, ProgressChangedEventArgs e)
        {
            ReportProgressAction?.Invoke(e.ProgressPercentage, e.UserState);
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsCompleted = true;
            if (e.Error == null)
            {
                FinishAction?.Invoke((T2)e.Result);
            }
            else
            {
                if (FinishWithErrorAction != null)
                {
                    FinishWithErrorAction.Invoke(e.Error);
                }
                else
                {
                    throw e.Error;
                }
            }

        }

        public void SendCancel()
        {
            if (_innerBg.IsBusy && _innerBg.WorkerSupportsCancellation)
            {
                _innerBg.CancelAsync();
            }
        }
    }
}
