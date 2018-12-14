using PianificazioneService.Properties;
using ScheduleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf.Logging;

namespace PianificazioneService
{
    public class WindowsService
    {
        private LogWriter _log = HostLogger.Get<WindowsService>();
        private Timer _timer;
        public void Start()
        {
            try
            {
                string messaggio = string.Format("Timer impostato a {0} minuti", Settings.Default.TimerPeriod);
                _timer = new Timer(TimerCallBack, null, 1L, Settings.Default.TimerPeriod * 60 * 1000);
                _log.Info("Servizio Pianificazione avviato");

            }
            catch (Exception ex)
            {
                _log.Error("Errore in fase di start del servizio", ex);

            }
        }

        public void Stop()
        {
            try
            {
                _timer.Dispose();
                _log.Info("Servizio Pianificazione fermato");
            }
            catch (Exception ex)
            {
                _log.Error("Errore in fase di stop del servizio", ex);
            }
        }

        private void TimerCallBack(Object stateInfo)
        {
            try
            {
                ScheduleService.ScheduleService sPianificazione = new ScheduleService.ScheduleService();
                ScheduleDS.MONITOR_SCHEDULERRow schedulazione;
                if (sPianificazione.VerificaEsecuzione("PIANIFICAZIONE_1", out schedulazione))
                {
                    Pianificazione.Service.PianificazioneService pianificazione = new Pianificazione.Service.PianificazioneService();
                    //MagazzinoMonitor mMagazzino = new MagazzinoMonitor();
                    //mMagazzino.VerificaSaldiNegativi();
                    sPianificazione.AggiornaSchedulazione(schedulazione);
                }

                if (sPianificazione.VerificaEsecuzione("PIANIFICAZIONE_2", out schedulazione))
                {
                    Pianificazione.Service.PianificazioneService pianificazione = new Pianificazione.Service.PianificazioneService();
                    //MagazzinoMonitor mMagazzino = new MagazzinoMonitor();
                    //mMagazzino.VerificaSaldiNegativi();
                    sPianificazione.AggiornaSchedulazione(schedulazione);
                }

                if (sPianificazione.VerificaEsecuzione("PIANIFICAZIONE_3", out schedulazione))
                {
                    Pianificazione.Service.PianificazioneService pianificazione = new Pianificazione.Service.PianificazioneService();
                    //MagazzinoMonitor mMagazzino = new MagazzinoMonitor();
                    //mMagazzino.VerificaSaldiNegativi();
                    sPianificazione.AggiornaSchedulazione(schedulazione);
                }

            }
            catch (Exception ex)
            {
                _log.Error("Errore Servizio Pianificazione", ex);
                while (ex.InnerException != null)
                {
                    _log.Error("--- INNER EXCEPTION", ex);
                    ex = ex.InnerException;
                }
            }
            finally
            {
            }
        }
    }
}
